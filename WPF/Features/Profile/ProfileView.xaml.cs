using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF.Features.Profile
{
    public partial class ProfileView : UserControl, INotifyPropertyChanged
    {
        public string FullName { get; set; } = "Nguyễn Thành";
        public string AvatarInitials { get; set; } = "NT";
        public string Email { get; set; } = "thanh.nguyen@example.com";
        public string Phone { get; set; } = "0901234567";
        public string SubscriptionPlan { get; set; } = "Premium";
        public string JoinDate { get; set; } = "Thành viên từ Tháng 1, 2026";
        public string TotalDataSize { get; set; } = "1.2 MB / 5.0 GB";

        private string _oldPassword = string.Empty;
        public string OldPassword
        {
            get => _oldPassword;
            set { _oldPassword = value; OnPropertyChanged(); }
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        private string _alertMessage = string.Empty;
        public string AlertMessage
        {
            get => _alertMessage;
            set { _alertMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasAlert)); }
        }

        private bool _isAlertError;
        public bool IsAlertError
        {
            get => _isAlertError;
            set { _isAlertError = value; OnPropertyChanged(); }
        }

        public bool HasAlert => !string.IsNullOrEmpty(AlertMessage);

        public ProfileView()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            AlertMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                IsAlertError = true;
                AlertMessage = "Vui lòng nhập đầy đủ thông tin mật khẩu.";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                IsAlertError = true;
                AlertMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return;
            }

            if (NewPassword.Length < 6)
            {
                IsAlertError = true;
                AlertMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                return;
            }

            // Simulate API call/processing
            await Task.Delay(800);

            // Success
            IsAlertError = false;
            AlertMessage = "Đổi mật khẩu thành công!";
            
            // Clear fields
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OldPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
                OldPassword = pb.Password;
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
                NewPassword = pb.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
                ConfirmPassword = pb.Password;
        }
    }
}

