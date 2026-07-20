using System.Windows;
using System.Windows.Input;

namespace WPF.Features.Budget
{
    public partial class AddBudgetWindow : Window
    {
        public AddBudgetWindow()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void LoadCategories()
        {
            var categoryService = new Services.CategoryService();
            var expenses = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(categoryService.GetCategoriesByUserId(1), c => c.CategoryType == "Expense"));
            CategoryComboBox.ItemsSource = expenses;
            if (expenses.Count > 0) CategoryComboBox.SelectedIndex = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string amountText = AmountTextBox.Text.Replace(".", "").Replace(",", "");
            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền giới hạn hợp lệ (lớn hơn 0)!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục chi tiêu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int categoryId = (int)CategoryComboBox.SelectedValue;
            
            // Extract month and year from MonthComboBox
            int month = System.DateTime.Now.Month;
            int year = System.DateTime.Now.Year;
            if (MonthComboBox.SelectedIndex == 0) { month = 7; year = 2026; }
            if (MonthComboBox.SelectedIndex == 1) { month = 8; year = 2026; }
            if (MonthComboBox.SelectedIndex == 2) { month = 9; year = 2026; }

            var budget = new BusinessObjects.Models.Budget
            {
                UserId = 1,
                CategoryId = categoryId,
                Month = month,
                Year = year,
                AmountLimit = amount,
                Note = NoteTextBox.Text,
                CreatedAt = System.DateTime.Now
            };

            var budgetService = new Services.BudgetService(new Repositories.BudgetRepository(), new Repositories.TransactionRepository());
            try
            {
                await budgetService.AddBudgetAsync(budget);
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
