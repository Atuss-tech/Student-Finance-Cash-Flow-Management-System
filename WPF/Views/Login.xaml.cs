using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Services;

namespace Student_Finance___Cash_Flow_Management_System
{
    // Lớp giao diện xử lý Đăng nhập và Đăng ký (Login.xaml)
    public partial class Login : Window
    {
        private readonly IUserService userService;
        private bool isLoginPasswordVisible = false;
        private bool isRegisterPasswordVisible = false;
        private bool _isSyncingLoginPassword = false;
        private bool _isSyncingRegisterPassword = false;

        // Hàm khởi tạo window và gọi service
        public Login()
        {
            InitializeComponent();
            userService = new UserService();
            // Khởi tạo các event sync
            txtLoginPassword.PasswordChanged += txtLoginPassword_PasswordChanged;
            txtLoginPasswordVisible.TextChanged += txtLoginPasswordVisible_TextChanged;
            txtRegisterPassword.PasswordChanged += txtRegisterPassword_PasswordChanged;
            txtRegisterPasswordVisible.TextChanged += txtRegisterPasswordVisible_TextChanged;
        }

        // Sự kiện khi người dùng bấm nút Đăng Nhập
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtLoginEmail.Text.Trim();
            string password = txtLoginPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Email và Mật khẩu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = userService.AuthenticateUser(email, password);
            if (user != null)
            {
                MessageBox.Show($"Đăng nhập thành công! Chào mừng {user.FullName}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                // TODO: Open MainWindow and close this
                // MainWindow main = new MainWindow();
                // main.Show();
                // this.Close();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện khi người dùng bấm nút Đăng Ký
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtRegisterFullName.Text.Trim();
            string email = txtRegisterEmail.Text.Trim();
            string password = txtRegisterPassword.Password;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra độ phức tạp của mật khẩu
            if (password.Length > 10)
            {
                MessageBox.Show("Mật khẩu chỉ được giới hạn tối đa 10 ký tự.", "Lỗi mật khẩu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!password.Any(char.IsUpper) || !password.Any(char.IsDigit) || !password.Any(c => !char.IsLetterOrDigit(c)))
            {
                MessageBox.Show("Mật khẩu phải chứa ít nhất 1 chữ hoa, 1 số và 1 ký tự đặc biệt.", "Lỗi mật khẩu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool success = userService.RegisterUser(fullName, email, password);
            if (success)
            {
                MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                // Clear fields
                txtRegisterFullName.Clear();
                txtRegisterEmail.Clear();
                txtRegisterPassword.Clear();
            }
            else
            {
                MessageBox.Show("Email này đã được sử dụng. Vui lòng chọn email khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý kéo thả Window (do WindowStyle="None" nên phải code phần kéo thả)
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Xử lý khi nhấn nút tắt ứng dụng (dấu X)
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Chuyển sang Tab Đăng Ký
        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = TabRegister;
        }

        // Chuyển về Tab Đăng Nhập
        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = TabLogin;
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // --- Logic cho tính năng Mắt xem mật khẩu ---

        private void btnToggleLoginPassword_Click(object sender, RoutedEventArgs e)
        {
            isLoginPasswordVisible = !isLoginPasswordVisible;
            if (isLoginPasswordVisible)
            {
                txtLoginPasswordVisible.Visibility = Visibility.Visible;
                txtLoginPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtLoginPasswordVisible.Visibility = Visibility.Collapsed;
                txtLoginPassword.Visibility = Visibility.Visible;
            }

            var sb = new System.Windows.Media.Animation.Storyboard();
            var slashAnim = new System.Windows.Media.Animation.DoubleAnimation()
            {
                To = isLoginPasswordVisible ? 25 : 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            var blinkAnim = new System.Windows.Media.Animation.DoubleAnimation()
            {
                From = 0, To = 1, Duration = TimeSpan.FromSeconds(0.2)
            };
            System.Windows.Media.Animation.Storyboard.SetTargetName(slashAnim, "LoginEyeSlash");
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(slashAnim, new PropertyPath("StrokeDashOffset"));
            
            System.Windows.Media.Animation.Storyboard.SetTargetName(blinkAnim, "LoginEyeScale");
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(blinkAnim, new PropertyPath("ScaleY"));

            sb.Children.Add(slashAnim);
            sb.Children.Add(blinkAnim);
            sb.Begin(this);
        }

        private void btnToggleRegisterPassword_Click(object sender, RoutedEventArgs e)
        {
            isRegisterPasswordVisible = !isRegisterPasswordVisible;
            if (isRegisterPasswordVisible)
            {
                txtRegisterPasswordVisible.Visibility = Visibility.Visible;
                txtRegisterPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtRegisterPasswordVisible.Visibility = Visibility.Collapsed;
                txtRegisterPassword.Visibility = Visibility.Visible;
            }

            var sb = new System.Windows.Media.Animation.Storyboard();
            var slashAnim = new System.Windows.Media.Animation.DoubleAnimation()
            {
                To = isRegisterPasswordVisible ? 25 : 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            var blinkAnim = new System.Windows.Media.Animation.DoubleAnimation()
            {
                From = 0, To = 1, Duration = TimeSpan.FromSeconds(0.2)
            };
            System.Windows.Media.Animation.Storyboard.SetTargetName(slashAnim, "RegisterEyeSlash");
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(slashAnim, new PropertyPath("StrokeDashOffset"));

            System.Windows.Media.Animation.Storyboard.SetTargetName(blinkAnim, "RegisterEyeScale");
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(blinkAnim, new PropertyPath("ScaleY"));

            sb.Children.Add(slashAnim);
            sb.Children.Add(blinkAnim);
            sb.Begin(this);
        }

        private void txtLoginPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isSyncingLoginPassword) return;
            _isSyncingLoginPassword = true;
            txtLoginPasswordVisible.Text = txtLoginPassword.Password;
            _isSyncingLoginPassword = false;
        }

        private void txtLoginPasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isSyncingLoginPassword) return;
            _isSyncingLoginPassword = true;
            txtLoginPassword.Password = txtLoginPasswordVisible.Text;
            _isSyncingLoginPassword = false;
        }

        private void txtRegisterPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isSyncingRegisterPassword) return;
            _isSyncingRegisterPassword = true;
            txtRegisterPasswordVisible.Text = txtRegisterPassword.Password;
            _isSyncingRegisterPassword = false;
        }

        private void txtRegisterPasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isSyncingRegisterPassword) return;
            _isSyncingRegisterPassword = true;
            txtRegisterPassword.Password = txtRegisterPasswordVisible.Text;
            _isSyncingRegisterPassword = false;
        }
    }
}
