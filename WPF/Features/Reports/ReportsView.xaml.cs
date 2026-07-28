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
using WPF.UIData;
using Services;
using Repositories;
using System.Collections.Generic;
using System.Windows.Media;

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
                string fmt = abs >= 1_000_000 ? (abs / 1_000_000).ToString("0.#") + "Mđ" : (abs / 1_000).ToString("0") + "Kđ";
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

    public class SpendingGroupLegendItem
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        private string _colorHex = "#6b7280";
        public string ColorHex
        {
            get => _colorHex;
            set
            {
                _colorHex = value;
                try { ColorBrush = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(value)); }
                catch { ColorBrush = System.Windows.Media.Brushes.Gray; }
            }
        }

        private string _lightBackground = "#f3f4f6";
        public string LightBackground
        {
            get => _lightBackground;
            set
            {
                _lightBackground = value;
                try { LightBrush = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(value)); }
                catch { LightBrush = System.Windows.Media.Brushes.WhiteSmoke; }
            }
        }

        public System.Windows.Media.SolidColorBrush ColorBrush { get; private set; } = System.Windows.Media.Brushes.Gray;
        public System.Windows.Media.SolidColorBrush LightBrush { get; private set; } = System.Windows.Media.Brushes.WhiteSmoke;

        public decimal Amount { get; set; }
        public string FormattedAmount =>
            Amount >= 1_000_000
                ? (Amount / 1_000_000).ToString("0.#") + "Mđ"
                : (Amount / 1_000).ToString("0") + "Kđ";
        public string Percentage { get; set; } = "0%";
        /// <summary>Width for mini progress bar (max 60px)</summary>
        public double BarWidth { get; set; }
    }

    public enum ReportPeriod
    {
        Week,
        Month,
        Year
    }

    public partial class ReportsView : UserControl, INotifyPropertyChanged
    {
        private ReportPeriod _currentPeriod = ReportPeriod.Month;
        public ReportPeriod CurrentPeriod
        {
            get => _currentPeriod;
            set { _currentPeriod = value; OnPropertyChanged(); UpdateDateRange(); }
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private void UpdateDateRange()
        {
            var today = DateTime.Today;
            switch (CurrentPeriod)
            {
                case ReportPeriod.Week:
                    int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                    StartDate = today.AddDays(-1 * diff).Date;
                    EndDate = StartDate.AddDays(7).AddTicks(-1);
                    break;
                case ReportPeriod.Month:
                    StartDate = new DateTime(today.Year, today.Month, 1);
                    EndDate = StartDate.AddMonths(1).AddTicks(-1);
                    break;
                case ReportPeriod.Year:
                    StartDate = new DateTime(today.Year, 1, 1);
                    EndDate = StartDate.AddYears(1).AddTicks(-1);
                    break;
            }
        }

        public ReportStatCardModel TotalIncomeReport { get; } = new() { Title = "TỔNG THU", BorderAccentBrush = "#10d9a0", Icon = "💵", Subtext = "Thời gian đã chọn" };
        public ReportStatCardModel TotalExpenseReport { get; } = new() { Title = "TỔNG CHI", BorderAccentBrush = "#f43f5e", Icon = "🔥", Subtext = "Thời gian đã chọn" };
        public ReportStatCardModel NetCashFlowReport { get; } = new() { Title = "DÒNG TIỀN", BorderAccentBrush = "#7c6df8", Icon = "📈", Subtext = "Thu nhập - Chi tiêu" };

        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#6b7280")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        private ISeries[] _expenseAllocationSeries = Array.Empty<ISeries>();
        public ISeries[] ExpenseAllocationSeries { get => _expenseAllocationSeries; set { _expenseAllocationSeries = value; OnPropertyChanged(); } }

        private ISeries[] _spendingGroupSeries = Array.Empty<ISeries>();
        public ISeries[] SpendingGroupSeries { get => _spendingGroupSeries; set { _spendingGroupSeries = value; OnPropertyChanged(); } }

        private ObservableCollection<SpendingGroupLegendItem> _spendingGroupLegend = new();
        public ObservableCollection<SpendingGroupLegendItem> SpendingGroupLegend
        {
            get => _spendingGroupLegend;
            set { _spendingGroupLegend = value; OnPropertyChanged(); }
        }

        private ISeries[] _cashFlowTrendSeries = Array.Empty<ISeries>();
        public ISeries[] CashFlowTrendSeries { get => _cashFlowTrendSeries; set { _cashFlowTrendSeries = value; OnPropertyChanged(); } }
        
        public Axis[] CashFlowXAxes { get; set; }
        public Axis[] CashFlowYAxes { get; set; }

        private ObservableCollection<TransactionData> _detailTransactions = new();
        public ObservableCollection<TransactionData> DetailTransactions
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

        private readonly ITransactionService _transactionService;

        public ReportsView()
        {
            InitializeComponent();
            this.DataContext = this;

            _transactionService = new TransactionService(new TransactionRepository());

            DetailTransactions = new ObservableCollection<TransactionData>();

            CashFlowXAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")) } };
            CashFlowYAxes = new Axis[] { new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), Labeler = v => (v / 1_000_000).ToString("0.#") + "M" } };

            UpdateDateRange();

            this.Loaded += ReportsView_Loaded;

            var t2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            t2.Tick += (s, e) => { t2.Stop(); TotalIncomeReport.Animate(); TotalExpenseReport.Animate(); NetCashFlowReport.Animate(); };
            t2.Start();
        }

        private async void ReportsView_Loaded(object sender, RoutedEventArgs e)
        {
            await GenerateReportsAsync();
        }

        private void Period_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            
            if (sender is RadioButton rb)
            {
                var content = rb.Content?.ToString();
                switch (content)
                {
                    case "Tuần này": CurrentPeriod = ReportPeriod.Week; break;
                    case "Tháng này": CurrentPeriod = ReportPeriod.Month; break;
                    case "Năm nay": CurrentPeriod = ReportPeriod.Year; break;
                }
                _ = GenerateReportsAsync();
            }
        }

        public async System.Threading.Tasks.Task GenerateReportsAsync()
        {
            int userId = 1;

            try
            {
                var allTransactions = await System.Threading.Tasks.Task.Run(() => _transactionService.GetTransactionsByUserId(userId));
                var start = DateOnly.FromDateTime(StartDate);
                var end = DateOnly.FromDateTime(EndDate);
                var transactions = allTransactions.Where(t => t.TransactionDate >= start && t.TransactionDate <= end).ToList();

                // 1. Lấy thông số chung
                decimal totalIncome = transactions.Where(t => t.TransactionType == "Income").Sum(t => t.Amount);
                decimal totalExpense = transactions.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount);
                decimal balance = totalIncome - totalExpense;

                TotalIncomeReport.TargetAmount = totalIncome;
                TotalIncomeReport.Value = totalIncome;
                TotalExpenseReport.TargetAmount = totalExpense;
                TotalExpenseReport.Value = totalExpense;
                NetCashFlowReport.TargetAmount = balance;
                NetCashFlowReport.Value = balance;

                // 2. Lấy chi phí theo danh mục (Pie Chart)
                var expensesByCategory = transactions
                    .Where(t => t.TransactionType == "Expense")
                    .GroupBy(t => t.Category?.CategoryName ?? "Unknown")
                    .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

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
                                await ShowTransactionsForCategoryAsync(clickedCategory, transactions);
                            });
                        }
                    };

                    pieSeriesList.Add(series);
                    colorIndex++;
                }
                ExpenseAllocationSeries = pieSeriesList.ToArray();

                // 3. Lấy xu hướng dòng tiền (Bar Chart)
                Dictionary<string, decimal> trend = new Dictionary<string, decimal>();
                if (CurrentPeriod == ReportPeriod.Year)
                {
                    trend = transactions.GroupBy(t => t.TransactionDate.Month)
                                        .ToDictionary(g => $"Tháng {g.Key}", 
                                                      g => g.Where(t => t.TransactionType == "Income").Sum(t => t.Amount) - g.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount));
                }
                else
                {
                    trend = transactions.GroupBy(t => t.TransactionDate)
                                        .ToDictionary(g => g.Key.ToString("dd/MM"), 
                                                      g => g.Where(t => t.TransactionType == "Income").Sum(t => t.Amount) - g.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount));
                }

                var labels = new List<string>();
                var netVals = new List<double>();
                
                // Sort trend chronologically
                IEnumerable<KeyValuePair<string, decimal>> orderedTrend;
                if (CurrentPeriod == ReportPeriod.Year)
                {
                    orderedTrend = trend.OrderBy(x => int.Parse(x.Key.Replace("Tháng ", "")));
                }
                else
                {
                    orderedTrend = trend.OrderBy(x => DateTime.ParseExact(x.Key, "dd/MM", null));
                }

                foreach (var kvp in orderedTrend)
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

                // 4. Cơ cấu chi tiêu theo 4 nhóm
                var essentialKeywords = new[] { "ăn", "uống", "nhà", "ở", "thuê", "điện", "nước", "y tế", "thuốc", "khám", "bệnh", "đi lại", "xăng", "sửa xe", "xe", "internet", "điện thoại", "sinh hoạt", "chợ", "tạp hóa", "thực phẩm" };
                var personalKeywords = new[] { "giải trí", "cafe", "cà phê", "mua sắm", "du lịch", "thể thao", "mỹ phẩm", "làm đẹp", "quà tặng", "quà", "thời trang", "quần áo", "phim", "game", "nhà hàng", "karaoke", "vui chơi", "đồ dùng cá nhân" };
                var savingsKeywords = new[] { "tiết kiệm", "đầu tư", "bảo hiểm", "tích lũy", "quỹ", "dự phòng" };
                var futureKeywords = new[] { "học", "khóa học", "sách", "phát triển", "ngoại ngữ", "kỹ năng", "đào tạo", "chứng chỉ", "giáo dục", "học phí", "trường", "online" };
                
                var spendingGroups = new Dictionary<string, decimal> { ["🏠 Nhu cầu thiết yếu"] = 0, ["🎮 Sở thích cá nhân"] = 0, ["💰 Tích lũy"] = 0, ["🎓 Tương lai"] = 0 };

                var expenses = transactions.Where(t => t.TransactionType == "Expense").ToList();
                foreach (var t in expenses)
                {
                    var catName = (t.Category?.CategoryName ?? "").ToLowerInvariant();
                    var desc = (t.Description ?? "").ToLowerInvariant();
                    var combined = catName + " " + desc;

                    if (futureKeywords.Any(k => combined.Contains(k))) spendingGroups["🎓 Tương lai"] += t.Amount;
                    else if (savingsKeywords.Any(k => combined.Contains(k))) spendingGroups["💰 Tích lũy"] += t.Amount;
                    else if (personalKeywords.Any(k => combined.Contains(k))) spendingGroups["🎮 Sở thích cá nhân"] += t.Amount;
                    else spendingGroups["🏠 Nhu cầu thiết yếu"] += t.Amount;
                }

                var groupMeta = new Dictionary<string, (string Color, string Icon, string Light, string DisplayName)>
                {
                    ["🏠 Nhu cầu thiết yếu"] = ("#f43f5e", "🏠", "#fff1f2", "Nhu cầu thiết yếu"),
                    ["🎮 Sở thích cá nhân"]   = ("#f59e0b", "🎮", "#fffbeb", "Sở thích cá nhân"),
                    ["💰 Tích lũy"]             = ("#10d9a0", "💰", "#ecfdf5", "Tích lũy"),
                    ["🎓 Tương lai"]             = ("#7c6df8", "🎓", "#f5f3ff", "Tương lai"),
                };

                var totalGroupExpense = spendingGroups.Values.Sum();
                var groupPieSeries = new List<PieSeries<double>>();
                var legendItems = new ObservableCollection<SpendingGroupLegendItem>();

                var orderedKeys = new[] { "🏠 Nhu cầu thiết yếu", "🎮 Sở thích cá nhân", "💰 Tích lũy", "🎓 Tương lai" };

                foreach (var key in orderedKeys)
                {
                    decimal amount = spendingGroups.ContainsKey(key) ? spendingGroups[key] : 0;
                    if (amount <= 0) continue; // Skip empty groups
                    
                    var (color, icon, light, displayName) = groupMeta[key];
                    double pct = totalGroupExpense > 0 ? (double)(amount / totalGroupExpense) * 100 : 0;

                    groupPieSeries.Add(new PieSeries<double>
                    {
                        Values = new double[] { (double)amount > 0 ? (double)amount : 0.001 },
                        Name = displayName,
                        InnerRadius = 60,
                        Fill = new SolidColorPaint(SKColor.Parse(color))
                    });

                    legendItems.Add(new SpendingGroupLegendItem
                    {
                        Name = displayName,
                        Icon = icon,
                        ColorHex = color,
                        LightBackground = light,
                        Amount = amount,
                        Percentage = $"{pct:0.#}% tổng chi",
                        BarWidth = pct * 0.6  // max 60px
                    });
                }

                SpendingGroupSeries = groupPieSeries.ToArray();
                SpendingGroupLegend = legendItems;

                await ShowTransactionsForCategoryAsync("Tất cả chi tiêu", transactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message);
            }
        }

        private async System.Threading.Tasks.Task ShowTransactionsForCategoryAsync(string? category, List<BusinessObjects.Models.FinanceTransaction> filteredTransactions)
        {
            SelectedCategoryLabel = $"Chi tiết: {category}";

            var details = category == "Tất cả chi tiêu" 
                ? filteredTransactions.Where(t => t.TransactionType == "Expense").OrderByDescending(t => t.TransactionDate)
                : filteredTransactions.Where(t => t.Category?.CategoryName == category).OrderByDescending(t => t.TransactionDate);
                
            var models = details.Select(t => new TransactionData
            {
                Title = t.Description ?? "Không có mô tả",
                Category = t.Category?.CategoryName ?? "Khác",
                Amount = t.Amount,
                Date = t.CreatedAt != default ? t.CreatedAt : t.TransactionDate.ToDateTime(TimeOnly.MinValue),
                IsExpense = t.TransactionType == "Expense",
                Icon = "💵",
                IconBackground = t.TransactionType == "Expense" ? "#45f43f5e" : "#4510d9a0"
            }).ToList();

            DetailTransactions = new ObservableCollection<TransactionData>(models);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
