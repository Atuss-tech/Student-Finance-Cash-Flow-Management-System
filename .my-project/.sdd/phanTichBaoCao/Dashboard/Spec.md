# Feature: Dashboard (Bảng điều khiển tổng quan)
Status: Draft
Author: AI Assistant | Reviewer: [Tên Reviewer] | Date: 2026-07-06
Priority: High

## 1. Business Context
Dashboard là "bộ mặt" của ứng dụng, mang lại cái nhìn bao quát nhất về sức khỏe tài chính của sinh viên ngay khi vừa khởi động. Một Dashboard trực quan, hiện đại, và tải dữ liệu nhanh sẽ tạo động lực lớn giúp sinh viên sử dụng ứng dụng mỗi ngày để quản lý thu chi. 

## 2. User Stories
# Story 1 (Happy Path - Xem tổng số dư):
# As a sinh viên (student), when tôi mở ứng dụng, I want to thấy "Tổng số dư" hiện tại cộng gộp của tất cả các ví, so that tôi biết mình còn tổng cộng bao nhiêu tiền để chi tiêu.

# Story 2 (Happy Path - Xem thu/chi trong tháng):
# As a sinh viên (student), I want to thấy tổng thu nhập và tổng chi tiêu của tháng hiện tại trên Dashboard, so that tôi đối chiếu nhanh với tình trạng dòng tiền của bản thân.

# Story 3 (Happy Path - Xem giao dịch gần đây):
# As a sinh viên (student), I want to xem danh sách 5 giao dịch mới nhất, so that tôi có thể kiểm tra lại các khoản tiền mình vừa thao tác.

# Story 4 (Edge Case - Chưa có ví/giao dịch nào):
# As a sinh viên (student), when tôi mới tạo tài khoản và chưa có dữ liệu, I want to thấy số dư bằng 0 và một giao diện "Trống" (Empty State) thân thiện, so that tôi không bị báo lỗi và biết cách bắt đầu.

## 3. Acceptance Criteria (EARS)
# WHEN user đăng nhập thành công và truy cập màn hình chính
# THE SYSTEM SHALL hiển thị Dashboard bao gồm: Tổng số dư (Total Balance), Tổng thu (Total Income), Tổng chi (Total Expense) của tháng hiện tại, và danh sách Giao dịch gần đây (Recent Transactions).

# WHEN user bấm vào nút "Xem tất cả" ở mục Giao dịch gần đây
# THE SYSTEM SHALL chuyển hướng user sang màn hình Quản lý/Danh sách giao dịch.

# WHEN có sự thay đổi dữ liệu (ví dụ: lưu giao dịch mới hoặc sửa/xoá giao dịch)
# THE SYSTEM SHALL tính toán và cập nhật lại các con số trên Dashboard ngay lập tức để đảm bảo tính đồng bộ.

## 4. Data Access Contract (Service / Repository Layer)
Dự án sử dụng Local Database, các lớp Service/DAO sẽ cung cấp các hàm lấy dữ liệu tổng hợp như sau:

# Method: GetDashboardSummaryAsync(int userId, int month, int year)
# Parameters: userId, month, year
# Return Type: { TotalBalance: decimal, TotalIncome: decimal, TotalExpense: decimal }

# Method: GetRecentTransactionsAsync(int userId, int limit = 5)
# Parameters: userId, limit
# Return Type: IEnumerable<{ TransactionId: int, WalletName: string, CategoryName: string, TransactionType: string, Amount: decimal, TransactionDate: DateTime, Description: string }>

## 5. Technical Constraints
# - Performance: Hàm `GetDashboardSummaryAsync` phải được tối ưu hoá (sử dụng query `SUM` trực tiếp trong CSDL thay vì load toàn bộ list lên C#) để đảm bảo Dashboard tải dưới 0.5s.
# - UI/UX Animation: Các thẻ (Cards) hiển thị số dư, thu chi phải áp dụng các hiệu ứng khởi động (Storyboard WPF) nhẹ nhàng theo đúng tiêu chuẩn thiết kế (như fade-in, trượt lên, số chạy).

## 6. Out of Scope
# - Chức năng tùy chỉnh (Customization) vị trí các thẻ (widgets) trên Dashboard bằng thao tác kéo thả (Drag & Drop) sẽ không hỗ trợ trong phiên bản này.
# - Các chỉ số dự đoán dòng tiền tương lai (Smart AI Prediction) trên Dashboard.
