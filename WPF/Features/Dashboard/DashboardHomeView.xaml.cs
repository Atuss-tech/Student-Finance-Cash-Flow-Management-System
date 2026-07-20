using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using WPF.UIData;
using Services;
using Repositories;
using System.Collections.Generic;

namespace WPF.Features.Dashboard
{
    /// <summary>
    /// UserControl hiển thị màn hình Dashboard chính (Tổng quan số dư, giao dịch gần đây, tiến độ ngân sách, biểu đồ phân bổ chi tiêu và xu hướng dòng tiền).
    /// Giao tiếp với Database thông qua các Service.
    /// </summary>
    public partial class DashboardHomeView : UserControl, INotifyPropertyChanged
    {
        // --- CÁC PROPERTY RÀNG BUỘC (BINDING) LÊN GIAO DIỆN ---
        
        public DashboardSummaryData TotalBalance { get; set; }
        public DashboardSummaryData TotalIncome { get; set; }
        public DashboardSummaryData TotalExpense { get; set; }
        public DashboardSummaryData NetCashFlow { get; set; }

        public ObservableCollection<TransactionData> RecentTransactions { get; set; }
        public ObservableCollection<BudgetData> BudgetProgresses { get; set; }

        private ISeries[] _chartSeries = Array.Empty<ISeries>();
        public ISeries[] ChartSeries { get => _chartSeries; set { _chartSeries = value; OnPropertyChanged(); } }
        
        private ISeries[] _expenseAllocationSeries = Array.Empty<ISeries>();
        public ISeries[] ExpenseAllocationSeries { get => _expenseAllocationSeries; set { _expenseAllocationSeries = value; OnPropertyChanged(); } }
        
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public SolidColorPaint ChartLegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#6b7280")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        // --- CÁC SERVICE XỬ LÝ NGHIỆP VỤ ---
        
        private readonly IReportService _reportService;
        private readonly ITransactionService _transactionService;
        private readonly IBudgetService _budgetService;

        /// <summary>
        /// Constructor khởi tạo các Service và cấu hình ban đầu cho biểu đồ, danh sách.
        /// </summary>
        public DashboardHomeView()
        {
            InitializeComponent();
            this.DataContext = this;

            // Khởi tạo các Service (Không dùng DI container để giữ code đơn giản theo đúng yêu cầu dự án)
            _reportService = new ReportService(new TransactionRepository());
            _transactionService = new TransactionService(new TransactionRepository());
            _budgetService = new BudgetService(new BudgetRepository(), new TransactionRepository());

            TotalBalance = new DashboardSummaryData { Title = "SỐ DƯ HIỆN TẠI", TargetAmount = 0, Subtext = "Tổng", BorderAccentBrush = "#10d9a0", Icon = "💳" };
            TotalIncome  = new DashboardSummaryData { Title = "THU NHẬP THÁNG", TargetAmount = 0, Subtext = "Tháng này", BorderAccentBrush = "#3b82f6", Icon = "📈" };
            TotalExpense = new DashboardSummaryData { Title = "CHI TIÊU THÁNG", TargetAmount = 0, Subtext = "Tháng này", BorderAccentBrush = "#f43f5e", Icon = "💸" };
            NetCashFlow  = new DashboardSummaryData { Title = "DÒNG TIỀN",      TargetAmount = 0, Subtext = "Thu - Chi", BorderAccentBrush = "#f59e0b", Icon = "🎯" };

            RecentTransactions = new ObservableCollection<TransactionData>();
            BudgetProgresses = new ObservableCollection<BudgetData>();

            InitCharts();

            // Gắn sự kiện Loaded để tự động tải dữ liệu khi màn hình được hiển thị
            this.Loaded += DashboardHomeView_Loaded;
        }

        /// <summary>
        /// Hàm xử lý khi màn hình Dashboard được load lên. 
        /// Thực hiện gọi các Service để kéo dữ liệu từ DB và đẩy lên các Property Binding.
        /// </summary>
        private async void DashboardHomeView_Loaded(object sender, RoutedEventArgs e)
        {
            int userId = 1;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            try
            {
                // 1. Tải số liệu tổng quan thống kê
                var report = await _reportService.GetMonthlyReportAsync(userId, month, year);
                TotalBalance.TargetAmount = report.Balance;
                TotalIncome.TargetAmount = report.TotalIncome;
                TotalExpense.TargetAmount = report.TotalExpense;
                NetCashFlow.TargetAmount = report.Balance;

                // Kích hoạt animation chạy số tiền
                TotalBalance.AnimateTo(TotalBalance.TargetAmount);
                TotalIncome.AnimateTo(TotalIncome.TargetAmount);
                TotalExpense.AnimateTo(TotalExpense.TargetAmount);
                NetCashFlow.AnimateTo(NetCashFlow.TargetAmount);

                // 2. Tải danh sách giao dịch gần đây (tối đa 8 giao dịch mới nhất)
                var allTx = await _transactionService.GetTransactionsByMonthAsync(userId, month, year);
                var recent = allTx.OrderByDescending(t => t.TransactionDate).Take(8).ToList();
                
                RecentTransactions.Clear();
                foreach(var t in recent) {
                    RecentTransactions.Add(new TransactionData {
                        Title = t.Description ?? "Giao dịch",
                        Category = t.Category?.CategoryName ?? "Khác",
                        Amount = t.Amount,
                        // Chuyển đổi DateOnly sang DateTime để tương thích với Model WPF
                        Date = t.TransactionDate.ToDateTime(TimeOnly.MinValue),
                        IsExpense = t.TransactionType == "Expense",
                        Icon = t.TransactionType == "Expense" ? "🔥" : "💵",
                        IconBackground = t.TransactionType == "Expense" ? "#45f43f5e" : "#4510d9a0"
                    });
                }

                // 3. Tải danh sách tiến độ ngân sách (tối đa 5 ngân sách)
                var budgets = await _budgetService.GetBudgetProgressesAsync(userId, month, year);
                BudgetProgresses.Clear();
                foreach(var b in budgets.Take(5)) {
                    BudgetProgresses.Add(new BudgetData {
                        CategoryName = b.CategoryName,
                        Icon = "🏷️",
                        SpentAmount = b.SpentAmount,
                        TotalAmount = b.AmountLimit
                    });
                }
                foreach (var b in BudgetProgresses) b.AnimateProgress();

                // 4. Tải biểu đồ tròn (Pie Chart) - Phân bổ chi phí theo danh mục
                var expensesByCategory = await _reportService.GetExpenseByCategoryAsync(userId, month, year);
                var pieColors = new[] { "#f59e0b", "#7c6df8", "#3b82f6", "#f43f5e", "#10d9a0" };
                var pieList = new List<PieSeries<double>>();
                int cIdx = 0;
                foreach (var kvp in expensesByCategory.OrderByDescending(x => x.Value).Take(5))
                {
                    pieList.Add(new PieSeries<double> {
                        Values = new double[] { (double)kvp.Value },
                        Name = kvp.Key,
                        Fill = new SolidColorPaint(SKColor.Parse(pieColors[cIdx % pieColors.Length])),
                        InnerRadius = 55
                    });
                    cIdx++;
                }
                ExpenseAllocationSeries = pieList.ToArray();

                // 5. Tải biểu đồ đường (Line Chart) - Xu hướng Thu / Chi 12 tháng
                var yearlyTx = await _transactionService.GetTransactionsByYearAsync(userId, year);
                double[] incData = new double[12];
                double[] expData = new double[12];
                for (int m = 1; m <= 12; m++)
                {
                    incData[m - 1] = (double)yearlyTx.Where(t => t.TransactionDate.Month == m && t.TransactionType == "Income").Sum(t => t.Amount);
                    expData[m - 1] = (double)yearlyTx.Where(t => t.TransactionDate.Month == m && t.TransactionType == "Expense").Sum(t => t.Amount);
                }
                ChartSeries = BuildChartSeries(incData, expData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Dashboard: " + ex.Message);
            }
        }

        /// <summary>
        /// Khởi tạo khung hiển thị cho trục X và Y của biểu đồ đường.
        /// </summary>
        private void InitCharts()
        {
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
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#e5e7eb")),
                    Labeler = value => value >= 1_000_000 ? (value / 1_000_000).ToString("0.#") + "M" : (value / 1_000).ToString("0") + "K"
                }
            };
        }

        /// <summary>
        /// Tạo các đường LineSeries đồ họa đẹp mắt (đường mềm mại và hiệu ứng gradient) cho biểu đồ Thu/Chi.
        /// </summary>
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
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#ffffff")) { StrokeThickness = 2 },
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
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#ffffff")) { StrokeThickness = 2 },
                    LineSmoothness = 0.5
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
