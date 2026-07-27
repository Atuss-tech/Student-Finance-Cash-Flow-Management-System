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
                // Danh mục đã phát sinh giao dịch/ngân sách → không cho xóa hẳn, chỉ cho ẨN
                bool isConfirmed = Common.CustomMessageBoxWindow.ShowConfirm(
                    this,
                    "Không thể xóa danh mục này",
                    $"Danh mục \"{catName}\" đã có giao dịch hoặc ngân sách liên kết trong hệ thống nên không thể xóa hoàn toàn.",
                    "Danh mục sẽ được ẨN: Không xuất hiện khi chọn danh mục mới nhưng vẫn bảo toàn báo cáo và lịch sử giao dịch cũ.",
                    "Ẩn danh mục ngay",
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
                            $"Danh mục \"{catName}\" đã được chuyển sang trạng thái ĐÃ ẨN.");
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi khi ẩn danh mục", ex.Message);
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
                Common.CustomMessageBoxWindow.ShowInfo(this, "Lỗi cập nhật", ex.Message);
            }
        }
    }
}
