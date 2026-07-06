using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace WPF.ViewModels
{
    public class DashboardSummaryViewModel : ViewModelBase
    {
        public string Title { get; set; } = string.Empty;
        public string Subtext { get; set; } = string.Empty;
        public double ChangePercentage { get; set; }
        public string BorderAccentBrush { get; set; } = "#1E2330";
        public string Icon { get; set; } = string.Empty;

        public Brush IconBackgroundBrush
        {
            get
            {
                try
                {
                    var c = (System.Windows.Media.Color)ColorConverter.ConvertFromString(BorderAccentBrush);
                    return new SolidColorBrush(System.Windows.Media.Color.FromArgb(90, c.R, c.G, c.B));
                }
                catch { return Brushes.Transparent; }
            }
        }

        // Raw target amount for animation
        public decimal TargetAmount { get; set; }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); OnPropertyChanged(nameof(FormattedAmount)); }
        }

        public string FormattedAmount => Amount >= 1000000
            ? (Amount / 1000000).ToString("0.#") + "M₫"
            : (Amount >= 1000 ? (Amount / 1000).ToString("0.#") + "K₫" : Amount.ToString("#,##0 ₫"));

        public string FormattedChange => (ChangePercentage > 0 ? "▲ " : "▼ ") + Math.Abs(ChangePercentage).ToString("0.#") + "%";
        public string ChangeColorBrush => ChangePercentage > 0 ? "#10d9a0" : "#f43f5e";

        public void AnimateTo(decimal target, int steps = 30)
        {
            decimal start = 0;
            decimal delta = target / steps;
            int tick = 0;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(35) };
            timer.Tick += (s, e) =>
            {
                tick++;
                // easing: ease-out cubic
                double t = (double)tick / steps;
                double easedT = 1 - Math.Pow(1 - t, 3);
                Amount = (decimal)(easedT * (double)target);
                if (tick >= steps) { Amount = target; timer.Stop(); }
            };
            timer.Start();
        }
    }

    public class TransactionViewModel
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string IconBackground { get; set; } = "#1a2035";
        public bool IsExpense { get; set; }

        public string FormattedAmount
        {
            get
            {
                string numStr = Amount >= 1000000
                    ? (Amount / 1000000).ToString("0.##") + "M₫"
                    : (Amount >= 1000 ? (Amount / 1000).ToString("0.#") + "K₫" : Amount.ToString("#,##0 ₫"));
                return (IsExpense ? "-" : "+") + numStr;
            }
        }
        public string AmountColorBrush => IsExpense ? "#f43f5e" : "#10d9a0";
        public string FormattedDate => Date.ToString("dd/MM");
    }

    public class BudgetViewModel : ViewModelBase
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        private decimal _spentAmount;
        public decimal SpentAmount
        {
            get => _spentAmount;
            set { _spentAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(ProgressPercentage)); OnPropertyChanged(nameof(FormattedSpent)); OnPropertyChanged(nameof(FormattedTotal)); OnPropertyChanged(nameof(ProgressBrush)); OnPropertyChanged(nameof(StatusText)); OnPropertyChanged(nameof(IsWarning)); OnPropertyChanged(nameof(IsExceeded)); }
        }

        private double _animatedProgress;
        public double AnimatedProgress
        {
            get => _animatedProgress;
            set { _animatedProgress = value; OnPropertyChanged(); }
        }

        public double ProgressPercentage => TotalAmount == 0 ? 0 : Math.Min(100, (double)(SpentAmount / TotalAmount * 100));
        public bool IsWarning => ProgressPercentage >= 80 && ProgressPercentage < 100;
        public bool IsExceeded => ProgressPercentage >= 100;

        public string ProgressBrush => IsExceeded ? "#f43f5e" : (IsWarning ? "#f59e0b" : "#10d9a0");
        public string StatusText => IsExceeded ? "🔴 Vượt mức" : (IsWarning ? "🟡 Sắp vượt" : "🟢 An toàn");
        public string FormattedSpent => SpentAmount >= 1000000 ? (SpentAmount / 1000000).ToString("0.#") + "M" : (SpentAmount / 1000).ToString("0") + "K";
        public string FormattedTotal => TotalAmount >= 1000000 ? (TotalAmount / 1000000).ToString("0.#") + "M" : (TotalAmount / 1000).ToString("0") + "K";

        public void AnimateProgress()
        {
            double target = ProgressPercentage;
            int steps = 40;
            int tick = 0;
            AnimatedProgress = 0;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(25) };
            timer.Tick += (s, e) =>
            {
                tick++;
                double t = (double)tick / steps;
                double easedT = 1 - Math.Pow(1 - t, 3);
                AnimatedProgress = easedT * target;
                if (tick >= steps) { AnimatedProgress = target; timer.Stop(); }
            };
            timer.Start();
        }
    }

    public class DashboardViewModel : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set 
            { 
                _currentView = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(IsDashboardActive));
                OnPropertyChanged(nameof(IsTransactionsActive));
                OnPropertyChanged(nameof(IsBudgetsActive));
                OnPropertyChanged(nameof(IsReportsActive));
            }
        }

        public bool IsDashboardActive => CurrentView == _dashboardHomeView;
        public bool IsTransactionsActive => CurrentView == _transactionsView;
        public bool IsBudgetsActive => CurrentView == _budgetsView;
        public bool IsReportsActive => CurrentView == _reportsView;

        private string _pageTitle = "Tổng quan tài chính";
        public string PageTitle
        {
            get => _pageTitle;
            set { _pageTitle = value; OnPropertyChanged(); }
        }

        private string _pageSubtitle = "🟢 Cập nhật theo thời gian thực - Tháng 7, 2026";
        public string PageSubtitle
        {
            get => _pageSubtitle;
            set { _pageSubtitle = value; OnPropertyChanged(); }
        }

        private System.Windows.Visibility _addTransactionVisibility = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility AddTransactionVisibility
        {
            get => _addTransactionVisibility;
            set { _addTransactionVisibility = value; OnPropertyChanged(); }
        }

        private System.Windows.Visibility _addBudgetVisibility = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility AddBudgetVisibility
        {
            get => _addBudgetVisibility;
            set { _addBudgetVisibility = value; OnPropertyChanged(); }
        }

        public bool IsDarkMode => Utils.ThemeManager.IsDarkMode;

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowTransactionsCommand { get; }
        public ICommand ShowBudgetsCommand { get; }
        public ICommand ShowReportsCommand { get; }
        public ICommand ShowProfileCommand { get; }

        public ICommand OpenTransactionDialogCommand { get; }
        public ICommand OpenBudgetDialogCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public DashboardSummaryViewModel TotalBalance { get; set; }
        public DashboardSummaryViewModel TotalIncome { get; set; }
        public DashboardSummaryViewModel TotalExpense { get; set; }
        public DashboardSummaryViewModel NetCashFlow { get; set; }

        public ObservableCollection<TransactionViewModel> RecentTransactions { get; set; }
        public ObservableCollection<BudgetViewModel> BudgetProgresses { get; set; }

        // LiveCharts
        private ISeries[] _chartSeries;
        public ISeries[] ChartSeries { get => _chartSeries; set { _chartSeries = value; OnPropertyChanged(); } }
        public ISeries[] ExpenseAllocationSeries { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        private object _dashboardHomeView;
        private object _transactionsView;
        private object _budgetsView;
        private object _reportsView;
        private object _profileView;

        private DispatcherTimer? _liveChartTimer;
        private Random _rng = new Random();

        // Bright legend text for charts
        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#c0cfe8")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        public DashboardViewModel()
        {
            ShowDashboardCommand = new RelayCommand(_ =>
            {
                if (_dashboardHomeView == null)
                    _dashboardHomeView = new Views.UserControls.DashboardHomeControl { DataContext = this };
                CurrentView = _dashboardHomeView;
                PageTitle = "Tổng quan tài chính";
                PageSubtitle = "🟢 Cập nhật theo thời gian thực - Tháng 7, 2026";
                AddTransactionVisibility = System.Windows.Visibility.Collapsed;
                AddBudgetVisibility = System.Windows.Visibility.Collapsed;
            });
            ShowTransactionsCommand = new RelayCommand(_ =>
            {
                if (_transactionsView == null)
                    _transactionsView = new Views.UserControls.TransactionsViewControl { DataContext = new TransactionsViewModel() };
                CurrentView = _transactionsView;
                PageTitle = "Quản lý Giao dịch";
                PageSubtitle = "Theo dõi và tra cứu biến động số dư";
                AddTransactionVisibility = System.Windows.Visibility.Visible;
                AddBudgetVisibility = System.Windows.Visibility.Collapsed;
            });
            ShowBudgetsCommand = new RelayCommand(_ =>
            {
                if (_budgetsView == null)
                    _budgetsView = new Views.UserControls.BudgetsViewControl { DataContext = new BudgetsViewModel() };
                CurrentView = _budgetsView;
                PageTitle = "Ngân sách & Mục tiêu";
                PageSubtitle = "Quản lý hạn mức chi tiêu hiệu quả";
                AddTransactionVisibility = System.Windows.Visibility.Collapsed;
                AddBudgetVisibility = System.Windows.Visibility.Visible;
            });
            ShowReportsCommand = new RelayCommand(_ =>
            {
                if (_reportsView == null)
                    _reportsView = new Views.UserControls.ReportsViewControl { DataContext = new ReportsViewModel() };
                CurrentView = _reportsView;
                PageTitle = "Phân tích & Báo cáo";
                PageSubtitle = "Bức tranh toàn cảnh về dòng tiền của bạn";
                AddTransactionVisibility = System.Windows.Visibility.Collapsed;
                AddBudgetVisibility = System.Windows.Visibility.Collapsed;
            });
            ShowProfileCommand = new RelayCommand(_ =>
            {
                if (_profileView == null)
                    _profileView = new Views.UserControls.ProfileViewControl { DataContext = new ProfileViewModel() };
                CurrentView = _profileView;
                PageTitle = "Hồ sơ tài khoản";
                PageSubtitle = "Quản lý thông tin cá nhân và bảo mật";
                AddTransactionVisibility = System.Windows.Visibility.Collapsed;
                AddBudgetVisibility = System.Windows.Visibility.Collapsed;
            });

            OpenTransactionDialogCommand = new RelayCommand(_ =>
            {
                var dialog = new Views.Dialogs.TransactionDialogWindow();
                dialog.Owner = System.Windows.Application.Current.MainWindow;
                dialog.ShowDialog();
            });

            OpenBudgetDialogCommand = new RelayCommand(_ =>
            {
                var dialog = new Views.Dialogs.BudgetDialogWindow();
                dialog.Owner = System.Windows.Application.Current.MainWindow;
                dialog.ShowDialog();
            });

            ToggleThemeCommand = new RelayCommand(_ => 
            {
                Utils.ThemeManager.ToggleTheme();
                OnPropertyChanged(nameof(IsDarkMode));
            });

            // Initialize stat cards
            TotalBalance = new DashboardSummaryViewModel { Title = "SỐ DƯ HIỆN TẠI", TargetAmount = 127_500_000, Subtext = "Tất cả tài khoản", ChangePercentage = 8.2, BorderAccentBrush = "#10d9a0", Icon = "💳" };
            TotalIncome  = new DashboardSummaryViewModel { Title = "THU NHẬP THÁNG", TargetAmount = 33_000_000,  Subtext = "Tháng 7/2025",    ChangePercentage = 12.4, BorderAccentBrush = "#3b82f6", Icon = "📈" };
            TotalExpense = new DashboardSummaryViewModel { Title = "CHI TIÊU THÁNG", TargetAmount = 11_800_000,  Subtext = "Tháng 7/2025",    ChangePercentage = -3.1, BorderAccentBrush = "#f43f5e", Icon = "💸" };
            NetCashFlow  = new DashboardSummaryViewModel { Title = "TIẾT KIỆM",      TargetAmount = 21_300_000,  Subtext = "Mục tiêu: 30M₫",  ChangePercentage = 5.7,  BorderAccentBrush = "#f59e0b", Icon = "🎯" };

            // 8 recent transactions
            RecentTransactions = new ObservableCollection<TransactionViewModel>
            {
                new TransactionViewModel { Title = "Nhận lương tháng 7", Category = "Thu nhập",  Amount = 15_000_000, Date = DateTime.Now.AddDays(-1), IsExpense = false, Icon = "💵", IconBackground = "#4510d9a0" },
                new TransactionViewModel { Title = "Đi siêu thị VinMart", Category = "Ăn uống",  Amount = 350_000,    Date = DateTime.Now.AddDays(-1), IsExpense = true,  Icon = "🛒", IconBackground = "#45f59e0b" },
                new TransactionViewModel { Title = "Thưởng dự án Q3",     Category = "Thu nhập",  Amount = 5_000_000,  Date = DateTime.Now.AddDays(-2), IsExpense = false, Icon = "🏆", IconBackground = "#4510d9a0" },
                new TransactionViewModel { Title = "Tiền điện tháng 7",   Category = "Sinh hoạt", Amount = 320_000,    Date = DateTime.Now.AddDays(-3), IsExpense = true,  Icon = "⚡", IconBackground = "#45f43f5e" },
                new TransactionViewModel { Title = "Grab — đi làm",       Category = "Di chuyển", Amount = 45_000,     Date = DateTime.Now.AddDays(-3), IsExpense = true,  Icon = "🚗", IconBackground = "#453b82f6" },
                new TransactionViewModel { Title = "Netflix Premium",      Category = "Giải trí",  Amount = 260_000,    Date = DateTime.Now.AddDays(-4), IsExpense = true,  Icon = "🎬", IconBackground = "#457c6df8" },
                new TransactionViewModel { Title = "Tiền Internet VNPT",  Category = "Sinh hoạt", Amount = 165_000,    Date = DateTime.Now.AddDays(-5), IsExpense = true,  Icon = "🌐", IconBackground = "#453b82f6" },
                new TransactionViewModel { Title = "Freelance design",    Category = "Thu nhập",  Amount = 3_500_000,  Date = DateTime.Now.AddDays(-6), IsExpense = false, Icon = "💼", IconBackground = "#4510d9a0" },
            };

            // 5 budget items
            BudgetProgresses = new ObservableCollection<BudgetViewModel>
            {
                new BudgetViewModel { CategoryName = "Ăn uống",    Icon = "🍜", SpentAmount = 4_800_000,  TotalAmount = 5_000_000 },
                new BudgetViewModel { CategoryName = "Nhà ở & HĐ", Icon = "🏠", SpentAmount = 3_200_000,  TotalAmount = 4_500_000 },
                new BudgetViewModel { CategoryName = "Di chuyển",  Icon = "🚗", SpentAmount = 980_000,    TotalAmount = 1_500_000 },
                new BudgetViewModel { CategoryName = "Giải trí",   Icon = "🎮", SpentAmount = 1_550_000,  TotalAmount = 1_200_000 },
                new BudgetViewModel { CategoryName = "Mua sắm",    Icon = "🛍️", SpentAmount = 2_100_000,  TotalAmount = 3_000_000 },
            };

            InitCharts();

            // Animated counter after a short delay
            var startTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
            startTimer.Tick += (s, e) =>
            {
                startTimer.Stop();
                TotalBalance.AnimateTo(TotalBalance.TargetAmount);
                TotalIncome.AnimateTo(TotalIncome.TargetAmount);
                TotalExpense.AnimateTo(TotalExpense.TargetAmount);
                NetCashFlow.AnimateTo(NetCashFlow.TargetAmount);
                foreach (var b in BudgetProgresses) b.AnimateProgress();
            };
            startTimer.Start();

            // Live chart timer every 2.8 seconds
            _liveChartTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2.8) };
            _liveChartTimer.Tick += OnLiveChartTick;
            _liveChartTimer.Start();

            ShowDashboardCommand.Execute(null);
        }

        private double[] _incomeData = { 22, 25, 27, 30, 28, 33, 31, 35, 30, 29, 32, 33 };
        private double[] _expenseData = { 10, 12, 11, 14, 13, 11, 12, 14, 12, 11, 13, 11.8 };

        private void InitCharts()
        {
            ChartSeries = BuildChartSeries(_incomeData, _expenseData);

            ExpenseAllocationSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 28 }, Name = "Ăn uống",   Fill = new SolidColorPaint(SKColor.Parse("#f59e0b")), InnerRadius = 55, OuterRadiusOffset = 0 },
                new PieSeries<double> { Values = new double[] { 32 }, Name = "Nhà ở",     Fill = new SolidColorPaint(SKColor.Parse("#7c6df8")), InnerRadius = 55 },
                new PieSeries<double> { Values = new double[] { 17 }, Name = "Di chuyển", Fill = new SolidColorPaint(SKColor.Parse("#3b82f6")), InnerRadius = 55 },
                new PieSeries<double> { Values = new double[] { 13 }, Name = "Mua sắm",   Fill = new SolidColorPaint(SKColor.Parse("#f43f5e")), InnerRadius = 55 },
                new PieSeries<double> { Values = new double[] { 10 }, Name = "Khác",      Fill = new SolidColorPaint(SKColor.Parse("#10d9a0")), InnerRadius = 55 },
            };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = new string[] { "T1","T2","T3","T4","T5","T6","T7","T8","T9","T10","T11","T12" },
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")),
                    SeparatorsPaint = null,
                    TicksPaint = null
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")),
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#1a2035")),
                    Labeler = value => value.ToString("0") + "M"
                }
            };
        }

        private ISeries[] BuildChartSeries(double[] income, double[] expense)
        {
            return new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = income,
                    Name = "Thu nhập",
                    Stroke = new SolidColorPaint(SKColor.Parse("#10d9a0")) { StrokeThickness = 2.5f },
                    Fill = new LinearGradientPaint(
                        new[] { SKColor.Parse("#3010d9a0"), SKColor.Parse("#0010d9a0") },
                        new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                    GeometrySize = 6,
                    GeometryFill = new SolidColorPaint(SKColor.Parse("#10d9a0")),
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#080a0f")) { StrokeThickness = 2 },
                    LineSmoothness = 0.5
                },
                new LineSeries<double>
                {
                    Values = expense,
                    Name = "Chi tiêu",
                    Stroke = new SolidColorPaint(SKColor.Parse("#f43f5e")) { StrokeThickness = 2.5f },
                    Fill = new LinearGradientPaint(
                        new[] { SKColor.Parse("#30f43f5e"), SKColor.Parse("#00f43f5e") },
                        new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                    GeometrySize = 6,
                    GeometryFill = new SolidColorPaint(SKColor.Parse("#f43f5e")),
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#080a0f")) { StrokeThickness = 2 },
                    LineSmoothness = 0.5
                }
            };
        }

        private void OnLiveChartTick(object? sender, EventArgs e)
        {
            // Randomly nudge the last data point to simulate live feed
            for (int i = 0; i < _incomeData.Length; i++)
            {
                _incomeData[i] = Math.Max(15, _incomeData[i] + (_rng.NextDouble() - 0.48) * 1.5);
                _expenseData[i] = Math.Max(8,  _expenseData[i] + (_rng.NextDouble() - 0.52) * 0.8);
            }
            ChartSeries = BuildChartSeries((double[])_incomeData.Clone(), (double[])_expenseData.Clone());
        }
    }
}
