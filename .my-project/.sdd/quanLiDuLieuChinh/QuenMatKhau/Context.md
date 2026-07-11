# CONTEXT.md — Quên Mật Khẩu (Forgot Password)
# Người viết: AI Agent | Ngày: 2026-07-07

## 1. PROBLEM STATEMENT
Sinh viên thường xuyên đăng nhập một lần rồi quên mật khẩu. Khi cần đăng nhập lại từ máy khác hoặc sau thời gian dài, nếu không có chức năng khôi phục, họ sẽ phải từ bỏ tài khoản cũ cùng toàn bộ dữ liệu lịch sử thu chi.

## 2. DOMAIN KNOWLEDGE
- **Authentication**: Xác thực và quản lý quyền truy cập.
- **Recovery Method**: Phương pháp khôi phục mật khẩu (Email OTP, SMS OTP, Security Question).

## 3. STAKEHOLDERS
- **Sinh viên**: Cần một cách an toàn và đơn giản để khôi phục quyền truy cập vào tài khoản của chính mình.

## 4. CONSTRAINTS (Ràng buộc)
- Ứng dụng Desktop thuần túy có thể không có sẵn SMTP Server đáng tin cậy để gửi Email tự động.
- Mật khẩu lưu trữ bắt buộc không được để dạng plain text (BR-014), nên không thể làm tính năng "Lấy lại mật khẩu cũ", chỉ có thể "Tạo mật khẩu mới".

## 5. ASSUMPTIONS (Giả định)
- Do là app Desktop nội bộ sinh viên, tính năng khôi phục sẽ ưu tiên sử dụng "Câu hỏi bảo mật" (Security Question) được thiết lập lúc đăng ký, thay vì gửi Email OTP để tránh cấu hình phức tạp.

## 6. OPEN QUESTIONS
- Liệu có muốn phát triển giả lập gửi Email (Ghi file log / Hiển thị luôn mã OTP ra MessageBox cho mục đích demo) hay dùng Câu hỏi bảo mật? (Đề xuất: Dùng Security Question cho thực tế).
