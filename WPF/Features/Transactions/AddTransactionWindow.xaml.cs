using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Services;

namespace WPF.Features.Transactions
{
    public partial class AddTransactionWindow : Window
    {
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;
        private readonly ICategoryService _categoryService;

        public AddTransactionWindow()
        {
            InitializeComponent();
            _transactionService = new TransactionService();
            _walletService = new WalletService();
            _categoryService = new CategoryService();
            
            TransactionDatePicker.SelectedDate = DateTime.Now;

            LoadDropdownData();
        }

        private void LoadDropdownData()
        {
            try
            {
                int userId = 1;
                var wallets = _walletService.GetWalletsByUserId(userId);
                WalletComboBox.ItemsSource = wallets;
                if (wallets.Any())
                {
                    WalletComboBox.SelectedIndex = 0;
                }

                FilterCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterCategories()
        {
            try
            {
                int userId = 1;
                string selectedType = ExpenseRadio.IsChecked == true ? "Expense" : "Income";
                var allCategories = _categoryService.GetCategoriesByUserId(userId);
                var filteredCategories = allCategories.Where(c => c.CategoryType == selectedType).ToList();
                CategoryComboBox.ItemsSource = filteredCategories;
                if (filteredCategories.Any())
                {
                    CategoryComboBox.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void TransactionType_Checked(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox != null)
            {
                FilterCategories();
            }
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
            string amountText = AmountTextBox.Text.Replace(".", "").Replace(",", "");
            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Số tiền không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (WalletComboBox.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn ví.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TransactionDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày giao dịch.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int walletId = (int)WalletComboBox.SelectedValue;
            int categoryId = (int)CategoryComboBox.SelectedValue;
            string type = ExpenseRadio.IsChecked == true ? "Expense" : "Income";
            DateTime date = TransactionDatePicker.SelectedDate.Value;
            string note = NoteTextBox.Text;

            try
            {
                int userId = 1;
                _transactionService.AddTransaction(userId, walletId, categoryId, type, amount, date, note);
                
                MessageBox.Show("Thêm giao dịch thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm giao dịch: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
