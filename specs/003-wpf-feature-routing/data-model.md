# Mô hình Dữ liệu: Kiến trúc WPF theo Tính năng

Không có thực thể dữ liệu cơ sở dữ liệu mới nào được tạo ra trong tính năng này. Đây hoàn toàn là tái cấu trúc giao diện người dùng (UI refactoring).

Tuy nhiên, có một số cấu trúc UI và ViewModel cần thiết để định tuyến:

- **NavigationItem** (hoặc tương tự, nếu sử dụng menu điều hướng MVVM):
  - `Title` (string): Tên tính năng (ví dụ: "Ngân sách").
  - `Icon` (string/image): Biểu tượng cho tính năng.
  - `ViewType` (Type) hoặc `ViewModelType` (Type): Loại giao diện cần tải khi được chọn.

- **MainWindowViewModel** (hoặc Code-behind của MainWindow):
  - `CurrentView` (object): Thuộc tính lưu trữ View hoặc ViewModel hiện tại đang được hiển thị trong vùng nội dung (Content area) của MainWindow.
