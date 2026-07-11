# CONTEXT.md — Dashboard (Bảng điều khiển tổng quan)
# Người viết: AI Assistant | Ngày: 2026-07-06

## 1. PROBLEM STATEMENT
Khi sinh viên mở ứng dụng, họ cần ngay lập tức biết được mình đang có bao nhiêu tiền (Tổng số dư), tháng này đã thu/chi bao nhiêu và các giao dịch gần nhất là gì. Nếu không có Dashboard, họ phải bấm vào từng ví hoặc từng báo cáo để xem, gây mất thời gian và giảm trải nghiệm người dùng (UX). 

## 2. DOMAIN KNOWLEDGE
- **Dashboard (Bảng điều khiển):** Màn hình chính đầu tiên xuất hiện sau khi đăng nhập thành công. Tổng hợp thông tin cốt lõi từ nhiều module khác nhau (Ví, Giao dịch, Ngân sách).
- **Tổng số dư (Total Balance):** Tổng tiền khả dụng cộng gộp từ tất cả các ví (Wallets) đang hoạt động (`IsActive = 1`) của người dùng.
- **Giao dịch gần đây (Recent Transactions):** Danh sách khoảng 5 giao dịch mới nhất được thực hiện, giúp nhận biết các khoản tiền vừa biến động nhanh chóng.

## 3. STAKEHOLDERS
- **Sinh viên (End Users):** Người xem tổng quan tài chính để đưa ra các quyết định chi tiêu nhanh chóng trong ngày.
- **Nhà phát triển (Developers):** Người thiết kế UI Dashboard sao cho trực quan, load nhanh và thực hiện các câu truy vấn tổng hợp (aggregation queries) từ Local Database một cách tối ưu.

## 4. CONSTRAINTS (ràng buộc không thể thay đổi)
- **Tech:** 
  - Giao diện WPF XAML. Phải tích hợp các Animation lúc tải trang (các thành phần tự động "mọc" lên, số chạy, hoặc thẻ trượt) để tạo cảm giác hiện đại và liền mạch.
- **Business:** 
  - Dữ liệu hiển thị trên Dashboard phải được truy xuất từ Local Database và phản ánh thời gian thực tình trạng tài chính hiện tại (ví dụ: vừa thêm giao dịch thì số dư trên Dashboard lập tức thay đổi).

## 5. ASSUMPTIONS (giả định — cần confirm)
- Dashboard chỉ mang tính chất "Xem tổng quan" (Read-only view). Các thao tác "Xem tất cả" sẽ điều hướng (Navigate) người dùng đến màn hình Danh sách chi tiết.
- Các chỉ số Thu/Chi luôn hiển thị mặc định theo "Tháng hiện tại" (Current Month) dựa vào ngày hệ thống.

## 6. OPEN QUESTIONS (câu hỏi chưa có câu trả lời)
- Không có (Các ràng buộc đều được thiết kế để khớp hoàn toàn với các bảng `Wallets`, `FinanceTransactions` trong Local DB).
