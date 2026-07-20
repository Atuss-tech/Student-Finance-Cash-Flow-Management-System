using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Services;
using Repositories;

namespace WPF.Features.Categories
{
    public class CategoryItemData : INotifyPropertyChanged
    {
        public int    CategoryId     { get; set; }
        public int    UserId         { get; set; }
        public string Name           { get; set; } = string.Empty;
        public string Type           { get; set; } = string.Empty;
        public string Icon           { get; set; } = string.Empty;
        public string IconBackground { get; set; } = string.Empty;
        public string ColorHex       { get; set; } = "#3b82f6";
        public string Description    { get; set; } = string.Empty;

        public double CurrentSpending { get; set; }
        public double BudgetLimit     { get; set; }
        public double ProgressPercentage => BudgetLimit > 0 ? (CurrentSpending / BudgetLimit) * 100 : 0;
        public string CurrentSpendingText => $"{CurrentSpending:N0} đ";
        public string BudgetLimitText     => $"{BudgetLimit:N0} đ";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public partial class CategoriesView : UserControl, INotifyPropertyChanged
    {
        // ── Stat card properties ──────────────────────────────────────────
        private int _totalCategoryCount = 0;
        public int TotalCategoryCount
        {
            get => _totalCategoryCount;
            set { _totalCategoryCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalCategories)); }
        }

        // "22 Danh mục" — dùng cho binding label phụ nếu cần
        public string TotalCategories => $"{_totalCategoryCount} Danh mục";

        private string _highestSpendingCategory = "—";
        public string HighestSpendingCategory
        {
            get => _highestSpendingCategory;
            set { _highestSpendingCategory = value; OnPropertyChanged(); }
        }

        private string _highestSpendingAmount = "";
        public string HighestSpendingAmount
        {
            get => _highestSpendingAmount;
            set { _highestSpendingAmount = value; OnPropertyChanged(); }
        }

        private double _budgetHealthPercentage = 0;
        public double BudgetHealthPercentage
        {
            get => _budgetHealthPercentage;
            set { _budgetHealthPercentage = value; OnPropertyChanged(); }
        }

        // ── List & chart data ─────────────────────────────────────────────
        public ObservableCollection<CategoryItemData> Categories { get; set; } = new();

        private ISeries[] _categoryBreakdownSeries = Array.Empty<ISeries>();
        public ISeries[] CategoryBreakdownSeries
        {
            get => _categoryBreakdownSeries;
            set { _categoryBreakdownSeries = value; OnPropertyChanged(); }
        }

        public SolidColorPaint ChartLegendTextPaint { get; } =
            new SolidColorPaint(SKColor.Parse("#6b7280"))
            {
                SKTypeface = SkiaSharp.SKTypeface.FromFamilyName("Segoe UI")
            };

        // ── Services ──────────────────────────────────────────────────────
        private readonly ICategoryService _categoryService;
        private readonly IReportService _reportService;
        private readonly ITransactionRepository _transactionRepository;

        public CategoriesView()
        {
            InitializeComponent();
            this.DataContext = this;

            _categoryService     = new CategoryService();
            _transactionRepository = new TransactionRepository();
            _reportService       = new ReportService(_transactionRepository);

            this.Loaded += CategoriesView_Loaded;
        }

        private async void CategoriesView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        // ── Reload helper (called after add/edit/delete) ─────────────────
        public async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                int month = DateTime.Now.Month;
                int year  = DateTime.Now.Year;

                // 1. Lấy TOÀN BỘ danh mục (tất cả userId)
                var rawCategories = _categoryService.GetAllCategories();

                // 2. Lấy chi tiêu thực theo danh mục cho tháng hiện tại
                //    (dùng userId=1 — tích hợp multi-user sau nếu cần)
                var expenseByCategory = await _reportService.GetExpenseByCategoryAsync(1, month, year);

                // 3. Icon mapping
                var iconMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Ăn uống"]       = "🍜",  ["Food"]          = "🍜",
                    ["Di chuyển"]     = "🚗",  ["Transport"]     = "🚗",
                    ["Mua sắm"]       = "🛍️",  ["Shopping"]      = "🛍️",
                    ["Giải trí"]      = "🎮",  ["Entertainment"] = "🎮",
                    ["Sức khỏe"]      = "💊",  ["Health"]        = "💊",
                    ["Giáo dục"]      = "📚",  ["Education"]     = "📚",
                    ["Điện nước"]     = "💡",  ["Utilities"]     = "💡",
                    ["Tiết kiệm"]     = "🏦",  ["Savings"]       = "🏦",
                    ["Lương"]         = "💵",  ["Salary"]        = "💵",
                    ["Đầu tư"]        = "📈",  ["Investment"]    = "📈",
                    ["Nhà ở"]         = "🏠",  ["Housing"]       = "🏠",
                    ["Bảo hiểm"]      = "🛡️",  ["Insurance"]     = "🛡️",
                    ["Du lịch"]       = "✈️",  ["Travel"]        = "✈️",
                    ["Thể thao"]      = "🏋️",  ["Fitness"]       = "🏋️",
                    ["Quà tặng"]      = "🎁",  ["Gifts"]         = "🎁",
                    ["Gia đình"]      = "👨‍👩‍👧", ["Family"]        = "👨‍👩‍👧",
                    ["Thu nhập khác"] = "💰",  ["Other Income"]  = "💰",
                    ["Chi khác"]      = "📦",  ["Other"]         = "📦",
                    ["Ăn sáng"]       = "🍳",
                    ["Shopping"]      = "🛍️",
                    ["Học bổng FPT"]  = "🎓",
                };

                var colorPalette = new[]
                {
                    "#f43f5e", "#f59e0b", "#7c6df8", "#3b82f6",
                    "#10d9a0", "#8b5cf6", "#06b6d4", "#ec4899",
                    "#84cc16", "#f97316", "#14b8a6", "#6366f1",
                    "#a855f7", "#22d3ee", "#fb923c", "#4ade80",
                    "#e879f9", "#38bdf8", "#fbbf24", "#34d399",
                    "#f87171", "#60a5fa", "#a3e635",
                };

                // 4. Populate danh sách phân bổ (hiển thị tất cả 23+)
                Categories.Clear();
                int colorIdx = 0;
                foreach (var cat in rawCategories)
                {
                    string name     = cat.CategoryName;
                    bool   isIncome = cat.CategoryType == "Income";
                    string icon     = iconMap.TryGetValue(name, out var ico) ? ico : (isIncome ? "💰" : "📦");
                    double spending = expenseByCategory.TryGetValue(name, out var sp) ? (double)sp : 0;
                    string color    = colorPalette[colorIdx % colorPalette.Length];
                    colorIdx++;

                    Categories.Add(new CategoryItemData
                    {
                        CategoryId      = cat.CategoryId,
                        UserId          = cat.UserId,
                        Name            = name,
                        Type            = isIncome ? "Thu nhập" : "Chi tiêu",
                        Icon            = icon,
                        IconBackground  = color + "33",
                        ColorHex        = color,
                        Description     = cat.Description ?? "",
                        CurrentSpending = spending,
                        BudgetLimit     = 0
                    });
                }

                // 5. Stat cards
                TotalCategoryCount = rawCategories.Count;   // hiển thị 23

                if (expenseByCategory.Count > 0)
                {
                    var top = expenseByCategory.OrderByDescending(x => x.Value).First();
                    HighestSpendingCategory = top.Key;
                    HighestSpendingAmount   = FormatAmount(top.Value);
                }
                else
                {
                    HighestSpendingCategory = "Chưa có";
                    HighestSpendingAmount   = "";
                }

                // 6. Pie chart: mỗi danh mục 1 lát cắt
                //    - Danh mục có chi tiêu → giá trị thực
                //    - Danh mục chưa có chi tiêu → 1 đơn vị (vẫn hiển thị)
                var pieList = new List<PieSeries<double>>();
                colorIdx = 0;
                foreach (var cat in rawCategories)
                {
                    string name    = cat.CategoryName;
                    double value   = expenseByCategory.TryGetValue(name, out var sv) && sv > 0
                                     ? (double)sv
                                     : 1.0;  // placeholder để danh mục vẫn xuất hiện
                    string color   = colorPalette[colorIdx % colorPalette.Length];
                    colorIdx++;

                    pieList.Add(new PieSeries<double>
                    {
                        Values      = new double[] { value },
                        Name        = name,
                        InnerRadius = 50,
                        Fill        = new SolidColorPaint(SKColor.Parse(color))
                    });
                }

                CategoryBreakdownSeries = pieList.ToArray();

                // 7. Budget health (placeholder)
                double totalExpense = expenseByCategory.Values.Sum(v => (double)v);
                BudgetHealthPercentage = totalExpense > 0 ? Math.Min(68, 100) : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi tải dữ liệu danh mục:\n" + ex.Message,
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────
        private static string FormatAmount(decimal amount)
        {
            if (amount >= 1_000_000) return (amount / 1_000_000).ToString("0.#") + "Mđ";
            if (amount >= 1_000)     return (amount / 1_000).ToString("0") + "Kđ";
            return amount.ToString("0") + "đ";
        }

        // ── Menu 3 chấm ──────────────────────────────────────────────────
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                // Mở ContextMenu ngay tại button
                if (btn.ContextMenu != null)
                {
                    btn.ContextMenu.PlacementTarget = btn;
                    btn.ContextMenu.IsOpen = true;
                }
            }
        }

        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is CategoryItemData item)
            {
                var window = new EditCategoryWindow(item.CategoryId, item.UserId, item.Name, item.Type, item.Description);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true)
                    await LoadDataAsync();
            }
        }

        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is CategoryItemData item)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa danh mục \"{item.Name}\" không?\nĐiều này không thể hoàn tác.",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _categoryService.DeleteCategory(item.UserId, item.CategoryId);
                        await LoadDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // ── Filter buttons (to be implemented) ──────────────────────────
        private void SetAll_Click(object sender, RoutedEventArgs e)    { /* TODO */ }
        private void SetIncome_Click(object sender, RoutedEventArgs e) { /* TODO */ }
        private void SetExpense_Click(object sender, RoutedEventArgs e){ /* TODO */ }

        // ── Add category ─────────────────────────────────────────────────
        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddCategoryWindow();
            if (window.ShowDialog() == true)
            {
                await LoadDataAsync();   // await đúng cách để không bỏ qua lỗi
            }
        }

        // ── INotifyPropertyChanged ────────────────────────────────────────
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
