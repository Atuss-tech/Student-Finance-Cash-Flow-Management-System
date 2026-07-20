using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Services;

namespace WPF.Features.Wallets
{
    public partial class AddWalletWindow : Window
    {
        private readonly IWalletService _walletService;

        public AddWalletWindow()
        {
            InitializeComponent();
            _walletService = new WalletService();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string walletName = WalletNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(walletName))
            {
                MessageBox.Show("Vui lòng nhập tên ví.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string amountText = BalanceTextBox.Text.Replace(".", "").Replace(",", "");
            if (!decimal.TryParse(amountText, out decimal balance))
            {
                MessageBox.Show("Số dư ban đầu không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string type = (WalletTypeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Cash";
            string note = NoteTextBox.Text;

            try
            {
                int userId = 1;
                _walletService.AddWallet(userId, walletName, type, balance, note);
                MessageBox.Show("Thêm ví thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm ví: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
