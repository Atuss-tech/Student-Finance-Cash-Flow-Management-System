using System.Windows;
using System.Windows.Input;
using Services;
using BusinessObjects.Models;

namespace WPF.Features.Categories
{
    public partial class AddCategoryWindow : Window
    {
        private readonly ICategoryService _categoryService;

        public AddCategoryWindow()
        {
            InitializeComponent();
            _categoryService = new CategoryService();
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
            string categoryName = CategoryNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string type = ExpenseRadio.IsChecked == true ? "Expense" : "Income";

            string note = NoteTextBox.Text;

            try
            {
                int userId = 1;
                _categoryService.AddCategory(userId, categoryName, type, note);
                
                MessageBox.Show("Thêm danh mục thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm danh mục: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
