using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using SkiaSharp;

namespace WPF.ViewModels
{
    public class ReportStatCardViewModel : ViewModelBase
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
    }

    public class ReportsViewModel : ViewModelBase
    {
        // ── Stat cards ───────────────────────────
        public ReportStatCardViewModel TotalIncomeReport { get; } = new() { Title = "TỔNG THU", BorderAccentBrush = "#10d9a0", Icon = "💵", Subtext = "Khoảng thời gian đã chọn" };
        public ReportStatCardViewModel TotalExpenseReport { get; } = new() { Title = "TỔNG CHI", BorderAccentBrush = "#f43f5e", Icon = "🔥", Subtext = "Khoảng thời gian đã chọn" };
        public ReportStatCardViewModel NetCashFlowReport { get; } = new() { Title = "DÒNG TIỀN", BorderAccentBrush = "#7c6df8", Icon = "📈", Subtext = "Thu nhập - Chi tiêu" };

        // ── Charts ───────────────────────────────
        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#c0cfe8")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        private ISeries[] _expenseAllocationSeries;
        public ISeries[] ExpenseAllocationSeries { get => _expenseAllocationSeries; set { _expenseAllocationSeries = value; OnPropertyChanged(); } }

        private ISeries[] _cashFlowTrendSeries;
        public ISeries[] CashFlowTrendSeries { get => _cashFlowTrendSeries; set { _cashFlowTrendSeries = value; OnPropertyChanged(); } }
        
        public Axis[] CashFlowXAxes { get; set; }
        public Axis[] CashFlowYAxes { get; set; }

        // ── Drill Down Data ──────────────────────
        private ObservableCollection<TransactionViewModel> _detailTransactions;
        public ObservableCollection<TransactionViewModel> DetailTransactions
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

        // ── Filters ──────────────────────────────
        private int _selectedRangeDays = 30;
        public ICommand SetRange30Command { get; }
        public ICommand SetRange90Command { get; }
        public ICommand SetRange180Command { get; }

        private readonly ObservableCollection<TransactionViewModel> _allTransactionsMock;

        public ReportsViewModel()
        {
            _allTransactionsMock = new ObservableCollection<TransactionViewModel>(GetMockData());
            DetailTransactions = new ObservableCollection<TransactionViewModel>();

            CashFlowXAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")) } };
            CashFlowYAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), Labeler = v => (v / 1_000_000).ToString("0.#") + "M" } };

            SetRange30Command = new RelayCommand(_ => { _selectedRangeDays = 30; GenerateReports(); });
            SetRange90Command = new RelayCommand(_ => { _selectedRangeDays = 90; GenerateReports(); });
            SetRange180Command = new RelayCommand(_ => { _selectedRangeDays = 180; GenerateReports(); });

            GenerateReports();

            // Animate stat cards
            var t2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            t2.Tick += (s, e) => { t2.Stop(); TotalIncomeReport.Animate(); TotalExpenseReport.Animate(); NetCashFlowReport.Animate(); };
            t2.Start();
        }

        private void GenerateReports()
        {
            var cutoffDate = DateTime.Now.Date.AddDays(-_selectedRangeDays);
            var filtered = _allTransactionsMock.Where(t => t.Date >= cutoffDate).ToList();

            // Stats
            var income = filtered.Where(t => !t.IsExpense).Sum(t => t.Amount);
            var expense = filtered.Where(t => t.IsExpense).Sum(t => t.Amount);
            var net = income - expense;

            TotalIncomeReport.TargetAmount = income;
            TotalIncomeReport.Value = income;
            TotalExpenseReport.TargetAmount = expense;
            TotalExpenseReport.Value = expense;
            NetCashFlowReport.TargetAmount = net;
            NetCashFlowReport.Value = net;

            // Expense Allocation Donut
            var expensesByCategory = filtered.Where(t => t.IsExpense)
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
                .OrderByDescending(x => x.Total)
                .ToList();

            var colors = new[] { "#f43f5e", "#f59e0b", "#7c6df8", "#3b82f6", "#10d9a0", "#8b5cf6" };
            
            var pieSeriesList = new System.Collections.Generic.List<PieSeries<double>>();
            for (int i = 0; i < expensesByCategory.Count; i++)
            {
                var cat = expensesByCategory[i];
                var series = new PieSeries<double>
                {
                    Values = new double[] { (double)cat.Total },
                    Name = cat.Category,
                    InnerRadius = 60,
                    Fill = new SolidColorPaint(SKColor.Parse(colors[i % colors.Length]))
                };
                
                // Add click handler for drill down
                series.ChartPointPointerDown += (IChartView chart, LiveChartsCore.Kernel.ChartPoint<double, LiveChartsCore.SkiaSharpView.Drawing.Geometries.DoughnutGeometry, LiveChartsCore.SkiaSharpView.Drawing.Geometries.LabelGeometry>? point) =>
                {
                    if (point != null)
                    {
                        var clickedCategory = point.Context.Series.Name;
                        System.Windows.Application.Current.Dispatcher.Invoke(() => {
                            ShowTransactionsForCategory(clickedCategory, filtered);
                        });
                    }
                };

                pieSeriesList.Add(series);
            }
            ExpenseAllocationSeries = pieSeriesList.ToArray();

            // Cash Flow Trend Bar Chart
            int numPeriods = _selectedRangeDays == 30 ? 4 : (_selectedRangeDays == 90 ? 3 : 6);
            var incomeVals = new double[numPeriods];
            var expenseVals = new double[numPeriods];
            var labels = new string[numPeriods];

            for (int i = 0; i < numPeriods; i++)
            {
                DateTime pStart, pEnd;
                if (_selectedRangeDays == 30) {
                    pEnd = DateTime.Now.AddDays(-i * 7);
                    pStart = pEnd.AddDays(-7);
                    labels[numPeriods - 1 - i] = $"Tuần {4 - i}";
                } else {
                    pEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i).AddMonths(1).AddDays(-1);
                    pStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i);
                    labels[numPeriods - 1 - i] = $"Th {pStart.Month}";
                }
                
                var pTrans = filtered.Where(t => t.Date >= pStart && t.Date <= pEnd).ToList();
                incomeVals[numPeriods - 1 - i] = (double)pTrans.Where(t => !t.IsExpense).Sum(t => t.Amount);
                expenseVals[numPeriods - 1 - i] = (double)pTrans.Where(t => t.IsExpense).Sum(t => t.Amount);
            }

            CashFlowXAxes[0].Labels = labels;
            CashFlowTrendSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = incomeVals,
                    Name = "Tổng Thu",
                    Fill = new SolidColorPaint(SKColor.Parse("#10d9a0")),
                    MaxBarWidth = 20,
                    Rx = 6, Ry = 6
                },
                new ColumnSeries<double>
                {
                    Values = expenseVals,
                    Name = "Tổng Chi",
                    Fill = new SolidColorPaint(SKColor.Parse("#f43f5e")),
                    MaxBarWidth = 20,
                    Rx = 6, Ry = 6
                }
            };

            // Default show all expenses
            ShowTransactionsForCategory("Tất cả chi tiêu", filtered);
        }

        private void ShowTransactionsForCategory(string category, System.Collections.Generic.List<TransactionViewModel> filtered)
        {
            SelectedCategoryLabel = $"Chi tiết: {category}";
            var details = category == "Tất cả chi tiêu" 
                ? filtered.Where(t => t.IsExpense).OrderByDescending(t => t.Date)
                : filtered.Where(t => t.Category == category).OrderByDescending(t => t.Date);
                
            DetailTransactions = new ObservableCollection<TransactionViewModel>(details);
        }

        private static System.Collections.Generic.List<TransactionViewModel> GetMockData()
        {
            var now = DateTime.Now;
            var list = new System.Collections.Generic.List<TransactionViewModel>();
            var random = new Random(42);

            string[] categories = { "Ăn uống", "Di chuyển", "Mua sắm", "Giải trí", "Sinh hoạt", "Giáo dục" };
            string[] icons = { "🍜", "🚗", "🛍️", "🎮", "🏠", "📚" };
            string[] colors = { "#45f59e0b", "#453b82f6", "#45f43f5e", "#457c6df8", "#4510d9a0", "#45f43f5e" };

            // Generate 180 days of fake data
            for (int i = 0; i < 180; i++)
            {
                var date = now.AddDays(-i);
                
                // Income
                if (date.Day == 1 || date.Day == 15) {
                    list.Add(new TransactionViewModel { Title = "Nhận lương", Category = "Thu nhập", Amount = 15_000_000m + (decimal)(random.NextDouble() * 5000000), Date = date, IsExpense = false, Icon = "💵", IconBackground = "#4510d9a0" });
                }

                // Daily expenses
                int expenseCount = random.Next(1, 4);
                for (int j = 0; j < expenseCount; j++)
                {
                    int cIdx = random.Next(categories.Length);
                    decimal amt = (decimal)(random.Next(50, 500) * 1000);
                    if (categories[cIdx] == "Mua sắm" || categories[cIdx] == "Giáo dục") amt *= 3;

                    list.Add(new TransactionViewModel
                    {
                        Title = $"{categories[cIdx]} {j+1}",
                        Category = categories[cIdx],
                        Amount = amt,
                        Date = date.AddHours(-random.Next(1, 23)),
                        IsExpense = true,
                        Icon = icons[cIdx],
                        IconBackground = colors[cIdx]
                    });
                }
            }
            return list;
        }
    }
}
