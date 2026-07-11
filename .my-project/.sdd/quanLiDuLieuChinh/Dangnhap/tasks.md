# Tasks: Nút xem Mật khẩu (Eye Icon) có Hiệu ứng động

## Phase 1: Cập nhật XAML UI (User Story 1)
- **Goal:** Thêm các thẻ `TextBox` và nút `Button` chứa `Path` vẽ hình con mắt vào màn hình Đăng nhập và Đăng ký.
- **Independent Test Criteria:** Mở giao diện không bị vỡ layout, hiển thị đúng biểu tượng con mắt nằm góc phải ô nhập mật khẩu.

- [x] T001 [US1] Tại **Tab Đăng nhập**, bọc `PasswordBox` hiện tại vào `Grid`. Thêm `TextBox x:Name="txtLoginPasswordVisible"` (ẩn mặc định) và nút bấm `btnToggleLoginPassword` với icon mắt.
- [x] T002 [US1] Tại **Tab Đăng ký**, bọc `PasswordBox` (Mật khẩu) vào `Grid`. Thêm `TextBox x:Name="txtRegisterPasswordVisible"` (ẩn) và nút bấm `btnToggleRegisterPassword` với icon mắt.
- [x] T003 [US1] (Tùy chọn) Bọc cả ô Xác nhận mật khẩu bên Đăng ký tương tự T002. *(Không áp dụng vì thiết kế cũ không có ô Xác nhận)*
- [x] T004 [US1] Bổ sung các cấu trúc XAML để vẽ hình Con mắt (Eye) và đường chéo (Slash), cấu hình sẵn các `Storyboard` chuẩn bị cho hiệu ứng.

## Phase 2: Code-Behind & Xử lý Logic (User Story 2)
- **Goal:** Viết các hàm C# xử lý sự kiện khi bấm nút con mắt (chuyển đổi UI và chạy Animation), cùng logic đồng bộ dữ liệu.
- **Independent Test Criteria:** Người dùng có thể bấm nút mắt để ẩn hiện, chữ nhập vào giữ nguyên, các animation chạy mượt.

- [x] T005 [US2] Mở `Login.xaml.cs`, khai báo cờ trạng thái `bool isLoginPasswordVisible = false;` và `bool isRegisterPasswordVisible = false;`.
- [x] T006 [US2] Viết hàm xử lý sự kiện `btnToggleLoginPassword_Click` để:
  1. Toggle cờ trạng thái.
  2. Bật/Tắt `Visibility` của `PasswordBox` và `TextBox`.
  3. Bắt đầu (Begin) hiệu ứng animation cho đường gạch chéo của mắt.
- [x] T007 [US2] Tương tự T006, viết hàm `btnToggleRegisterPassword_Click`.
- [x] T008 [US2] Bổ sung các event `PasswordChanged` và `TextChanged` cho từng ô nhập liệu để đồng bộ giá trị `Text` lẫn nhau liên tục.
- [x] T009 [US2] Build (F5) và nghiệm thu trực tiếp trên giao diện thực.

## Dependencies
- Phase 2 yêu cầu Phase 1 phải hoàn tất trước (vì cần có các biến `x:Name` từ XAML).

## Implementation Strategy
- Làm dứt điểm từng khu vực (Tab Đăng nhập xong rồi mới qua Tab Đăng ký). Tránh để code rối.
- Kiểm tra tính đồng bộ Text cẩn thận vì đây là trường bảo mật quan trọng.
