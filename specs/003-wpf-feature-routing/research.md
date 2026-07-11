# Nghiên cứu: Tổ chức lại Kiến trúc WPF theo Tính năng

## WPF Routing (Định tuyến WPF)
- **Quyết định**: Sử dụng `ContentControl` được liên kết với một `ViewModel` (cho Main Window) để lưu trữ `UserControl` hiện tại, kết hợp với các `DataTemplate` để ánh xạ ViewModels sang Views nếu sử dụng MVVM, hoặc chỉ đơn giản là thay đổi thuộc tính `Content` sang một instance của View mới.
- **Lý do**: Đây là cách tiêu chuẩn, nhẹ nhàng và dễ dàng nhất để điều hướng trong WPF mà không cần phụ thuộc vào thư viện bên thứ ba (trừ khi dự án đã dùng).
- **Các giải pháp thay thế đã xem xét**: Sử dụng `Frame` và `Page` (phù hợp hơn cho kiểu điều hướng như trình duyệt web có lịch sử back/forward, nhưng ở đây chúng ta muốn giao diện kiểu dashboard/shell).

## Quản lý Trạng thái (State Retention)
- **Quyết định**: Reload fresh (Tải lại từ đầu) khi chuyển đổi tính năng.
- **Lý do**: Dễ cài đặt, ít lỗi tiềm ẩn liên quan đến rò rỉ bộ nhớ (memory leaks). Nếu người dùng rời đi, trạng thái trước đó bị xóa bỏ, đảm bảo giao diện luôn hiển thị dữ liệu mới nhất khi quay lại.
- **Các giải pháp thay thế đã xem xét**: Retain state (giữ nguyên instance của View/ViewModel trong bộ nhớ), nhưng nó làm tăng độ phức tạp và tốn bộ nhớ.
