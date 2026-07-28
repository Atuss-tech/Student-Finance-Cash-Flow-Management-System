using System.Windows;
using System.Windows.Controls;
using WPF.Features.Dashboard;
using WPF.Features.Transactions;
using WPF.Features.Budget;
using WPF.Features.Reports;
using WPF.Features.Wallets;
using WPF.Features.Categories;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Student_Finance___Cash_Flow_Management_System
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _currentContext = "Dashboard";

        private string _userName = "Người Dùng";
        public string UserName 
        { 
            get => _userName; 
            set { _userName = value; OnPropertyChanged(); } 
        }

        private string _userInitials = "U";
        public string UserInitials 
        { 
            get => _userInitials; 
            set { _userInitials = value; OnPropertyChanged(); } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GlobalAddButton.Content = "Thêm Giao dịch";
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Tạm thời fix cứng ở ViewModel cho đến khi có Login Session thật sự
            UserName = "Nguyễn Thành";
            UserInitials = "NT";
        }

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "U";
            var parts = fullName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0][0].ToString().ToUpper();
            return (parts[0][0].ToString() + parts[^1][0].ToString()).ToUpper();
        }

        private async void GlobalAddButton_Click(object sender, RoutedEventArgs e)
        {
            Window? window = null;
            switch (_currentContext)
            {
                case "Wallets":
                    window = new AddWalletWindow();
                    break;
                case "Budgets":
                    window = new AddBudgetWindow();
                    break;
                case "Categories":
                    window = new AddCategoryWindow();
                    break;
                case "Transactions":
                default:
                    window = new AddTransactionWindow();
                    break;
            }

            if (window != null)
            {
                window.Owner = this;
                if (window.ShowDialog() == true)
                {
                    // Auto-refresh view sau khi thêm thành công.
                    if (MainContentControl?.Content is WPF.Features.Transactions.TransactionsView txView)
                    {
                        await txView.LoadDataAsync();
                    }
                    else if (MainContentControl?.Content is WPF.Features.Dashboard.DashboardHomeView dashView)
                    {
                        await dashView.LoadDashboardDataAsync();
                    }
                    else if (MainContentControl?.Content is WPF.Features.Reports.ReportsView repView)
                    {
                        await repView.GenerateReportsAsync();
                    }
                    else if (MainContentControl?.Content is WPF.Features.Categories.CategoriesView catView)
                    {
                        await catView.LoadDataAsync();
                    }
                    else if (MainContentControl?.Content is WPF.Features.Wallets.WalletsView walletView)
                    {
                        await walletView.LoadWalletsAsync();
                    }
                    else if (MainContentControl?.Content is WPF.Features.Budget.BudgetsView budgetView)
                    {
                        await budgetView.LoadBudgetDataAsync();
                    }
                }
            }
        }

        private void SetView(UserControl view, string title, string context, string buttonText)
        {
            if (MainContentControl != null)
            {
                MainContentControl.Content = view;
            }
            if (PageTitle != null)
            {
                PageTitle.Text = title;
            }
            _currentContext = context;
            if (GlobalAddButton != null)
            {
                GlobalAddButton.Content = buttonText;
                GlobalAddButton.Visibility = (context == "Dashboard" || context == "Reports") ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void Nav_Dashboard_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new DashboardHomeView(), "Tổng quan tài chính", "Dashboard", "");
        }

        private void Nav_Transactions_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new TransactionsView(), "Quản lý giao dịch", "Transactions", "Thêm Giao dịch");
        }

        private void Nav_Budgets_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new BudgetsView(), "Ngân sách & Mục tiêu", "Budgets", "Thêm Ngân sách");
        }

        private void Nav_Reports_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new ReportsView(), "Báo cáo chi tiêu", "Reports", "");
        }

        private void Nav_Wallets_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new WalletsView(), "Ví điện tử", "Wallets", "Thêm Ví");
        }

        private void Nav_Categories_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new CategoriesView(), "Quản lý danh mục", "Categories", "Thêm Danh mục");
        }

        public void NavigateToBudgets()
        {
            if (NavBudgetsBtn != null)
            {
                NavBudgetsBtn.IsChecked = true;
            }
        }
    }
}