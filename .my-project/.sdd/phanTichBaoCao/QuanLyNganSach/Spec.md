# Feature: Quản Lý Ngân Sách (Budget Management)
Status: Draft
Author: AI Assistant | Reviewer: [Tên Reviewer] | Date: 2026-07-06
Priority: High

## 1. Business Context
Tính năng Quản lý ngân sách giúp sinh viên lên kế hoạch và giới hạn số tiền chi tiêu tối đa cho các mục đích cụ thể theo từng tháng. Nó hoạt động như một nền tảng cốt lõi, cung cấp dữ liệu đầu vào cho tính năng "Cảnh báo vượt ngân sách", đóng vai trò quan trọng trong việc rèn luyện tính kỷ luật và ngăn chặn tình trạng vung tay quá trán.

## 2. User Stories
# Story 1 (Happy Path - Thiết lập ngân sách):
# As a sinh viên (student), when tôi chọn một danh mục chi tiêu (VD: Ăn uống) và nhập số tiền giới hạn cho tháng hiện tại, I want to lưu lại cấu hình này thành công, so that hệ thống có cơ sở để theo dõi và cảnh báo tôi.

# Story 2 (Happy Path - Xem tiến độ ngân sách):
# As a sinh viên (student), I want to xem danh sách các ngân sách đã lập trong tháng kèm theo một thanh tiến trình (progress bar) thể hiện mức độ chi tiêu hiện tại so với hạn mức, so that tôi biết mình còn bao nhiêu dư địa (ngân sách còn lại).

# Story 3 (Edge Case - Trùng ngân sách):
# As a sinh viên (student), when tôi cố gắng tạo thêm một ngân sách mới cho một danh mục đã được đặt ngân sách trong cùng một tháng đó, I want to nhận được thông báo lỗi "Ngân sách cho danh mục này đã tồn tại", so that tôi không tạo ra các bản ghi trùng lặp gây lỗi tính toán.

## 3. Acceptance Criteria (EARS)
# WHEN user gửi yêu cầu tạo ngân sách mới với đầy đủ thông tin hợp lệ (Danh mục là 'Expense', Tháng 1-12, Số tiền giới hạn > 0)
# THE SYSTEM SHALL lưu bản ghi mới vào bảng `Budgets` và hiển thị thông báo thành công.

# WHEN user cố gắng tạo ngân sách cho một `CategoryId` đã tồn tại trong cùng `Month` và `Year` của chính `UserId` đó
# THE SYSTEM SHALL chặn thao tác (nhờ bắt lỗi Unique Constraint của DB) và hiển thị cảnh báo "Ngân sách đã tồn tại, vui lòng chỉnh sửa bản ghi hiện tại thay vì tạo mới".

# WHEN user xem danh sách ngân sách của một tháng
# THE SYSTEM SHALL tính toán Tổng thực chi (`SUM(Amount)` từ `FinanceTransactions`) cho từng danh mục trong tháng đó, và hiển thị song song với `AmountLimit` dưới dạng Progress Bar.

# WHEN user xóa (Delete) một ngân sách
# THE SYSTEM SHALL xóa bản ghi khỏi bảng `Budgets` nhưng KHÔNG làm ảnh hưởng đến các giao dịch chi tiêu (`FinanceTransactions`) đã phát sinh.

## 4. Data Access Contract (Service / Repository Layer)
Dự án sử dụng Local Database. Lớp Service/DAO sẽ cung cấp các hàm thao tác sau:

# Method: CreateOrUpdateBudgetAsync(int userId, int categoryId, int month, int year, decimal amountLimit, string note)
# Parameters: userId, categoryId, month, year, amountLimit, note
# Return Type: { Success: bool, Message: string, BudgetId: int }

# Method: GetBudgetProgressAsync(int userId, int month, int year)
# Parameters: userId, month, year
# Return Type: IEnumerable<{ BudgetId: int, CategoryName: string, AmountLimit: decimal, ActualSpent: decimal, RemainingAmount: decimal, ProgressPercentage: double }>

# Method: DeleteBudgetAsync(int budgetId)
# Parameters: budgetId
# Return Type: bool

## 5. Technical Constraints
# - Performance: Hàm `GetBudgetProgressAsync` cần thực hiện phép Join (kết nối) giữa bảng `Budgets`, `Categories` và kết hợp truy vấn `SUM` từ `FinanceTransactions`. Cần viết LINQ/SQL query tối ưu để đảm bảo tốc độ phản hồi nhanh < 0.5s.
# - UI: Progress bar hiển thị mức độ chi tiêu nên tự động đổi màu theo ngưỡng phần trăm nhờ DataTrigger WPF (Ví dụ: < 80% là màu xanh lá, 80-99% là màu vàng/cam, >= 100% là màu đỏ).

## 6. Out of Scope
# - Tính năng Copy (Sao chép) toàn bộ danh sách ngân sách của tháng trước sang tháng sau chỉ bằng 1 nút bấm (Có thể bổ sung ở phiên bản nâng cấp).
# - Tính năng gọi ý cấu hình ngân sách tự động (AI Recommendation) dựa trên thói quen chi tiêu trong quá khứ.
