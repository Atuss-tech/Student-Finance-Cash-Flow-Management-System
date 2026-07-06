using System.Windows;
using WPF.ViewModels;

namespace WPF.Views
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }
    }
}
