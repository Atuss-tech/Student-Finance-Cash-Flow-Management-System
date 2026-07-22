using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Services;
using WPF.UIData;

namespace WPF.Features.Transactions
{
    public partial class AddTransactionWindow : Window
    {
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;
        private readonly ICategoryService _categoryService;
        private readonly TransactionData? _transactionToEdit;

        public AddTransactionWindow()
        {
            InitializeComponent();
            _transactionService = new TransactionService();
            _walletService = new WalletService();
            _categoryService = new CategoryService();
            
            TransactionDatePicker.SelectedDate = DateTime.Now;

            LoadDropdownData();
        }

        public AddTransactionWindow(TransactionData transactionToEdit)
        {
            InitializeComponent();
            _transactionService = new TransactionService();
            _walletService = new WalletService();
            _categoryService = new CategoryService();
            _transactionToEdit = transactionToEdit;

            LoadDropdownData();
            PopulateEditData();
        }

        private void PopulateEditData()
        {
            if (_transactionToEdit == null) return;

            TitleTextBlock.Text = "Sửa Giao dịch";
            SaveButton.Content = "Cập nhật Giao dịch";

            AmountTextBox.Text = _transactionToEdit.Amount.ToString("0");
            ExpenseRadio.IsChecked = _transactionToEdit.IsExpense;
            IncomeRadio.IsChecked = !_transactionToEdit.IsExpense;
            
            FilterCategories();

            if (_transactionToEdit.WalletId > 0)
            {
                WalletComboBox.SelectedValue = _transactionToEdit.WalletId;
            }
            if (_transactionToEdit.CategoryId > 0)
            {
                CategoryComboBox.SelectedValue = _transactionToEdit.CategoryId;
            }
            else if (!string.IsNullOrEmpty(_transactionToEdit.Category))
            {
                var catList = CategoryComboBox.ItemsSource as System.Collections.IEnumerable;
                if (catList != null)
                {
                    foreach (BusinessObjects.Models.Category c in catList)
                    {
                        if (c.CategoryName == _transactionToEdit.Category)
                        {
                            CategoryComboBox.SelectedItem = c;
                            break;
                        }
                    }
                }
            }

            TransactionDatePicker.SelectedDate = _transactionToEdit.Date;
            NoteTextBox.Text = string.IsNullOrEmpty(_transactionToEdit.Note) ? _transactionToEdit.Title : _transactionToEdit.Note;
        }

        private void LoadDropdownData()
        {
            try
            {
                int userId = 1;
                var wallets = _walletService.GetAllWalletsByUser(userId);
                var selectableWallets = wallets.Where(w => w.IsActive || (_transactionToEdit != null && w.WalletId == _transactionToEdit.WalletId)).ToList();
                WalletComboBox.ItemsSource = selectableWallets;
                if (selectableWallets.Any())
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
                if (_transactionToEdit != null)
                {
                    _transactionService.UpdateTransaction(userId, _transactionToEdit.TransactionId, walletId, categoryId, type, amount, date, note);
                    MessageBox.Show("Cập nhật giao dịch thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _transactionService.AddTransaction(userId, walletId, categoryId, type, amount, date, note);
                    MessageBox.Show("Thêm giao dịch thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu giao dịch: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
