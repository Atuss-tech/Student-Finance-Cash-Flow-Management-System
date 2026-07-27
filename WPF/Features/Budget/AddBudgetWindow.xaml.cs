using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WPF.Features.Budget
{
    public class MonthOption
    {
        public string DisplayText { get; set; } = string.Empty;
        public string MonthYearKey { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public partial class AddBudgetWindow : Window
    {
        private int _editBudgetId = 0;
        private UIData.BudgetData? _editBudget = null;
        private List<MonthOption> _months = new();

        public AddBudgetWindow(UIData.BudgetData? editBudget = null)
        {
            InitializeComponent();
            _editBudget = editBudget;

            LoadMonths();
            LoadCategories();

            if (editBudget != null)
            {
                _editBudgetId = editBudget.BudgetId;
                TitleBlock.Text = "Cập nhật Ngân sách";
                AmountTextBox.Text = editBudget.TotalAmount.ToString("0");
                NoteTextBox.Text = editBudget.Note ?? string.Empty;
                CategoryComboBox.SelectedValue = editBudget.CategoryId;
                
                string targetKey = $"{editBudget.Month}_{editBudget.Year}";
                if (!_months.Any(m => m.MonthYearKey == targetKey))
                {
                    _months.Insert(0, new MonthOption
                    {
                        DisplayText = $"Tháng {editBudget.Month}, {editBudget.Year}",
                        MonthYearKey = targetKey,
                        Month = editBudget.Month,
                        Year = editBudget.Year
                    });
                    MonthComboBox.ItemsSource = null;
                    MonthComboBox.ItemsSource = _months;
                }
                MonthComboBox.SelectedValue = targetKey;

                CategoryComboBox.IsEnabled = false;
                MonthComboBox.IsEnabled = false;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void LoadMonths()
        {
            _months = new List<MonthOption>();
            var now = DateTime.Now;

            // Nạp tháng hiện tại và 11 tháng tiếp theo
            for (int i = 0; i < 12; i++)
            {
                var dt = now.AddMonths(i);
                _months.Add(new MonthOption
                {
                    DisplayText = $"Tháng {dt.Month}, {dt.Year}",
                    MonthYearKey = $"{dt.Month}_{dt.Year}",
                    Month = dt.Month,
                    Year = dt.Year
                });
            }

            MonthComboBox.ItemsSource = _months;
            if (_months.Count > 0) MonthComboBox.SelectedIndex = 0;
        }

        private void LoadCategories()
        {
            var categoryService = new Services.CategoryService();
            var expenses = Enumerable.ToList(Enumerable.Where(categoryService.GetCategoriesByUserId(1), c => c.CategoryType == "Expense"));
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

            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            if (MonthComboBox.SelectedItem is MonthOption selectedMonthOption)
            {
                month = selectedMonthOption.Month;
                year = selectedMonthOption.Year;
            }
            else if (_editBudget != null)
            {
                month = _editBudget.Month;
                year = _editBudget.Year;
            }

            var budget = new BusinessObjects.Models.Budget
            {
                UserId = 1,
                CategoryId = categoryId,
                Month = month,
                Year = year,
                AmountLimit = amount,
                Note = NoteTextBox.Text?.Trim(),
                CreatedAt = DateTime.Now
            };

            var budgetService = new Services.BudgetService(new Repositories.BudgetRepository(), new Repositories.TransactionRepository());
            try
            {
                if (_editBudget != null)
                {
                    budget.BudgetId = _editBudgetId;
                    await budgetService.UpdateBudgetAsync(budget);
                }
                else
                {
                    await budgetService.AddBudgetAsync(budget);
                }
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
