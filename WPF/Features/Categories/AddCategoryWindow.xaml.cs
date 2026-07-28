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
                Common.CustomMessageBoxWindow.ShowInfo(this, "Cảnh báo", "Vui lòng nhập tên danh mục.");
                return;
            }

            if (categoryName.Length > 250)
            {
                Common.CustomMessageBoxWindow.ShowInfo(this, "Cảnh báo", "Tên danh mục không được vượt quá 250 ký tự.");
                return;
            }

            string type = ExpenseRadio.IsChecked == true ? "Expense" : "Income";

            string note = NoteTextBox.Text;
            if (!string.IsNullOrEmpty(note) && note.Length > 250)
            {
                Common.CustomMessageBoxWindow.ShowInfo(this, "Cảnh báo", "Mô tả / Ghi chú không được vượt quá 250 ký tự.");
                return;
            }

            try
            {
                int userId = Services.UserSession.CurrentUserId;
                _categoryService.AddCategory(userId, categoryName, type, note);
                
                Common.CustomMessageBoxWindow.ShowInfo(this, "Thành công", "Thêm danh mục mới thành công!");
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi thêm danh mục", ex.Message);
            }
        }
    }
}
