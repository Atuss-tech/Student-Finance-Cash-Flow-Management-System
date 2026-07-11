# CONTEXT.md — Cảnh Báo Vượt Ngân Sách (Budget Exceeded Warning)
# Người viết: AI Assistant | Ngày: 2026-07-06

## 1. PROBLEM STATEMENT
Sinh viên thường xuyên chi tiêu theo cảm xúc và không theo dõi sát sao giới hạn ngân sách đã đề ra (ví dụ: ngân sách ăn uống một tháng là 2 triệu nhưng tiêu đến 2.5 triệu mà không hay biết). Khi nhận ra thì tiền đã hết, dẫn đến tình trạng thiếu hụt tài chính cuối tháng. Hệ thống cần có cơ chế cảnh báo kịp thời để sinh viên dừng lại hoặc điều chỉnh hành vi chi tiêu.

## 2. DOMAIN KNOWLEDGE
- **Ngân sách (Budget):** Số tiền giới hạn (mức tối đa) được phép chi tiêu trong một khoảng thời gian (thường là một tháng) cho một danh mục cụ thể hoặc tổng thể.
- **Vượt ngân sách (Over-budget):** Trạng thái khi tổng số tiền đã chi (Actual Expense) lớn hơn hoặc bằng Ngân sách (Budget limit).
- **Mức cảnh báo (Threshold):** Mức phần trăm (%) chi tiêu so với ngân sách để bắt đầu kích hoạt thông báo (ví dụ: cảnh báo sớm khi chi tiêu đạt 80% ngân sách).

## 3. STAKEHOLDERS
- **Sinh viên (End Users):** Người thiết lập ngân sách và nhận cảnh báo để điều chỉnh việc chi tiêu cá nhân.
- **Nhà phát triển (Developers):** Người sẽ thực hiện logic kiểm tra ngân sách (trigger) mỗi khi có giao dịch mới và thiết kế UI hiển thị cảnh báo (Toast/Popup).

## 4. CONSTRAINTS (ràng buộc không thể thay đổi)
- **Tech:** 
  - Phải sử dụng WPF XAML. Cảnh báo hiển thị trong ứng dụng (In-app notification) như Toast Notification, hoặc một thông báo/icon nổi bật ở trang Báo cáo/Dashboard.
- **Business:** 
  - Hệ thống phải kiểm tra trạng thái ngân sách ngay khi một giao dịch chi tiêu mới được lưu thành công vào Local Database, đảm bảo tính realtime.

## 5. ASSUMPTIONS (giả định — cần confirm)
- Hệ thống đã có hoặc sẽ có chức năng "Thiết lập ngân sách" (Budget Management) để sinh viên định nghĩa số tiền tối đa.
- Dữ liệu được truy xuất trực tiếp từ Local Database (thông qua Service / DAO) nên việc kiểm tra (query) diễn ra cục bộ và nhanh chóng.
- Tính năng chỉ hiển thị cảnh báo trong phạm vi ứng dụng (không yêu cầu gửi Email hay SMS).

## 6. OPEN QUESTIONS (câu hỏi chưa có câu trả lời)
- Không còn câu hỏi mở.
- *Ghi chú từ Database:* Cấu trúc bảng `Budgets` không có cột lưu mức cấu hình % cảnh báo (`Threshold`) nên mốc cảnh báo (80%, 100%) sẽ được fix cứng trong logic code. Bảng cũng không có cột `IsMuted` nên sẽ không hỗ trợ tính năng "Tắt cảnh báo" cho từng danh mục riêng lẻ.
