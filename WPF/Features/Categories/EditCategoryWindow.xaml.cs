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
            string catName = CategoryNameTextBox.Text.Trim();

            bool hasRelated = false;
            try
            {
                hasRelated = _categoryService.HasRelatedData(_userId, _categoryId);
            }
            catch { /* Bỏ qua lỗi check, tiếp tục xử lý */ }

            if (hasRelated)
            {
                // Danh mục đã phát sinh giao dịch/ngân sách → ẩn khỏi danh sách, bảo toàn lịch sử giao dịch
                bool isConfirmed = Common.CustomMessageBoxWindow.ShowConfirm(
                    this,
                    "Xác nhận xóa danh mục",
                    $"Bạn có chắc chắn muốn xóa danh mục \"{catName}\" khỏi danh sách danh mục không?",
                    "Danh mục sẽ được xóa khỏi danh sách hiển thị, nhưng toàn bộ lịch sử giao dịch cũ vẫn được bảo toàn trong mục Giao dịch.",
                    "Xóa danh mục",
                    "Hủy bỏ",
                    Common.CustomDialogType.Warning,
                    "#E11D48");

                if (isConfirmed)
                {
                    try
                    {
                        _categoryService.DeleteCategory(_userId, _categoryId);
                        Common.CustomMessageBoxWindow.ShowInfo(
                            this,
                            "Thành công",
                            $"Danh mục \"{catName}\" đã được xóa thành công khỏi danh sách danh mục.");
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi khi xóa danh mục", ex.Message);
                    }
                }
            }
            else
            {
                // Danh mục chưa phát sinh giao dịch → cho phép xóa hẳn
                bool isConfirmed = Common.CustomMessageBoxWindow.ShowConfirm(
                    this,
                    "Xác nhận xóa danh mục",
                    $"Bạn có chắc chắn muốn xóa vĩnh viễn danh mục \"{catName}\" không?",
                    "Hành động này sẽ xóa hoàn toàn danh mục khỏi hệ thống và không thể hoàn tác.",
                    "Xóa vĩnh viễn",
                    "Hủy bỏ",
                    Common.CustomDialogType.Warning,
                    "#DC2626");

                if (isConfirmed)
                {
                    try
                    {
                        _categoryService.DeleteCategory(_userId, _categoryId);
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi khi xóa danh mục", ex.Message);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = CategoryNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                Common.CustomMessageBoxWindow.ShowInfo(this, "Cảnh báo", "Vui lòng nhập tên danh mục.");
                return;
            }

            if (newName.Length > 250)
            {
                Common.CustomMessageBoxWindow.ShowInfo(this, "Cảnh báo", "Tên danh mục không được vượt quá 250 ký tự.");
                return;
            }

            string newType = IncomeRadio.IsChecked == true ? "Income" : "Expense";
            string newNote = NoteTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(newNote) && newNote.Length > 250)
            {
                Common.CustomMessageBoxWindow.ShowInfo(this, "Cảnh báo", "Mô tả / Ghi chú không được vượt quá 250 ký tự.");
                return;
            }

            try
            {
                _categoryService.UpdateCategory(_userId, _categoryId, newName, newType, newNote);
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi cập nhật", ex.Message);
            }
        }
    }
}
