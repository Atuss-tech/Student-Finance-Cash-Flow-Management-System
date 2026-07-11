using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Kernel;
using SkiaSharp;
using WPF.Models;
using Services;
using Repositories;
using System.Collections.Generic;

namespace WPF.Features.Reports
{
    public class ReportStatCardModel : INotifyPropertyChanged
    {
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string BorderAccentBrush { get; set; } = "#10d9a0";
        public string Subtext { get; set; } = string.Empty;

        private decimal _targetAmount;
        public decimal TargetAmount { get => _targetAmount; set { _targetAmount = value; } }

        private decimal _value;
        public decimal Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); OnPropertyChanged(nameof(FormattedValue)); }
        }

        public string FormattedValue
        {
            get
            {
                var abs = Math.Abs(Value);
                string fmt = abs >= 1_000_000 ? (abs / 1_000_000).ToString("0.#") + "M₫" : (abs / 1_000).ToString("0") + "K₫";
                return Value < 0 ? "-" + fmt : fmt;
            }
        }

        public void Animate()
        {
            int steps = 35;
            int tick = 0;
            decimal target = TargetAmount;
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

    public partial class ReportsView : UserControl, INotifyPropertyChanged
    {
        public ReportStatCardModel TotalIncomeReport { get; } = new() { Title = "TỔNG THU", BorderAccentBrush = "#10d9a0", Icon = "💵", Subtext = "Tháng hiện tại" };
        public ReportStatCardModel TotalExpenseReport { get; } = new() { Title = "TỔNG CHI", BorderAccentBrush = "#f43f5e", Icon = "🔥", Subtext = "Tháng hiện tại" };
        public ReportStatCardModel NetCashFlowReport { get; } = new() { Title = "DÒNG TIỀN", BorderAccentBrush = "#7c6df8", Icon = "📈", Subtext = "Thu nhập - Chi tiêu" };

        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#6b7280")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        private ISeries[] _expenseAllocationSeries = Array.Empty<ISeries>();
        public ISeries[] ExpenseAllocationSeries { get => _expenseAllocationSeries; set { _expenseAllocationSeries = value; OnPropertyChanged(); } }

        private ISeries[] _cashFlowTrendSeries = Array.Empty<ISeries>();
        public ISeries[] CashFlowTrendSeries { get => _cashFlowTrendSeries; set { _cashFlowTrendSeries = value; OnPropertyChanged(); } }
        
        public Axis[] CashFlowXAxes { get; set; }
        public Axis[] CashFlowYAxes { get; set; }

        private ObservableCollection<TransactionModel> _detailTransactions = new();
        public ObservableCollection<TransactionModel> DetailTransactions
        {
            get => _detailTransactions;
            set { _detailTransactions = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasData)); }
        }

        private string _selectedCategoryLabel = "Tất cả giao dịch";
        public string SelectedCategoryLabel
        {
            get => _selectedCategoryLabel;
            set { _selectedCategoryLabel = value; OnPropertyChanged(); }
        }

        public bool HasData => DetailTransactions != null && DetailTransactions.Count > 0;

        private readonly IReportService _reportService;
        private readonly ITransactionService _transactionService;

        public ReportsView()
        {
            InitializeComponent();
            this.DataContext = this;

            _reportService = new ReportService(new TransactionRepository());
            _transactionService = new TransactionService(new TransactionRepository());

            DetailTransactions = new ObservableCollection<TransactionModel>();

            CashFlowXAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")) } };
            CashFlowYAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), Labeler = v => (v / 1_000_000).ToString("0.#") + "M" } };

            this.Loaded += ReportsView_Loaded;

            var t2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            t2.Tick += (s, e) => { t2.Stop(); TotalIncomeReport.Animate(); TotalExpenseReport.Animate(); NetCashFlowReport.Animate(); };
            t2.Start();
        }

        private async void ReportsView_Loaded(object sender, RoutedEventArgs e)
        {
            await GenerateReportsAsync();
        }

        private void SetRange30_Click(object sender, RoutedEventArgs e) { /* Bỏ qua, sử dụng theo tháng */ }
        private void SetRange90_Click(object sender, RoutedEventArgs e) { /* Bỏ qua */ }
        private void SetRange180_Click(object sender, RoutedEventArgs e) { /* Bỏ qua */ }

        private async System.Threading.Tasks.Task GenerateReportsAsync()
        {
            int userId = 1;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            try
            {
                // 1. Lấy thông số chung
                var report = await _reportService.GetMonthlyReportAsync(userId, month, year);

                TotalIncomeReport.TargetAmount = report.TotalIncome;
                TotalIncomeReport.Value = report.TotalIncome;
                TotalExpenseReport.TargetAmount = report.TotalExpense;
                TotalExpenseReport.Value = report.TotalExpense;
                NetCashFlowReport.TargetAmount = report.Balance;
                NetCashFlowReport.Value = report.Balance;

                // 2. Lấy chi phí theo danh mục (Pie Chart)
                var expensesByCategory = await _reportService.GetExpenseByCategoryAsync(userId, month, year);

                var colors = new[] { "#f43f5e", "#f59e0b", "#7c6df8", "#3b82f6", "#10d9a0", "#8b5cf6" };
                var pieSeriesList = new List<PieSeries<double>>();
                
                int colorIndex = 0;
                foreach (var kvp in expensesByCategory.OrderByDescending(x => x.Value))
                {
                    var series = new PieSeries<double>
                    {
                        Values = new double[] { (double)kvp.Value },
                        Name = kvp.Key,
                        InnerRadius = 60,
                        Fill = new SolidColorPaint(SKColor.Parse(colors[colorIndex % colors.Length]))
                    };
                    
                    series.ChartPointPointerDown += async (chart, point) =>
                    {
                        if (point != null)
                        {
                            var clickedCategory = point.Context.Series.Name;
                            await System.Windows.Application.Current.Dispatcher.InvokeAsync(async () => {
                                await ShowTransactionsForCategoryAsync(clickedCategory, userId, month, year);
                            });
                        }
                    };

                    pieSeriesList.Add(series);
                    colorIndex++;
                }
                ExpenseAllocationSeries = pieSeriesList.ToArray();

                // 3. Lấy xu hướng dòng tiền (Bar Chart)
                var trend = await _reportService.GetCashFlowTrendAsync(userId, year);
                var labels = new List<string>();
                var netVals = new List<double>();

                foreach (var kvp in trend.OrderBy(x => x.Key)) // sort by "Tháng X"
                {
                    labels.Add(kvp.Key);
                    netVals.Add((double)kvp.Value);
                }

                CashFlowXAxes[0].Labels = labels.ToArray();
                CashFlowTrendSeries = new ISeries[]
                {
                    new ColumnSeries<double>
                    {
                        Values = netVals.ToArray(),
                        Name = "Dòng tiền",
                        Fill = new SolidColorPaint(SKColor.Parse("#10d9a0")),
                        MaxBarWidth = 20,
                        Rx = 6, Ry = 6
                    }
                };

                await ShowTransactionsForCategoryAsync("Tất cả chi tiêu", userId, month, year);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message);
            }
        }

        private async System.Threading.Tasks.Task ShowTransactionsForCategoryAsync(string? category, int userId, int month, int year)
        {
            SelectedCategoryLabel = $"Chi tiết: {category}";
            var allTransactions = await _transactionService.GetTransactionsByMonthAsync(userId, month, year);

            var details = category == "Tất cả chi tiêu" 
                ? allTransactions.Where(t => t.TransactionType == "Expense").OrderByDescending(t => t.TransactionDate)
                : allTransactions.Where(t => t.Category?.CategoryName == category).OrderByDescending(t => t.TransactionDate);
                
            var models = details.Select(t => new TransactionModel
            {
                Title = t.Description ?? "Không có mô tả",
                Category = t.Category?.CategoryName ?? "Khác",
                Amount = t.Amount,
                Date = t.TransactionDate.ToDateTime(TimeOnly.MinValue),
                IsExpense = t.TransactionType == "Expense",
                Icon = "💵",
                IconBackground = t.TransactionType == "Expense" ? "#45f43f5e" : "#4510d9a0"
            }).ToList();

            DetailTransactions = new ObservableCollection<TransactionModel>(models);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
