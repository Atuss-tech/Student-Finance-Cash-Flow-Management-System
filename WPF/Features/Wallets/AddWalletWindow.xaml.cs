using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Services;

namespace WPF.Features.Wallets
{
    public partial class AddWalletWindow : Window
    {
        private readonly IWalletService _walletService;
        private readonly WPF.UIData.WalletData? _editingWallet;

        public AddWalletWindow()
        {
            InitializeComponent();
            _walletService = new WalletService();
        }

        public AddWalletWindow(WPF.UIData.WalletData walletToEdit) : this()
        {
            _editingWallet = walletToEdit;
            TitleTextBlock.Text = "Sửa Ví";
            SubtitleBlock.Text = $"Đang sửa: {walletToEdit.WalletName}";
            SubtitleBlock.Visibility = Visibility.Visible;
            SaveButton.Content = "Lưu thay đổi";
            DeleteButton.Visibility = Visibility.Visible;

            WalletNameTextBox.Text = walletToEdit.WalletName;
            BalanceTextBox.Text = walletToEdit.Balance.ToString("#,##0");
            BalanceTextBox.IsEnabled = false;
            NoteTextBox.Text = walletToEdit.Note;

            foreach (ComboBoxItem item in WalletTypeComboBox.Items)
            {
                if (string.Equals(item.Tag?.ToString(), walletToEdit.WalletType, StringComparison.OrdinalIgnoreCase))
                {
                    WalletTypeComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_editingWallet == null) return;

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa ví \"{_editingWallet.WalletName}\" không?\nĐiều này không thể hoàn tác.",
                "Xác nhận xóa ví",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int userId = _editingWallet.UserId > 0 ? _editingWallet.UserId : 1;
                    _walletService.RemoveOrDeactivateWallet(userId, _editingWallet.WalletId);
                    this.DialogResult = true;
                    this.Close();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa ví: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string walletName = WalletNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(walletName))
            {
                MessageBox.Show("Vui lòng nhập tên ví.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string type = (WalletTypeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Cash";
            string note = NoteTextBox.Text;
            int userId = _editingWallet != null && _editingWallet.UserId > 0 ? _editingWallet.UserId : 1;

            try
            {
                if (_editingWallet != null)
                {
                    _walletService.UpdateWalletInfo(userId, _editingWallet.WalletId, walletName, type, note);
                }
                else
                {
                    string amountText = BalanceTextBox.Text.Replace(".", "").Replace(",", "").Trim();
                    if (string.IsNullOrWhiteSpace(amountText))
                    {
                        amountText = "0";
                    }
                    if (!decimal.TryParse(amountText, out decimal balance))
                    {
                        MessageBox.Show("Số dư ban đầu không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    _walletService.CreateNewWallet(userId, walletName, type, balance, note);
                }
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu ví: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
