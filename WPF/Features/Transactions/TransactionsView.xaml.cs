using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Win32;
using SkiaSharp;
using WPF.Models;
using Services;
using Repositories;

namespace WPF.Features.Transactions
{
    public class TxStatCardModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CategoryPillModel : INotifyPropertyChanged
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
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e7eb"));
        public Brush PillForeground => IsSelected
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10d9a0"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7a8aa8"));

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TransactionGroupModel
    {
        public string DateLabel  { get; set; } = string.Empty;
        public string DayTotal   { get; set; } = string.Empty;
        public Brush  TotalBrush { get; set; } = Brushes.White;
        public ObservableCollection<TransactionModel> Items { get; set; } = new();
    }

    public partial class TransactionsView : UserControl, INotifyPropertyChanged
    {
        private readonly ITransactionService _transactionService;
        private List<TransactionModel> _allRaw = new();

        public TxStatCardModel CardTotal   { get; } = new() { Label = "Tổng giao dịch", Icon = "🔢", AccentColor = "#7c6df8" };
        public TxStatCardModel CardIncome  { get; } = new() { Label = "Tổng thu",        Icon = "📈", AccentColor = "#10d9a0" };
        public TxStatCardModel CardExpense { get; } = new() { Label = "Tổng chi",        Icon = "📉", AccentColor = "#f43f5e" };

        private ISeries[] _chartSeries = Array.Empty<ISeries>();
        public ISeries[] ChartSeries { get => _chartSeries; set { _chartSeries = value; OnPropertyChanged(); } }
        public Axis[] ChartXAxes { get; set; }
        public Axis[] ChartYAxes { get; set; }
        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#6b7280"));

        private int _selectedRange = 30;
        public int SelectedRange { get => _selectedRange; set { _selectedRange = value; OnPropertyChanged(); RefreshChart(); } }

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

        public ObservableCollection<CategoryPillModel> CategoryPills { get; }

        private ObservableCollection<TransactionGroupModel> _groupedList = new();
        public ObservableCollection<TransactionGroupModel> GroupedList
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

        private bool _isDetailOpen;
        public bool IsDetailOpen
        {
            get => _isDetailOpen;
            set { _isDetailOpen = value; OnPropertyChanged(); OnPropertyChanged(nameof(DetailPanelWidth)); }
        }
        public double DetailPanelWidth => IsDetailOpen ? 320 : 0;

        private TransactionModel? _selectedTransaction;
        public TransactionModel? SelectedTransaction
        {
            get => _selectedTransaction;
            set { _selectedTransaction = value; OnPropertyChanged(); IsDetailOpen = value != null; }
        }

        public TransactionsView()
        {
            InitializeComponent();
            this.DataContext = this;

            _transactionService = new TransactionService(new TransactionRepository());

            CategoryPills = new ObservableCollection<CategoryPillModel>();
            var cats = new[] { "Tất cả", "Ăn uống", "Nhà ở", "Di chuyển", "Tiện ích", "Mua sắm", "Giải trí", "Sức khỏe", "Giáo dục" };
            foreach (var c in cats)
            {
                CategoryPills.Add(new CategoryPillModel { Name = c, IsSelected = (c == "Tất cả") });
            }

            ChartXAxes = new Axis[]
            {
                new Axis { LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")), SeparatorsPaint = null }
            };
            ChartYAxes = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568")),
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#e5e7eb")),
                    Labeler = v => v >= 1_000_000 ? (v / 1_000_000).ToString("0.#") + "M" : (v / 1_000).ToString("0") + "K"
                }
            };

            this.Loaded += TransactionsView_Loaded;

            var t2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            t2.Tick += (s, e) => { t2.Stop(); CardTotal.Animate(); CardIncome.Animate(); CardExpense.Animate(); };
            t2.Start();
        }

        private async void TransactionsView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var transactionsDb = await _transactionService.GetTransactionsByYearAsync(1, DateTime.Now.Year);
                _allRaw = transactionsDb.Select(t => new TransactionModel
                {
                    Title = t.Description ?? "Giao dịch",
                    Category = t.Category?.CategoryName ?? "Khác",
                    Amount = t.Amount,
                    Date = t.TransactionDate.ToDateTime(TimeOnly.MinValue),
                    IsExpense = t.TransactionType == "Expense",
                    Status = "Hoàn thành",
                    Icon = t.TransactionType == "Expense" ? "🔥" : "💵",
                    IconBackground = t.TransactionType == "Expense" ? "#1Affb4ab" : "#1A10b981"
                }).ToList();

                RefreshChart();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu giao dịch: " + ex.Message);
            }
        }

        private void Pill_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CategoryPillModel pill)
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
        }

        private void SetRange7_Click(object sender, RoutedEventArgs e) => SelectedRange = 7;
        private void SetRange30_Click(object sender, RoutedEventArgs e) => SelectedRange = 30;
        private void SetRange90_Click(object sender, RoutedEventArgs e) => SelectedRange = 90;

        private void SetAll_Click(object sender, RoutedEventArgs e) => TypeFilter = "Tất cả";
        private void SetIncome_Click(object sender, RoutedEventArgs e) => TypeFilter = "Thu nhập";
        private void SetExpense_Click(object sender, RoutedEventArgs e) => TypeFilter = "Chi tiêu";

        private void PrevPage_Click(object sender, RoutedEventArgs e) { if (CanPrev) PageIndex--; }
        private void NextPage_Click(object sender, RoutedEventArgs e) { if (CanNext) PageIndex++; }

        private void SelectTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is TransactionModel tx)
                SelectedTransaction = tx;
        }

        private void CloseDetail_Click(object sender, RoutedEventArgs e) => SelectedTransaction = null;

        private void EditTransaction_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Edit transaction logic
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTransaction != null)
            {
                _allRaw.Remove(SelectedTransaction);
                ApplyFilters();
                SelectedTransaction = null;
            }
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "giao_dich.csv" };
            if (dlg.ShowDialog() != true) return;
            var sb = new StringBuilder("Ngày,Tên,Danh mục,Loại,Số tiền\n");
            foreach (var t in _allRaw.OrderByDescending(x => x.Date))
                sb.AppendLine($"{t.Date:dd/MM/yyyy},{t.Title},{t.Category},{(t.IsExpense?"Chi tiêu":"Thu nhập")},{t.Amount}");
            System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
        }

        private void ApplyFilters()
        {
            var filtered = _allRaw.AsEnumerable();

            if (TypeFilter == "Thu nhập")  filtered = filtered.Where(t => !t.IsExpense);
            if (TypeFilter == "Chi tiêu")  filtered = filtered.Where(t =>  t.IsExpense);

            var selectedCats = CategoryPills.Skip(1).Where(p => p.IsSelected).Select(p => p.Name).ToList();
            if (selectedCats.Count > 0)
                filtered = filtered.Where(t => selectedCats.Contains(t.Category));

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var q = SearchText.Trim().ToLower();
                filtered = filtered.Where(t =>
                    t.Title.ToLower().Contains(q) ||
                    t.Category.ToLower().Contains(q));
            }

            var list = filtered.OrderByDescending(t => t.Date).ToList();
            _totalFiltered = list.Count;

            CardTotal.TargetValue  = list.Count;
            CardIncome.TargetValue  = list.Where(t => !t.IsExpense).Sum(t => t.Amount);
            CardExpense.TargetValue = list.Where(t => t.IsExpense).Sum(t => t.Amount);
            CardTotal.Value   = CardTotal.TargetValue;
            CardIncome.Value  = CardIncome.TargetValue;
            CardExpense.Value = CardExpense.TargetValue;

            var paged = list.Skip(PageIndex * _pageSize).Take(_pageSize).ToList();

            var groups = paged
                .GroupBy(t => t.Date.Date)
                .OrderByDescending(g => g.Key)
                .Select(g =>
                {
                    var netAmount = g.Sum(t => t.IsExpense ? -t.Amount : t.Amount);
                    string dateLabel = g.Key.Date == DateTime.Today           ? "Hôm nay"
                                    : g.Key.Date == DateTime.Today.AddDays(-1) ? "Hôm qua"
                                    : g.Key.ToString("dd/MM/yyyy");
                    return new TransactionGroupModel
                    {
                        DateLabel  = dateLabel,
                        DayTotal   = (netAmount >= 0 ? "+" : "") + FormatAmount(netAmount),
                        TotalBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(
                                         netAmount >= 0 ? "#10d9a0" : "#f43f5e")),
                        Items = new ObservableCollection<TransactionModel>(g.OrderByDescending(t => t.Date))
                    };
                }).ToList();

            GroupedList = new ObservableCollection<TransactionGroupModel>(groups);

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
                    Fill = new SolidColorPaint(SKColor.Parse("#10b981")),
                    MaxBarWidth = 10,
                    Rx = 2, Ry = 2
                },
                new ColumnSeries<double>
                {
                    Values = expenseVals,
                    Name = "Chi tiêu",
                    Fill = new SolidColorPaint(SKColor.Parse("#818cf8")),
                    MaxBarWidth = 10,
                    Rx = 2, Ry = 2
                }
            };
        }

        private static string FormatAmount(decimal v)
        {
            var abs = Math.Abs(v);
            return abs >= 1_000_000 ? (v / 1_000_000).ToString("0.#") + "M₫" : (v / 1_000).ToString("0") + "K₫";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
