using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Win32;
using SkiaSharp;

namespace WPF.ViewModels
{
    // ─────────────────────────────────────────────
    // Mini stat card for Transactions screen
    // ─────────────────────────────────────────────
    public class TxStatCardViewModel : ViewModelBase
    {
        public string Label    { get; set; } = string.Empty;
        public string Icon     { get; set; } = string.Empty;
        public string AccentColor { get; set; } = "#10d9a0";
        public Brush  AccentBrush => new SolidColorBrush((Color)ColorConverter.ConvertFromString(AccentColor));
        public Brush  AccentBg
        {
            get
            {
                var c = (Color)ColorConverter.ConvertFromString(AccentColor);
                return new SolidColorBrush(Color.FromArgb(40, c.R, c.G, c.B));
            }
        }

        private decimal _targetValue;
        public decimal TargetValue { get => _targetValue; set { _targetValue = value; } }

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
                if (Label == "Tổng giao dịch") return Value.ToString("0");
                return Value >= 1_000_000
                    ? (Value / 1_000_000).ToString("0.#") + "M₫"
                    : (Value / 1_000).ToString("0") + "K₫";
            }
        }

        public void Animate()
        {
            int steps = 35;
            int tick  = 0;
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

    // ─────────────────────────────────────────────
    // Category filter pill
    // ─────────────────────────────────────────────
    public class CategoryPillViewModel : ViewModelBase
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); OnPropertyChanged(nameof(PillBackground)); OnPropertyChanged(nameof(PillForeground)); }
        }

        public Brush PillBackground => IsSelected
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10d9a0")) { Opacity = 0.15 }
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1a2035"));
        public Brush PillForeground => IsSelected
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10d9a0"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7a8aa8"));

        public ICommand ToggleCommand { get; set; }

        public CategoryPillViewModel() { ToggleCommand = new RelayCommand(_ => IsSelected = !IsSelected); }
    }

    // ─────────────────────────────────────────────
    // Transaction group (by date)
    // ─────────────────────────────────────────────
    public class TransactionGroupViewModel
    {
        public string DateLabel  { get; set; } = string.Empty;
        public string DayTotal   { get; set; } = string.Empty;
        public Brush  TotalBrush { get; set; } = Brushes.White;
        public ObservableCollection<TransactionViewModel> Items { get; set; } = new();
    }

    // ─────────────────────────────────────────────
    // MAIN Transactions ViewModel
    // ─────────────────────────────────────────────
    public class TransactionsViewModel : ViewModelBase
    {
        // ── Raw data ──────────────────────────────
        private readonly List<TransactionViewModel> _allRaw;

        // ── Stat cards ───────────────────────────
        public TxStatCardViewModel CardTotal   { get; } = new() { Label = "Tổng giao dịch", Icon = "🔢", AccentColor = "#7c6df8" };
        public TxStatCardViewModel CardIncome  { get; } = new() { Label = "Tổng thu",        Icon = "📈", AccentColor = "#10d9a0" };
        public TxStatCardViewModel CardExpense { get; } = new() { Label = "Tổng chi",        Icon = "📉", AccentColor = "#f43f5e" };

        // ── Chart ─────────────────────────────────
        private ISeries[] _chartSeries;
        public ISeries[] ChartSeries { get => _chartSeries; set { _chartSeries = value; OnPropertyChanged(); } }
        public Axis[] ChartXAxes { get; set; }
        public Axis[] ChartYAxes { get; set; }
        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#c0cfe8"));

        private int _selectedRange = 30;
        public int SelectedRange { get => _selectedRange; set { _selectedRange = value; OnPropertyChanged(); RefreshChart(); } }

        public ICommand SetRange7Command  { get; }
        public ICommand SetRange30Command { get; }
        public ICommand SetRange90Command { get; }

        // ── Filters ──────────────────────────────
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); PageIndex = 0; ApplyFilters(); }
        }

        private string _typeFilter = "Tất cả";
        public string TypeFilter
        {
            get => _typeFilter;
            set { _typeFilter = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsAll)); OnPropertyChanged(nameof(IsIncome)); OnPropertyChanged(nameof(IsExpense)); PageIndex = 0; ApplyFilters(); }
        }
        public bool IsAll     => TypeFilter == "Tất cả";
        public bool IsIncome  => TypeFilter == "Thu nhập";
        public bool IsExpense => TypeFilter == "Chi tiêu";

        public ICommand SetAllCommand     { get; }
        public ICommand SetIncomeCommand  { get; }
        public ICommand SetExpenseCommand { get; }

        public ObservableCollection<CategoryPillViewModel> CategoryPills { get; }

        // ── Grouped & Paged list ─────────────────
        private ObservableCollection<TransactionGroupViewModel> _groupedList = new();
        public ObservableCollection<TransactionGroupViewModel> GroupedList
        {
            get => _groupedList;
            set { _groupedList = value; OnPropertyChanged(); }
        }

        private int _pageIndex;
        public int PageIndex
        {
            get => _pageIndex;
            set { _pageIndex = value; OnPropertyChanged(); OnPropertyChanged(nameof(PageLabel)); OnPropertyChanged(nameof(CanPrev)); OnPropertyChanged(nameof(CanNext)); ApplyFilters(); }
        }
        private int _pageSize = 10;
        private int _totalFiltered;

        public string PageLabel => $"Trang {PageIndex + 1} / {Math.Max(1, (int)Math.Ceiling(_totalFiltered / (double)_pageSize))}";
        public bool CanPrev => PageIndex > 0;
        public bool CanNext => (PageIndex + 1) * _pageSize < _totalFiltered;
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }

        // ── Detail panel ─────────────────────────
        private bool _isDetailOpen;
        public bool IsDetailOpen
        {
            get => _isDetailOpen;
            set { _isDetailOpen = value; OnPropertyChanged(); OnPropertyChanged(nameof(DetailPanelWidth)); }
        }
        public double DetailPanelWidth => IsDetailOpen ? 320 : 0;

        private TransactionViewModel? _selectedTransaction;
        public TransactionViewModel? SelectedTransaction
        {
            get => _selectedTransaction;
            set { _selectedTransaction = value; OnPropertyChanged(); IsDetailOpen = value != null; }
        }
        public ICommand SelectTransactionCommand { get; }
        public ICommand CloseDetailCommand { get; }
        public ICommand EditCommand   { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ExportCsvCommand { get; }

        // ─────────────────────────────────────────
        public TransactionsViewModel()
        {
            _allRaw = BuildSampleData();

            // Category pills
            CategoryPills = new ObservableCollection<CategoryPillViewModel>
            {
                new() { Name = "Tất cả",     Icon = "✨" },
                new() { Name = "Ăn uống",    Icon = "🍜" },
                new() { Name = "Di chuyển",  Icon = "🚗" },
                new() { Name = "Mua sắm",    Icon = "🛍️" },
                new() { Name = "Giải trí",   Icon = "🎮" },
                new() { Name = "Sinh hoạt",  Icon = "🏠" },
                new() { Name = "Giáo dục",   Icon = "📚" },
                new() { Name = "Thu nhập",   Icon = "💵" },
                new() { Name = "Sức khỏe",   Icon = "❤️" },
            };
            CategoryPills[0].IsSelected = true;
            foreach (var pill in CategoryPills)
                pill.ToggleCommand = new RelayCommand(_ => { OnCategoryPillToggled(pill); });

            // Range commands
            SetRange7Command  = new RelayCommand(_ => SelectedRange = 7);
            SetRange30Command = new RelayCommand(_ => SelectedRange = 30);
            SetRange90Command = new RelayCommand(_ => SelectedRange = 90);

            // Type filter commands
            SetAllCommand     = new RelayCommand(_ => TypeFilter = "Tất cả");
            SetIncomeCommand  = new RelayCommand(_ => TypeFilter = "Thu nhập");
            SetExpenseCommand = new RelayCommand(_ => TypeFilter = "Chi tiêu");

            // Pagination
            PrevPageCommand = new RelayCommand(_ => { if (CanPrev) PageIndex--; }, _ => CanPrev);
            NextPageCommand = new RelayCommand(_ => { if (CanNext) PageIndex++; }, _ => CanNext);

            // Detail
            SelectTransactionCommand = new RelayCommand(tx => SelectedTransaction = tx as TransactionViewModel);
            CloseDetailCommand = new RelayCommand(_ => SelectedTransaction = null);
            EditCommand   = new RelayCommand(_ => { /* TODO */ });
            DeleteCommand = new RelayCommand(tx =>
            {
                if (tx is TransactionViewModel t) { _allRaw.Remove(t); ApplyFilters(); SelectedTransaction = null; }
            });
            ExportCsvCommand = new RelayCommand(_ => ExportCsv());

            // Init chart axes
            ChartXAxes = new Axis[]
            {
                new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), SeparatorsPaint = null }
            };
            ChartYAxes = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")),
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#1a2035")),
                    Labeler = v => v >= 1_000_000 ? (v / 1_000_000).ToString("0.#") + "M" : (v / 1_000).ToString("0") + "K"
                }
            };

            RefreshChart();
            ApplyFilters();

            // Animate stat cards
            var t2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            t2.Tick += (s, e) => { t2.Stop(); CardTotal.Animate(); CardIncome.Animate(); CardExpense.Animate(); };
            t2.Start();
        }

        private void OnCategoryPillToggled(CategoryPillViewModel pill)
        {
            if (pill.Name == "Tất cả")
            {
                foreach (var p in CategoryPills) p.IsSelected = false;
                pill.IsSelected = true;
            }
            else
            {
                CategoryPills[0].IsSelected = false;
                pill.IsSelected = !pill.IsSelected;
                if (!CategoryPills.Skip(1).Any(p => p.IsSelected))
                    CategoryPills[0].IsSelected = true;
            }
            PageIndex = 0;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = _allRaw.AsEnumerable();

            // Type filter
            if (TypeFilter == "Thu nhập")  filtered = filtered.Where(t => !t.IsExpense);
            if (TypeFilter == "Chi tiêu")  filtered = filtered.Where(t =>  t.IsExpense);

            // Category pills
            var selectedCats = CategoryPills.Skip(1).Where(p => p.IsSelected).Select(p => p.Name).ToList();
            if (selectedCats.Count > 0)
                filtered = filtered.Where(t => selectedCats.Contains(t.Category));

            // Search
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var q = SearchText.Trim().ToLower();
                filtered = filtered.Where(t =>
                    t.Title.ToLower().Contains(q) ||
                    t.Category.ToLower().Contains(q));
            }

            var list = filtered.OrderByDescending(t => t.Date).ToList();
            _totalFiltered = list.Count;

            // Update stat cards
            CardTotal.TargetValue  = list.Count;
            CardIncome.TargetValue  = list.Where(t => !t.IsExpense).Sum(t => t.Amount);
            CardExpense.TargetValue = list.Where(t => t.IsExpense).Sum(t => t.Amount);
            CardTotal.Value   = CardTotal.TargetValue;
            CardIncome.Value  = CardIncome.TargetValue;
            CardExpense.Value = CardExpense.TargetValue;

            // Page
            var paged = list.Skip(PageIndex * _pageSize).Take(_pageSize).ToList();

            // Group by date
            var groups = paged
                .GroupBy(t => t.Date.Date)
                .OrderByDescending(g => g.Key)
                .Select(g =>
                {
                    var netAmount = g.Sum(t => t.IsExpense ? -t.Amount : t.Amount);
                    string dateLabel = g.Key.Date == DateTime.Today           ? "Hôm nay"
                                    : g.Key.Date == DateTime.Today.AddDays(-1) ? "Hôm qua"
                                    : g.Key.ToString("dd/MM/yyyy");
                    return new TransactionGroupViewModel
                    {
                        DateLabel  = dateLabel,
                        DayTotal   = (netAmount >= 0 ? "+" : "") + FormatAmount(netAmount),
                        TotalBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(
                                         netAmount >= 0 ? "#10d9a0" : "#f43f5e")),
                        Items = new ObservableCollection<TransactionViewModel>(g.OrderByDescending(t => t.Date))
                    };
                }).ToList();

            GroupedList = new ObservableCollection<TransactionGroupViewModel>(groups);

            OnPropertyChanged(nameof(PageLabel));
            OnPropertyChanged(nameof(CanPrev));
            OnPropertyChanged(nameof(CanNext));
        }

        private void RefreshChart()
        {
            var days = SelectedRange;
            var incomeVals = new double[days];
            var expenseVals = new double[days];
            var labels = new string[days];

            for (int i = 0; i < days; i++)
            {
                var day = DateTime.Today.AddDays(-(days - 1 - i));
                var txDay = _allRaw.Where(t => t.Date.Date == day.Date).ToList();
                incomeVals[i]  = (double)txDay.Where(t => !t.IsExpense).Sum(t => t.Amount);
                expenseVals[i] = (double)txDay.Where(t =>  t.IsExpense).Sum(t => t.Amount);
                labels[i] = days <= 7 ? day.ToString("ddd") : (i % 5 == 0 ? day.ToString("dd/MM") : "");
            }

            ChartXAxes[0].Labels = labels;
            ChartSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = incomeVals,
                    Name = "Thu nhập",
                    Fill = new SolidColorPaint(SKColor.Parse("#10d9a0")),
                    MaxBarWidth = 12,
                    Rx = 4, Ry = 4
                },
                new ColumnSeries<double>
                {
                    Values = expenseVals,
                    Name = "Chi tiêu",
                    Fill = new SolidColorPaint(SKColor.Parse("#7c6df8")),
                    MaxBarWidth = 12,
                    Rx = 4, Ry = 4
                }
            };
        }

        private void ExportCsv()
        {
            var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "giao_dich.csv" };
            if (dlg.ShowDialog() != true) return;
            var sb = new StringBuilder("Ngày,Tên,Danh mục,Loại,Số tiền\n");
            foreach (var t in _allRaw.OrderByDescending(x => x.Date))
                sb.AppendLine($"{t.Date:dd/MM/yyyy},{t.Title},{t.Category},{(t.IsExpense?"Chi tiêu":"Thu nhập")},{t.Amount}");
            System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
        }

        private static string FormatAmount(decimal v)
        {
            var abs = Math.Abs(v);
            return abs >= 1_000_000 ? (v / 1_000_000).ToString("0.#") + "M₫" : (v / 1_000).ToString("0") + "K₫";
        }

        private static List<TransactionViewModel> BuildSampleData()
        {
            var now = DateTime.Now;
            return new List<TransactionViewModel>
            {
                // Today
                new() { Title="Nhận lương tháng 7",  Category="Thu nhập",  Amount=15_000_000, Date=now.AddHours(-2),   IsExpense=false, Icon="💵", IconBackground="#4510d9a0" },
                new() { Title="Cà phê sáng",          Category="Ăn uống",   Amount=35_000,     Date=now.AddHours(-5),   IsExpense=true,  Icon="☕", IconBackground="#45f59e0b" },
                // Yesterday
                new() { Title="Đi siêu thị VinMart",  Category="Ăn uống",   Amount=350_000,    Date=now.AddDays(-1),    IsExpense=true,  Icon="🛒", IconBackground="#45f59e0b" },
                new() { Title="Grab — đi làm",         Category="Di chuyển", Amount=45_000,     Date=now.AddDays(-1).AddHours(-3), IsExpense=true, Icon="🚗", IconBackground="#453b82f6" },
                new() { Title="Bán đồ cũ online",      Category="Thu nhập",  Amount=800_000,    Date=now.AddDays(-1).AddHours(-5), IsExpense=false, Icon="💰", IconBackground="#4510d9a0" },
                // 2 days ago
                new() { Title="Thưởng dự án Q3",       Category="Thu nhập",  Amount=5_000_000,  Date=now.AddDays(-2),   IsExpense=false, Icon="🏆", IconBackground="#4510d9a0" },
                new() { Title="Netflix Premium",        Category="Giải trí",  Amount=260_000,    Date=now.AddDays(-2).AddHours(-2), IsExpense=true, Icon="🎬", IconBackground="#457c6df8" },
                new() { Title="Khám sức khỏe",          Category="Sức khỏe", Amount=300_000,    Date=now.AddDays(-2).AddHours(-4), IsExpense=true, Icon="❤️", IconBackground="#45f43f5e" },
                // 3 days ago
                new() { Title="Tiền điện tháng 7",     Category="Sinh hoạt", Amount=320_000,    Date=now.AddDays(-3),   IsExpense=true,  Icon="⚡", IconBackground="#45f43f5e" },
                new() { Title="Tiền nước tháng 7",     Category="Sinh hoạt", Amount=85_000,     Date=now.AddDays(-3).AddHours(-1), IsExpense=true, Icon="💧", IconBackground="#45f43f5e" },
                new() { Title="Mua sách lập trình",    Category="Giáo dục",  Amount=185_000,    Date=now.AddDays(-3).AddHours(-3), IsExpense=true, Icon="📚", IconBackground="#457c6df8" },
                // 4 days ago
                new() { Title="Freelance design",       Category="Thu nhập",  Amount=3_500_000,  Date=now.AddDays(-4),   IsExpense=false, Icon="💼", IconBackground="#4510d9a0" },
                new() { Title="Ăn ngoài — KFC",        Category="Ăn uống",   Amount=165_000,    Date=now.AddDays(-4).AddHours(-2), IsExpense=true, Icon="🍗", IconBackground="#45f59e0b" },
                new() { Title="Mua quần áo",            Category="Mua sắm",  Amount=450_000,    Date=now.AddDays(-4).AddHours(-4), IsExpense=true, Icon="👕", IconBackground="#45f43f5e" },
                // 5 days ago
                new() { Title="Tiền Internet VNPT",    Category="Sinh hoạt", Amount=165_000,    Date=now.AddDays(-5),   IsExpense=true,  Icon="🌐", IconBackground="#453b82f6" },
                new() { Title="Grab Food — bữa tối",   Category="Ăn uống",   Amount=89_000,     Date=now.AddDays(-5).AddHours(-2), IsExpense=true, Icon="🍔", IconBackground="#45f59e0b" },
                new() { Title="Khóa học Udemy",         Category="Giáo dục",  Amount=450_000,    Date=now.AddDays(-5).AddHours(-4), IsExpense=true, Icon="🎓", IconBackground="#457c6df8" },
                // 6 days ago
                new() { Title="Thu nhập freelance",    Category="Thu nhập",  Amount=2_200_000,  Date=now.AddDays(-6),   IsExpense=false, Icon="💻", IconBackground="#4510d9a0" },
                new() { Title="Taxi — đi sân bay",     Category="Di chuyển", Amount=280_000,    Date=now.AddDays(-6).AddHours(-2), IsExpense=true, Icon="✈️", IconBackground="#453b82f6" },
                // 7 days ago
                new() { Title="Mua giày thể thao",     Category="Mua sắm",  Amount=1_200_000,  Date=now.AddDays(-7),   IsExpense=true,  Icon="👟", IconBackground="#45f43f5e" },
                new() { Title="Rút tiền ATM",           Category="Thu nhập",  Amount=2_000_000,  Date=now.AddDays(-7).AddHours(-1), IsExpense=false, Icon="🏧", IconBackground="#4510d9a0" },
                // Older
                new() { Title="Tiền thuê nhà",         Category="Sinh hoạt", Amount=3_500_000,  Date=now.AddDays(-10),  IsExpense=true,  Icon="🏠", IconBackground="#45f43f5e" },
                new() { Title="Thẻ tháng xe buýt",     Category="Di chuyển", Amount=200_000,    Date=now.AddDays(-12),  IsExpense=true,  Icon="🚌", IconBackground="#453b82f6" },
                new() { Title="Lương thêm giờ",        Category="Thu nhập",  Amount=1_800_000,  Date=now.AddDays(-15),  IsExpense=false, Icon="⏰", IconBackground="#4510d9a0" },
                new() { Title="Spa &amp; thư giãn",    Category="Sức khỏe", Amount=380_000,    Date=now.AddDays(-18),  IsExpense=true,  Icon="💆", IconBackground="#457c6df8" },
            };
        }
    }
}
