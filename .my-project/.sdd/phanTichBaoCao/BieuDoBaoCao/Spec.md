# Feature: Biểu Đồ Báo Cáo (Reporting Charts)
Status: Draft
Author: AI Assistant | Reviewer: [Tên Reviewer] | Date: 2026-07-06
Priority: High

## 1. Business Context
Tính năng này cung cấp một cái nhìn trực quan, sinh động về tình hình thu chi thông qua các loại biểu đồ (tròn, cột, đường). Nó giúp sinh viên dễ dàng nhận diện thói quen tiêu dùng, từ đó đưa ra các quyết định tài chính cá nhân thông minh hơn và sớm đạt được các mục tiêu tài chính đã đề ra. Nó là cốt lõi của module "Phân tích báo cáo".

## 2. User Stories
# Story 1 (Happy Path - Xem tỷ trọng):
# As a sinh viên (student), I want to xem biểu đồ tròn thể hiện tỷ trọng chi tiêu theo danh mục, so that tôi biết mình đang tiêu tiền nhiều nhất vào khoản nào.

# Story 2 (Happy Path - Xem xu hướng):
# As a sinh viên (student), I want to xem biểu đồ cột/đường so sánh tổng thu và tổng chi qua các tháng, so that tôi biết dòng tiền của mình đang âm hay dương qua từng chu kỳ.

# Story 3 (Happy Path - Xem chi tiết/Drill-down):
# As a sinh viên (student), when tôi bấm vào một danh mục trên biểu đồ tròn, I want to xem danh sách các giao dịch thuộc danh mục đó, so that tôi biết dòng tiền cụ thể đã đi đâu.

# Story 4 (Edge Case - Không có dữ liệu):
# As a sinh viên (student), when không có giao dịch nào trong khoảng thời gian được chọn, I want to thấy thông báo "Chưa có dữ liệu", so that tôi không bị bối rối với một biểu đồ trống hoặc lỗi.

## 3. Acceptance Criteria (EARS)
# WHEN user truy cập vào trang "Phân tích báo cáo" (hoặc tab Biểu đồ)
# THE SYSTEM SHALL hiển thị biểu đồ tỷ trọng chi tiêu (Pie/Donut Chart) và biểu đồ xu hướng dòng tiền (Bar/Line Chart) với dữ liệu của tháng hiện tại mặc định.

# WHEN user bấm vào một phần (slice) của biểu đồ tròn
# THE SYSTEM SHALL hiển thị danh sách chi tiết các giao dịch thuộc danh mục đó trong khoảng thời gian đang được chọn.

# WHEN user thay đổi bộ lọc thời gian (ví dụ: chọn từ ngày đến ngày, hoặc chọn khoảng tháng)
# THE SYSTEM SHALL tính toán lại và cập nhật toàn bộ biểu đồ tương ứng với dữ liệu mới.

# WHEN không có dữ liệu giao dịch thỏa mãn điều kiện lọc
# THE SYSTEM SHALL hiển thị thông báo "Không có dữ liệu giao dịch trong khoảng thời gian này" ở chính giữa khu vực hiển thị biểu đồ.

## 4. Data Access Contract (Service / Repository Layer)
Do dự án sử dụng Local Database thay vì API, giả định các lớp Service / DAO sẽ cung cấp các hàm sau cho UI ViewModel:

# Method: GetExpensesByCategoryAsync(int userId, DateTime startDate, DateTime endDate)
# Parameters: userId, startDate, endDate
# Return Type: IEnumerable<{ CategoryName: string, TotalAmount: decimal, Percentage: double, ColorCode: string }>

# Method: GetCashFlowTrendAsync(int userId, DateTime startDate, DateTime endDate, PeriodType period)
# Parameters: userId, startDate, endDate, period (Theo ngày/tuần/tháng)
# Return Type: IEnumerable<{ PeriodLabel: string, TotalIncome: decimal, TotalExpense: decimal }>

# Method: GetTransactionsByCategoryAsync(int userId, int categoryId, DateTime startDate, DateTime endDate)
# Parameters: userId, categoryId, startDate, endDate
# Return Type: IEnumerable<{ TransactionId: int, Amount: decimal, TransactionDate: DateTime, Description: string }>

## 5. Technical Constraints
# - Max render/load time: Quá trình query dữ liệu và render biểu đồ phải hoàn thành dưới 1 giây.
# - UI Animation: Biểu đồ cần hỗ trợ hiệu ứng (animation) vẽ hoặc mọc lên nhẹ nhàng khi load dữ liệu để tạo cảm giác ứng dụng "sống động" (tham khảo specs login-dashboard-animation).
# - Responsiveness: Biểu đồ phải tự động điều chỉnh kích thước (resize) mượt mà không bị vỡ bố cục khi thay đổi kích thước cửa sổ WPF.

## 6. Out of Scope
# - Tính năng xuất biểu đồ/báo cáo ra file định dạng Ảnh, Excel hoặc PDF (đã xác nhận không làm trong phiên bản này).
# - Tự động phân tích bằng AI hoặc đưa ra các lời khuyên tiết kiệm thông minh (Smart Insights).
