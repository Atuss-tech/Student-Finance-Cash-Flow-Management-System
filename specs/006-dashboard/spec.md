# Feature Specification: Dashboard (Bảng điều khiển tổng quan)

**Feature Branch**: `[006-dashboard]`

**Created**: 2026-07-11

**Status**: Draft

## 1. Context & Business Value
Dashboard là "bộ mặt" của ứng dụng, mang lại cái nhìn bao quát nhất về sức khỏe tài chính của sinh viên ngay khi vừa khởi động. Khi sinh viên mở ứng dụng, họ cần ngay lập tức biết được mình đang có bao nhiêu tiền (Tổng số dư), tháng này đã thu/chi bao nhiêu và các giao dịch gần nhất là gì.

Một Dashboard trực quan, hiện đại, và tải dữ liệu nhanh sẽ tạo động lực lớn giúp sinh viên sử dụng ứng dụng mỗi ngày để quản lý thu chi. Việc đồng bộ phong cách thiết kế với toàn hệ thống (dựa trên tài liệu Design.md / Stitch AI UI) là vô cùng quan trọng.

## 2. User Stories
- **Story 1 (Happy Path - Xem tổng số dư):** As a sinh viên (student), when tôi mở ứng dụng, I want to thấy "Tổng số dư" hiện tại cộng gộp của tất cả các ví, so that tôi biết mình còn tổng cộng bao nhiêu tiền để chi tiêu.
- **Story 2 (Happy Path - Xem thu/chi trong tháng):** As a sinh viên (student), I want to thấy tổng thu nhập và tổng chi tiêu của tháng hiện tại trên Dashboard, so that tôi đối chiếu nhanh với tình trạng dòng tiền của bản thân.
- **Story 3 (Happy Path - Xem giao dịch gần đây):** As a sinh viên (student), I want to xem danh sách 5 giao dịch mới nhất, so that tôi có thể kiểm tra lại các khoản tiền mình vừa thao tác.
- **Story 4 (Edge Case - Chưa có ví/giao dịch nào):** As a sinh viên (student), when tôi mới tạo tài khoản và chưa có dữ liệu, I want to thấy số dư bằng 0 và một giao diện "Trống" (Empty State) thân thiện, so that tôi không bị báo lỗi và biết cách bắt đầu.

## 3. Acceptance Criteria
- **Scenario 1:** WHEN user đăng nhập thành công và truy cập màn hình chính, THE SYSTEM SHALL hiển thị Dashboard bao gồm: Tổng số dư (Total Balance), Tổng thu (Total Income), Tổng chi (Total Expense) của tháng hiện tại, và danh sách Giao dịch gần đây (Recent Transactions).
- **Scenario 2:** WHEN user bấm vào nút "Xem tất cả" ở mục Giao dịch gần đây, THE SYSTEM SHALL chuyển hướng user sang màn hình Quản lý/Danh sách giao dịch.
- **Scenario 3:** UI phải đồng bộ với thiết kế (Stitch AI / Design.md), áp dụng Dark Theme với cấu trúc cột/thẻ hiện đại (như thẻ `StitchCard`, các viền cong `CornerRadius`, màu `SurfaceColor`).

## 4. Technical Constraints & Out of Scope
- **Tech Stack constraint (Ràng buộc đặc biệt của User):** CHỈ ĐƯỢC CODE GIAO DIỆN (FRONTEND) TRONG CÁC FILE `.xaml`. Nghiêm cấm code ở các file backend `.cs` (như code-behind hay ViewModels) nếu chưa được cấp phép. Dữ liệu tạm thời (mock data) phải được dùng sao cho hợp lệ nếu cần để demo UI.
- **UI/UX Animation:** Các thẻ (Cards) hiển thị số dư, thu chi phải có hiệu ứng tĩnh tốt, hoặc animation nhẹ nhàng nếu làm được trực tiếp trong XAML.
- **Out of Scope:** Chức năng tùy chỉnh kéo thả thẻ trên Dashboard; Các chỉ số dự đoán bằng AI.
