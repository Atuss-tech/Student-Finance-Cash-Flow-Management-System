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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetView(UserControl view, string title)
        {
            if (MainContentControl != null)
            {
                MainContentControl.Content = view;
            }
            if (PageTitle != null)
            {
                PageTitle.Text = title;
            }
        }

        private void Nav_Dashboard_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new DashboardHomeView(), "Tổng quan tài chính");
        }

        private void Nav_Transactions_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new TransactionsView(), "Quản lý giao dịch");
        }

        private void Nav_Budgets_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new BudgetsView(), "Ngân sách & Mục tiêu");
        }

        private void Nav_Reports_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new ReportsView(), "Báo cáo chi tiêu");
        }

        private void Nav_Cards_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new UserControl(), "Thẻ & tài khoản (Đang cập nhật)");
        }

        private void Nav_Wallets_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new WalletsView(), "Ví điện tử");
        }

        private void Nav_Categories_Checked(object sender, RoutedEventArgs e)
        {
            SetView(new CategoriesView(), "Quản lý danh mục");
        }
    }
}