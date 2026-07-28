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

            int userId = _editingWallet.UserId > 0 ? _editingWallet.UserId : 1;

            // Kiểm tra xem ví đã có giao dịch chưa để cảnh báo trước
            bool hasTransactions = false;
            try
            {
                hasTransactions = new Repositories.WalletRepository()
                    .HasTransactions(_editingWallet.WalletId, userId);
            }
            catch { /* Bỏ qua lỗi check, tiếp tục xử lý */ }

            if (hasTransactions)
            {
                // Ví đã có giao dịch → ẩn ví khỏi danh sách ví, giữ nguyên lịch sử giao dịch
                bool isConfirmed = Common.CustomMessageBoxWindow.ShowConfirm(
                    this,
                    "Xác nhận xóa ví",
                    $"Bạn có chắc chắn muốn xóa ví \"{_editingWallet.WalletName}\" khỏi danh sách ví không?",
                    "Ví sẽ được xóa khỏi danh sách hiển thị Ví, nhưng toàn bộ lịch sử giao dịch cũ vẫn được bảo toàn trong mục Giao dịch.",
                    "Xóa ví",
                    "Hủy bỏ",
                    Common.CustomDialogType.Warning,
                    "#E11D48");

                if (isConfirmed)
                {
                    try
                    {
                        _walletService.RemoveOrDeactivateWallet(userId, _editingWallet.WalletId);
                        Common.CustomMessageBoxWindow.ShowInfo(
                            this,
                            "Thành công",
                            $"Ví \"{_editingWallet.WalletName}\" đã được xóa thành công khỏi danh sách ví.");
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi khi xóa ví", ex.Message);
                    }
                }
            }
            else
            {
                // Ví chưa có giao dịch → cho phép xóa hẳn
                bool isConfirmed = Common.CustomMessageBoxWindow.ShowConfirm(
                    this,
                    "Xác nhận xóa ví",
                    $"Bạn có chắc chắn muốn xóa vĩnh viễn ví \"{_editingWallet.WalletName}\" không?",
                    "Hành động này sẽ xóa hoàn toàn ví khỏi hệ thống và không thể hoàn tác.",
                    "Xóa vĩnh viễn",
                    "Hủy bỏ",
                    Common.CustomDialogType.Warning,
                    "#DC2626");

                if (isConfirmed)
                {
                    try
                    {
                        _walletService.RemoveOrDeactivateWallet(userId, _editingWallet.WalletId);
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi khi xóa ví", ex.Message);
                    }
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

            if (walletName.Length > 250)
            {
                MessageBox.Show("Tên ví không được vượt quá 250 ký tự.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string type = (WalletTypeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Cash";
            string note = NoteTextBox.Text;

            if (!string.IsNullOrEmpty(note) && note.Length > 250)
            {
                MessageBox.Show("Ghi chú thêm không được vượt quá 250 ký tự.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
