# Feature: Phân Tích Dòng Tiền (Cash Flow Analysis)
Status: Draft
Author: AI Assistant | Reviewer: [Tên Reviewer] | Date: 2026-07-06
Priority: High

## 1. Business Context
Tính năng Phân tích dòng tiền cung cấp cho sinh viên một bức tranh toàn cảnh và chân thực nhất về sự chênh lệch giữa Thu và Chi. Bằng cách hiển thị Dòng tiền ròng (Net Cash Flow), sinh viên sẽ nhận diện sớm các tháng bị "âm tiền" để lên kế hoạch cắt giảm chi phí kịp thời, giúp rèn luyện tư duy và thói quen tích lũy tài chính.

## 2. User Stories
# Story 1 (Happy Path - Xem dòng tiền ròng):
# As a sinh viên (student), when tôi truy cập chức năng Phân tích dòng tiền, I want to thấy Tổng thu, Tổng chi và Dòng tiền ròng (Thặng dư/Thâm hụt) của khoảng thời gian hiện tại, so that tôi biết mình đang làm ra nhiều hơn tiêu hay tiêu nhiều hơn làm.

# Story 2 (Happy Path - Xem xu hướng theo năm):
# As a sinh viên (student), I want to xem biểu đồ so sánh chênh lệch dòng tiền của từng tháng trong năm (từ Tháng 1 đến Tháng 12), so that tôi biết xu hướng tài chính của mình qua các tháng đang diễn biến tốt hay xấu.

# Story 3 (Edge Case - Dòng tiền ròng âm):
# As a sinh viên (student), when Dòng tiền ròng của tôi mang giá trị âm, I want to hệ thống hiển thị con số này thật nổi bật (ví dụ: đổi sang màu đỏ hoặc có dấu âm rõ ràng), so that tôi ngay lập tức bị thu hút sự chú ý và có sự cảnh giác.

## 3. Acceptance Criteria (EARS)
# WHEN user truy cập vào tính năng Phân tích dòng tiền
# THE SYSTEM SHALL hiển thị bảng hoặc các thẻ chỉ số tóm tắt gồm: Total Income (Tổng thu), Total Expense (Tổng chi), và Net Cash Flow (Thu trừ Chi) trong kỳ báo cáo được chọn.

# WHEN Dòng tiền ròng (Net Cash Flow) có giá trị < 0
# THE SYSTEM SHALL hiển thị giá trị này bằng một UI Style đặc biệt (ví dụ: chữ màu đỏ) để cảnh báo.

# WHEN user chọn xem phân tích "Theo Năm"
# THE SYSTEM SHALL query Local Database, gom nhóm tổng Thu/Chi theo từng tháng trong năm, và trả về dữ liệu của đủ 12 tháng để hiển thị lên Biểu đồ.

## 4. Data Access Contract (Service / Repository Layer)
Dự án sử dụng Local Database, lớp Service/DAO sẽ cung cấp các hàm chuyên biệt sau:

# Method: GetCashFlowSummaryAsync(int userId, DateTime startDate, DateTime endDate)
# Parameters: userId, startDate, endDate
# Return Type: { TotalIncome: decimal, TotalExpense: decimal, NetCashFlow: decimal }

# Method: GetCashFlowTrendByYearAsync(int userId, int year)
# Parameters: userId, year
# Return Type: IEnumerable<{ Month: int, TotalIncome: decimal, TotalExpense: decimal, NetCashFlow: decimal }>

## 5. Technical Constraints
# - Performance: Hàm `GetCashFlowTrendByYearAsync` bắt buộc phải sử dụng hàm tổng hợp trực tiếp trên Database (Ví dụ: `GROUP BY MONTH(TransactionDate)`) để tiết kiệm bộ nhớ, thay vì load tất cả `FinanceTransactions` của 1 năm lên RAM (C#) rồi mới tính toán. Thời gian query phải dưới 0.5 giây.
# - UI: Các thẻ hiển thị số tiền và trục biểu đồ phải hỗ trợ tốt việc format số âm (ví dụ: `-500,000 đ`) và tự động thay đổi màu sắc số liệu bằng tính năng `DataTrigger` / `IValueConverter` của WPF.

## 6. Out of Scope
# - Tính năng Dự báo dòng tiền (Cash Flow Forecast) cho các tháng tương lai dựa trên lịch sử (sẽ yêu cầu thuật toán Machine Learning hoặc tính trung bình phức tạp, không làm ở phiên bản này).
# - Tính toán các chỉ số tài chính doanh nghiệp phức tạp (như ROI, Thanh khoản) do không phù hợp với nhu cầu sinh viên cá nhân.
