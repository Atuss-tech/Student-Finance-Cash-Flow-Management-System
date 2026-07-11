# Feature: Cảnh Báo Vượt Ngân Sách (Budget Exceeded Warning)
Status: Draft
Author: AI Assistant | Reviewer: [Tên Reviewer] | Date: 2026-07-06
Priority: High

## 1. Business Context
Tính năng này đóng vai trò như một "trợ lý tài chính", nhắc nhở sinh viên khi họ sắp hoặc đã chi tiêu quá số tiền cho phép trong tháng. Nó giúp sinh viên kiểm soát được thói quen mua sắm bốc đồng, tăng tính kỷ luật và giảm thiểu rủi ro cạn kiệt tài chính trước cuối tháng.

## 2. User Stories
# Story 1 (Happy Path - Cảnh báo vượt 100%):
# As a sinh viên (student), when tôi thêm một giao dịch chi tiêu làm tổng chi vượt quá ngân sách đã thiết lập, I want to nhận được thông báo cảnh báo ngay lập tức, so that tôi biết mình đã tiêu lẹm vào tiền tiết kiệm hoặc các khoản khác.

# Story 2 (Happy Path - Cảnh báo sớm 80%):
# As a sinh viên (student), when chi tiêu của tôi chạm mốc 80% ngân sách, I want to thấy một cảnh báo sớm (màu cam/vàng), so that tôi có thể bắt đầu thắt chặt chi tiêu trước khi hết tiền.

# Story 3 (Edge Case - Chưa thiết lập ngân sách):
# As a sinh viên (student), when tôi chưa thiết lập bất kỳ ngân sách nào cho danh mục, I want to hệ thống không hiển thị cảnh báo sai lệch hoặc báo lỗi, so that trải nghiệm sử dụng khi thêm giao dịch không bị gián đoạn.

## 3. Acceptance Criteria (EARS)
# WHEN user lưu thành công một giao dịch chi tiêu (Expense) mới vào Local Database
# THE SYSTEM SHALL tính toán lại tổng chi tiêu của danh mục đó trong tháng hiện tại và so sánh với ngân sách (Budget).

# WHEN tổng chi tiêu >= 100% ngân sách
# THE SYSTEM SHALL hiển thị cảnh báo mức độ cao (ví dụ: thông báo màu đỏ) báo hiệu "Đã vượt ngân sách".

# WHEN tổng chi tiêu đạt từ 80% đến dưới 100% ngân sách
# THE SYSTEM SHALL hiển thị cảnh báo mức độ trung bình (ví dụ: thông báo màu cam) báo hiệu "Sắp vượt ngân sách".

# WHEN user chưa thiết lập ngân sách cho danh mục của giao dịch đó
# THE SYSTEM SHALL bỏ qua việc kiểm tra và không hiển thị bất kỳ cảnh báo nào.

## 4. Data Access Contract (Service / Repository Layer)
Dự án sử dụng Local Database, giả định các lớp Service / DAO sẽ cung cấp các hàm sau để hỗ trợ logic cảnh báo:

# Method: CheckBudgetStatusAsync(int userId, int categoryId, int month, int year)
# Parameters: userId, categoryId, month, year
# Return Type: { Status: enum(Safe, Warning, Exceeded), AmountLimit: decimal, CurrentSpent: decimal, Percentage: double }

# Method: GetActiveAlertsAsync(int userId, int month, int year)
# Parameters: userId, month, year
# Return Type: IEnumerable<{ CategoryName: string, AmountLimit: decimal, CurrentSpent: decimal, AlertLevel: string }>

## 5. Technical Constraints
# - Performance: Quá trình kiểm tra ngân sách (`CheckBudgetStatusAsync`) nên chạy ngầm (async) và không được làm chậm hoặc block quá trình "Lưu giao dịch" (Save Transaction).
# - UI Notification: Cảnh báo phải hiển thị trực quan (ví dụ: Toast Notification nổi lên từ góc màn hình) và tự động ẩn sau khoảng 3-5 giây để không làm phiền người dùng, hoặc hiển thị cố định trong Notification Center của app.

## 6. Out of Scope
# - Tính năng tự động chặn (Block) không cho phép người dùng lưu giao dịch nếu vượt ngân sách (Chỉ dừng ở mức độ "cảnh báo", người dùng vẫn có quyền tiêu tiền của họ).
# - Gửi cảnh báo qua các kênh bên ngoài (Email, SMS, Push Notification lên thiết bị di động).
# - Tính năng "Tắt cảnh báo" (Mute alerts) hoặc tùy chỉnh mức % cảnh báo riêng biệt cho từng danh mục (do cấu trúc Local DB hiện tại không hỗ trợ lưu trữ các cấu hình này).
