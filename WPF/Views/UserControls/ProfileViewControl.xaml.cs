using System.Windows;
using System.Windows.Controls;
using WPF.ViewModels;

namespace WPF.Views.UserControls
{
    public partial class ProfileViewControl : UserControl
    {
        public ProfileViewControl()
        {
            InitializeComponent();
        }

        private void OldPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm && sender is PasswordBox pb)
                vm.OldPassword = pb.Password;
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm && sender is PasswordBox pb)
                vm.NewPassword = pb.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm && sender is PasswordBox pb)
                vm.ConfirmPassword = pb.Password;
        }
    }
}
