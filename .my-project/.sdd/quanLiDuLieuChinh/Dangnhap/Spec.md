# Feature Specification: Nút xem Mật khẩu (Eye Icon) có Hiệu ứng động

## 1. Tên tính năng (Short Name)
`password-visibility-animation`

## 2. Mục tiêu (Goals)
- Thêm biểu tượng "Con mắt" (Eye Icon) vào các ô nhập Mật khẩu trên cả hai tab Đăng nhập (Login) và Đăng ký (Register).
- Bổ sung hiệu ứng động (automation animation) cho con mắt khi người dùng click vào để ẩn/hiện mật khẩu, tạo cảm giác tương tác mượt mà.

## 3. Yêu cầu Chức năng (Functional Requirements)
- Nút con mắt phải có mặt ở:
  1. Ô Mật khẩu (Tab Đăng nhập)
  2. Ô Mật khẩu (Tab Đăng ký)
- Khi bấm vào "Con mắt" (lần 1): Hiển thị mật khẩu dưới dạng văn bản rõ (Text).
- Khi bấm vào "Con mắt" (lần 2): Ẩn mật khẩu thành dấu chấm đen (Password dạng ẩn).
- Cần đồng bộ hóa dữ liệu giữa ô `PasswordBox` và ô `TextBox` chứa mật khẩu hiện rõ.

## 4. Yêu cầu Hiệu ứng Động (Animation Requirements)
- Chuyển động của Con mắt: Khi thay đổi trạng thái từ "Hiển thị" sang "Ẩn", biểu tượng con mắt sẽ có một đường chéo (Slash) chạy qua bằng hiệu ứng vẽ tự động (sử dụng `StrokeDashOffset` animation) hoặc hiệu ứng nháy mắt (Blink effect - Scale thu hẹp chiều cao xuống 0 rồi mở lên với icon mới).
- Hiệu ứng thay đổi text: Có hiệu ứng Fade (mờ dần sang rõ) nhẹ trong 0.2 giây khi chuyển đổi giữa chữ rõ và dấu chấm.

## 5. Ràng buộc Kỹ thuật (Technical Constraints)
- Chỉ sử dụng WPF XAML thuần (`PasswordBox` và `TextBox` chồng lên nhau, dùng thuộc tính `Visibility`).
- Icon con mắt dùng XAML `Path` vector, không dùng file ảnh tĩnh để có thể làm hiệu ứng Stroke/Scale.
- Cần xử lý bằng code-behind (`Login.xaml.cs`) cho logic đồng bộ chữ và kích hoạt Storyboard/Animation.

## 6. Tiêu chí Thành công (Success Criteria)
- [ ] Bấm nút mắt -> Mật khẩu hiện rõ chữ.
- [ ] Bấm lại nút mắt -> Mật khẩu biến thành dấu sao/chấm đen.
- [ ] Icon con mắt có hiệu ứng động khi chuyển đổi trạng thái.
- [ ] Chữ nhập vào ở trạng thái hiện hay ẩn đều phải giống hệt nhau khi lưu/submit.
