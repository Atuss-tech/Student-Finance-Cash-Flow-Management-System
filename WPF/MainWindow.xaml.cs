using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Features.Dashboard;
using WPF.Features.Transactions;
using WPF.Features.Budget;
using WPF.Features.Reports;
using WPF.Features.Wallets;
using WPF.Features.Categories;

namespace Student_Finance___Cash_Flow_Management_System
{
    public partial class MainWindow : Window
    {
        private string _currentContext = "Dashboard";

        public MainWindow()
        {
            InitializeComponent();
            GlobalAddButton.Content = "Thêm Giao dịch";
        }

        private void GlobalAddButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = null;
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
                    // Refresh view if needed
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
    }
}