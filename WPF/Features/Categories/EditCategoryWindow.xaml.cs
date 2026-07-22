using System.Windows;
using System.Windows.Input;
using Services;

namespace WPF.Features.Categories
{
    public partial class EditCategoryWindow : Window
    {
        private readonly ICategoryService _categoryService;
        private readonly int _categoryId;
        private readonly int _userId;

        public EditCategoryWindow(int categoryId, int userId, string name, string type, string description)
        {
            InitializeComponent();
            _categoryService = new CategoryService();
            _categoryId      = categoryId;
            _userId          = userId;

            // Pre-fill dữ liệu hiện tại
            CategoryNameTextBox.Text = name;
            NoteTextBox.Text         = description;
            SubtitleBlock.Text       = $"Đang sửa: {name}";

            if (type == "Income" || type == "Thu nhập")
                IncomeRadio.IsChecked = true;
            else
                ExpenseRadio.IsChecked = true;
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa danh mục \"{CategoryNameTextBox.Text.Trim()}\" không?\nĐiều này không thể hoàn tác.",
                "Xác nhận xóa danh mục",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _categoryService.DeleteCategory(_userId, _categoryId);
                    this.DialogResult = true;
                    this.Close();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa danh mục: " + ex.Message, "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = CategoryNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục.", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newType = IncomeRadio.IsChecked == true ? "Income" : "Expense";
            string newNote = NoteTextBox.Text.Trim();

            try
            {
                _categoryService.UpdateCategory(_userId, _categoryId, newName, newType, newNote);
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật danh mục: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
