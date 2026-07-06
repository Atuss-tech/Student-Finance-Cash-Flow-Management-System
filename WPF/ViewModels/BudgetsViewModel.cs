using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace WPF.ViewModels
{
    // ─────────────────────────────────────────────
    // Mini stat card for Budget screen
    // ─────────────────────────────────────────────
    public class BudgetStatCardViewModel : ViewModelBase
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
                    ? (Value / 1_000_000).ToString("0.#") + "M₫"
                    : (Value / 1_000).ToString("0") + "K₫";
            }
        }

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
    }

    public class BudgetsViewModel : ViewModelBase
    {
        // ── Stat cards ───────────────────────────
        public BudgetStatCardViewModel CardTotalBudget { get; } = new() { Label = "Tổng ngân sách", Icon = "💰", AccentColor = "#3b82f6" };
        public BudgetStatCardViewModel CardSpent       { get; } = new() { Label = "Đã chi tiêu",    Icon = "🔥", AccentColor = "#f43f5e" };
        public BudgetStatCardViewModel CardRemaining   { get; } = new() { Label = "Còn khả dụng",   Icon = "✨", AccentColor = "#10d9a0" };
        public BudgetStatCardViewModel CardSavingRate  { get; } = new() { Label = "Tỷ lệ tiết kiệm",Icon = "📈", AccentColor = "#7c6df8", IsPercentage = true };

        // ── Budget items (10 categories) ─────────
        public ObservableCollection<BudgetViewModel> AllBudgets { get; set; }

        // ── Charts ───────────────────────────────
        public ISeries[] BarChartSeries { get; set; }
        public Axis[] BarChartXAxes { get; set; }
        public Axis[] BarChartYAxes { get; set; }

        public ISeries[] DonutSeries { get; set; }
        
        public ISeries[] AreaChartSeries { get; set; }
        public Axis[] AreaChartXAxes { get; set; }
        public Axis[] AreaChartYAxes { get; set; }

        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#c0cfe8")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        // ── Alert Banner ─────────────────────────
        public bool HasAlert { get; set; }
        public string AlertMessage { get; set; } = string.Empty;

        // ── Edit Modal ───────────────────────────
        private bool _isEditModalOpen;
        public bool IsEditModalOpen
        {
            get => _isEditModalOpen;
            set { _isEditModalOpen = value; OnPropertyChanged(); }
        }

        private BudgetViewModel? _editingBudget;
        public BudgetViewModel? EditingBudget
        {
            get => _editingBudget;
            set { _editingBudget = value; OnPropertyChanged(); IsEditModalOpen = value != null; }
        }

        public ICommand OpenEditCommand { get; }
        public ICommand CloseEditCommand { get; }
        public ICommand SaveEditCommand { get; }

        public BudgetsViewModel()
        {
            // Sample Data (10 items)
            AllBudgets = new ObservableCollection<BudgetViewModel>
            {
                new BudgetViewModel { CategoryName = "Ăn uống",    Icon = "🍜", SpentAmount = 4_800_000, TotalAmount = 5_000_000 },
                new BudgetViewModel { CategoryName = "Nhà ở & HĐ", Icon = "🏠", SpentAmount = 3_200_000, TotalAmount = 4_500_000 },
                new BudgetViewModel { CategoryName = "Di chuyển",  Icon = "🚗", SpentAmount = 980_000,   TotalAmount = 1_500_000 },
                new BudgetViewModel { CategoryName = "Giải trí",   Icon = "🎮", SpentAmount = 1_550_000, TotalAmount = 1_200_000 }, // Over budget
                new BudgetViewModel { CategoryName = "Mua sắm",    Icon = "🛍️", SpentAmount = 2_100_000, TotalAmount = 3_000_000 },
                new BudgetViewModel { CategoryName = "Giáo dục",   Icon = "📚", SpentAmount = 450_000,   TotalAmount = 1_000_000 },
                new BudgetViewModel { CategoryName = "Sức khỏe",   Icon = "❤️", SpentAmount = 200_000,   TotalAmount = 1_500_000 },
                new BudgetViewModel { CategoryName = "Quà tặng",   Icon = "🎁", SpentAmount = 500_000,   TotalAmount = 1_000_000 },
                new BudgetViewModel { CategoryName = "Đầu tư",     Icon = "📈", SpentAmount = 2_000_000, TotalAmount = 2_000_000 },
                new BudgetViewModel { CategoryName = "Khác",       Icon = "📦", SpentAmount = 150_000,   TotalAmount = 500_000 },
            };

            // Animate all bars
            foreach (var b in AllBudgets) b.AnimateProgress();

            // Commands
            OpenEditCommand = new RelayCommand(b => EditingBudget = b as BudgetViewModel);
            CloseEditCommand = new RelayCommand(_ => EditingBudget = null);
            SaveEditCommand = new RelayCommand(_ => {
                EditingBudget = null;
                // Re-evaluate stats and charts
                CalculateStats();
                SetupCharts();
            });

            CalculateStats();
            SetupCharts();

            // Animate stat cards
            var t2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            t2.Tick += (s, e) => { t2.Stop(); CardTotalBudget.Animate(); CardSpent.Animate(); CardRemaining.Animate(); CardSavingRate.Animate(); };
            t2.Start();
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
            // Bar Chart: Budget vs Actual for top 5 categories
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
                    Fill = new SolidColorPaint(SKColor.Parse("#10d9a0")), // Using emerald, ideally dynamically red if over but livecharts series level color is easier here
                    MaxBarWidth = 15,
                    Rx = 4, Ry = 4
                }
            };

            // Donut Chart: Overall
            double spentPct = (double)(CardSpent.TargetValue / (CardTotalBudget.TargetValue == 0 ? 1 : CardTotalBudget.TargetValue));
            double remainPct = Math.Max(0, 1.0 - spentPct);
            
            DonutSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { spentPct }, Name = "Đã chi", Fill = new SolidColorPaint(SKColor.Parse(spentPct > 1 ? "#f43f5e" : "#10d9a0")), InnerRadius = 60 },
                new PieSeries<double> { Values = new double[] { remainPct }, Name = "Còn lại", Fill = new SolidColorPaint(SKColor.Parse("#1a2035")), InnerRadius = 60 }
            };

            // Area Chart: 6 month trend
            AreaChartXAxes = new Axis[] { new Axis { Labels = new[] { "Th 2", "Th 3", "Th 4", "Th 5", "Th 6", "Th 7" }, LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")) } };
            AreaChartYAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), Labeler = v => (v / 1_000_000).ToString("0.#") + "M" } };
            
            AreaChartSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "Ngân sách đặt ra",
                    Values = new double[] { 18_000_000, 18_000_000, 19_000_000, 20_000_000, 20_000_000, 21_200_000 },
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColor.Parse("#7c6df8")) { StrokeThickness = 2, PathEffect = new LiveChartsCore.SkiaSharpView.Painting.Effects.DashEffect(new float[] { 10, 10 }) }
                },
                new LineSeries<double>
                {
                    Name = "Thực chi",
                    Values = new double[] { 15_500_000, 16_200_000, 17_800_000, 22_100_000, 18_900_000, (double)CardSpent.TargetValue },
                    Fill = new SolidColorPaint(SKColor.Parse("#2010d9a0")),
                    Stroke = new SolidColorPaint(SKColor.Parse("#10d9a0")) { StrokeThickness = 3 },
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#080a0f")) { StrokeThickness = 2 },
                    GeometryFill = new SolidColorPaint(SKColor.Parse("#10d9a0"))
                }
            };

            OnPropertyChanged(nameof(BarChartSeries));
            OnPropertyChanged(nameof(DonutSeries));
            OnPropertyChanged(nameof(AreaChartSeries));
        }
    }
}
