using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using WPF.UIData;
using Services;
using Repositories;

namespace WPF.Features.Budget
{
    /// <summary>
    /// Model đại diện cho các thẻ thống kê tổng quan ngân sách (Tổng, Đã chi, Còn lại, Tỷ lệ).
    /// </summary>
    public class BudgetStatCardModel : INotifyPropertyChanged
    {
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string AccentColor { get; set; } = "#10d9a0";
        public Brush AccentBrush => new SolidColorBrush((Color)ColorConverter.ConvertFromString(AccentColor));

        private decimal _targetValue;
        public decimal TargetValue { get => _targetValue; set { _targetValue = value; } }

        private decimal _value;
        public decimal Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); OnPropertyChanged(nameof(FormattedValue)); }
        }

        public bool IsPercentage { get; set; }

        public string FormattedValue
        {
            get
            {
                if (IsPercentage) return Value.ToString("0.0") + "%";
                return Value >= 1_000_000
                    ? (Value / 1_000_000).ToString("0.#") + "Mđ"
                    : (Value / 1_000).ToString("0") + "Kđ";
            }
        }

        /// <summary>
        /// Tạo hiệu ứng animation chạy số tiền từ 0 lên giá trị thực tế.
        /// </summary>
        public void Animate()
        {
            int steps = 35;
            int tick = 0;
            decimal target = TargetValue;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
            timer.Tick += (s, e) =>
            {
                tick++;
                double t = (double)tick / steps;
                double eased = 1 - Math.Pow(1 - t, 3);
                Value = (decimal)(eased * (double)target);
                if (tick >= steps) { Value = target; timer.Stop(); }
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// UserControl hiển thị màn hình Quản lý Ngân Sách.
    /// Cho phép xem tổng quan, chi tiết từng danh mục, và theo dõi tiến độ chi tiêu.
    /// </summary>
    public partial class BudgetsView : UserControl, INotifyPropertyChanged
    {
        // --- CÁC PROPERTY RÀNG BUỘC (BINDING) ---

        public BudgetStatCardModel CardTotalBudget { get; } = new() { Label = "Tổng ngân sách", Icon = "💰", AccentColor = "#3b82f6" };
        public BudgetStatCardModel CardSpent       { get; } = new() { Label = "Đã chi tiêu",    Icon = "🔥", AccentColor = "#f43f5e" };
        public BudgetStatCardModel CardRemaining   { get; } = new() { Label = "Còn khả dụng",   Icon = "✨", AccentColor = "#10d9a0" };
        public BudgetStatCardModel CardSavingRate  { get; } = new() { Label = "Tỷ lệ tiết kiệm",Icon = "📈", AccentColor = "#7c6df8", IsPercentage = true };

        public ObservableCollection<BudgetData> AllBudgets { get; set; }

        public ISeries[] BarChartSeries { get; set; }
        public Axis[] BarChartXAxes { get; set; }
        public Axis[] BarChartYAxes { get; set; }

        public ISeries[] DonutSeries { get; set; }
        
        public ISeries[] AreaChartSeries { get; set; }
        public Axis[] AreaChartXAxes { get; set; }
        public Axis[] AreaChartYAxes { get; set; }

        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#6b7280")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        private bool _hasAlert;
        public bool HasAlert
        {
            get => _hasAlert;
            set { _hasAlert = value; OnPropertyChanged(); }
        }

        private string _alertMessage = string.Empty;
        public string AlertMessage
        {
            get => _alertMessage;
            set { _alertMessage = value; OnPropertyChanged(); }
        }

        private readonly IBudgetService _budgetService;

        public BudgetsView()
        {
            InitializeComponent();
            this.DataContext = this;

            // Initialize services
            _budgetService = new BudgetService(new BudgetRepository(), new TransactionRepository());

            AllBudgets = new ObservableCollection<BudgetData>();

            // Load real data when view loads
            this.Loaded += BudgetsView_Loaded;
            
            var t2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            t2.Tick += (s, e) => { t2.Stop(); CardTotalBudget.Animate(); CardSpent.Animate(); CardRemaining.Animate(); CardSavingRate.Animate(); };
            t2.Start();
        }

        private async void BudgetsView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadBudgetDataAsync();
        }

        private double[] _trendBudgets = new double[6];
        private double[] _trendSpents = new double[6];
        private string[] _trendLabels = new string[6];

        public async System.Threading.Tasks.Task LoadBudgetDataAsync()
        {
            int currentUserId = 1; // Giả định user hiện tại là 1
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            try
            {
                // Fetch 6-month trend data
                for (int i = 5; i >= 0; i--)
                {
                    var dt = DateTime.Now.AddMonths(-i);
                    _trendLabels[5 - i] = $"Th {dt.Month}";
                    var history = await _budgetService.GetBudgetProgressesAsync(currentUserId, dt.Month, dt.Year);
                    _trendBudgets[5 - i] = (double)history.Sum(p => p.AmountLimit);
                    _trendSpents[5 - i] = (double)history.Sum(p => p.SpentAmount);
                }

                var budgetProgresses = await _budgetService.GetBudgetProgressesAsync(currentUserId, currentMonth, currentYear);

                AllBudgets.Clear();
                foreach (var progress in budgetProgresses)
                {
                    AllBudgets.Add(new BudgetData
                    {
                        BudgetId = progress.BudgetId,
                        CategoryId = progress.CategoryId,
                        CategoryName = progress.CategoryName,
                        Icon = "🏷️", // Default icon
                        SpentAmount = progress.SpentAmount,
                        TotalAmount = progress.AmountLimit
                    });
                }

                OnPropertyChanged(nameof(AllBudgets));

                foreach (var b in AllBudgets) b.AnimateProgress();

                CalculateStats();
                SetupCharts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ngân sách: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateStats()
        {
            decimal totalBudget = AllBudgets.Sum(b => b.TotalAmount);
            decimal totalSpent = AllBudgets.Sum(b => b.SpentAmount);
            
            CardTotalBudget.TargetValue = totalBudget;
            CardTotalBudget.Value = totalBudget;
            
            CardSpent.TargetValue = totalSpent;
            CardSpent.Value = totalSpent;
            
            CardRemaining.TargetValue = Math.Max(0, totalBudget - totalSpent);
            CardRemaining.Value = CardRemaining.TargetValue;
            
            CardSavingRate.TargetValue = totalBudget > 0 ? (totalBudget - totalSpent) / totalBudget * 100 : 0;
            CardSavingRate.Value = CardSavingRate.TargetValue;

            var overBudgetItems = AllBudgets.Where(b => b.SpentAmount > b.TotalAmount).ToList();
            HasAlert = overBudgetItems.Any();
            if (HasAlert)
            {
                AlertMessage = $"Cảnh báo: {overBudgetItems.Count} danh mục đã vượt ngân sách ({(string.Join(", ", overBudgetItems.Select(x => x.CategoryName)))})";
            }
        }

        private void SetupCharts()
        {
            var top5 = AllBudgets.OrderByDescending(b => b.TotalAmount).Take(5).ToList();
            
            BarChartXAxes = new Axis[] { new Axis { Labels = top5.Select(b => b.CategoryName).ToArray(), LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")) } };
            BarChartYAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), Labeler = v => (v / 1_000_000).ToString("0.#") + "M" } };
            
            BarChartSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = "Ngân sách",
                    Values = top5.Select(b => (double)b.TotalAmount).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#7c6df8")),
                    MaxBarWidth = 15,
                    Rx = 4, Ry = 4
                },
                new ColumnSeries<double>
                {
                    Name = "Thực chi",
                    Values = top5.Select(b => (double)b.SpentAmount).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#10d9a0")),
                    MaxBarWidth = 15,
                    Rx = 4, Ry = 4
                }
            };

            double spent = (double)CardSpent.TargetValue;
            double total = (double)CardTotalBudget.TargetValue;
            double remain = Math.Max(0, total - spent);
            
            if (total == 0)
            {
                DonutSeries = new ISeries[]
                {
                    new PieSeries<double> { Values = new double[] { 1 }, Name = "Chưa có dữ liệu", Fill = new SolidColorPaint(SKColor.Parse("#e5e7eb")), InnerRadius = 60 }
                };
            }
            else
            {
                DonutSeries = new ISeries[]
                {
                    new PieSeries<double> { Values = new double[] { spent }, Name = "Đã chi", Fill = new SolidColorPaint(SKColor.Parse(spent > total ? "#f43f5e" : "#10d9a0")), InnerRadius = 60 },
                    new PieSeries<double> { Values = new double[] { remain }, Name = "Còn lại", Fill = new SolidColorPaint(SKColor.Parse("#e5e7eb")), InnerRadius = 60 }
                };
            }

            AreaChartXAxes = new Axis[] { new Axis { Labels = _trendLabels, LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")) } };
            AreaChartYAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), Labeler = v => (v / 1_000_000).ToString("0.#") + "M" } };
            
            AreaChartSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "Ngân sách",
                    Values = _trendBudgets,
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColor.Parse("#7c6df8")) { StrokeThickness = 2, PathEffect = new LiveChartsCore.SkiaSharpView.Painting.Effects.DashEffect(new float[] { 10, 10 }) }
                },
                new LineSeries<double>
                {
                    Name = "Thực chi",
                    Values = _trendSpents,
                    Fill = new SolidColorPaint(SKColor.Parse("#2010d9a0")),
                    Stroke = new SolidColorPaint(SKColor.Parse("#10d9a0")) { StrokeThickness = 3 },
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#ffffff")) { StrokeThickness = 2 },
                    GeometryFill = new SolidColorPaint(SKColor.Parse("#10d9a0"))
                }
            };

            OnPropertyChanged(nameof(BarChartSeries));
            OnPropertyChanged(nameof(DonutSeries));
            OnPropertyChanged(nameof(AreaChartSeries));
        }

        private async void EditBudget_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is BudgetData budgetData)
            {
                var window = new AddBudgetWindow(budgetData);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true)
                {
                    await LoadBudgetDataAsync();
                }
            }
        }

        private async void DeleteBudget_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is BudgetData budgetData)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa ngân sách cho danh mục '{budgetData.CategoryName}'?", 
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _budgetService.DeleteBudgetAsync(budgetData.BudgetId);
                        await LoadBudgetDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa ngân sách: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
