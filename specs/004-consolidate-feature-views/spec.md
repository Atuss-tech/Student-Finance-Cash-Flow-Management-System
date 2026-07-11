# Feature Specification: Hợp nhất các View thành phần (Consolidate Feature Views)

**Feature Branch**: `[004-consolidate-feature-views]`

**Created**: 2026-07-10

**Status**: Draft

**Input**: User description: "trong folder này chỉ cần 1 BudgetsView.xaml và BudgetsView.cs thôi, tất cả liên đến Budgets thì làm ở trong này, các tính năng khác cũng vậy"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Hợp nhất View cho từng tính năng (Priority: P1)

Với tư cách là một nhà phát triển/người bảo trì, tôi muốn mỗi thư mục tính năng (ví dụ `WPF/Features/Budget`) chỉ chứa đúng 1 tệp XAML chính (ví dụ `BudgetsView.xaml`), thay vì bị phân mảnh thành nhiều UserControl nhỏ (như `BudgetProgressControl.xaml`), để dễ dàng quản lý mã nguồn và bảo trì.

**Why this priority**: Cấu trúc tinh gọn giúp giảm số lượng tệp không cần thiết, làm sạch project và tập trung logic UI vào một nơi duy nhất.

**Independent Test**: Có thể kiểm tra độc lập bằng cách quan sát cấu trúc thư mục của dự án và đảm bảo mã XAML cũ của các control nhỏ đã được gộp thành công vào file View chính mà không gây lỗi giao diện.

**Acceptance Scenarios**:

1. **Given** thư mục `WPF/Features/Budget` đang chứa nhiều UserControl, **When** hợp nhất mã nguồn, **Then** chỉ còn lại duy nhất `BudgetsView.xaml` (và code-behind) chứa toàn bộ giao diện của tính năng ngân sách.
2. **Given** thư mục `WPF/Features/Dashboard`, **When** hợp nhất mã nguồn, **Then** `SummaryCardControl` được gộp trực tiếp vào `DashboardHomeView.xaml`.

---

### User Story 2 - Đổi tên View cho ngắn gọn và nhất quán (Priority: P2)

Với tư cách là một nhà phát triển, tôi muốn các View chính bỏ hậu tố `Control` để tên ngắn gọn hơn (ví dụ `BudgetsViewControl` -> `BudgetsView`), tuân thủ chuẩn đặt tên chung của dự án.

**Why this priority**: Sự nhất quán trong cách đặt tên giúp code dễ đọc và dễ tìm kiếm.

**Independent Test**: Biên dịch dự án thành công sau khi tất cả các View và class liên quan đã được đổi tên.

**Acceptance Scenarios**:

1. **Given** tên class là `BudgetsViewControl`, **When** đổi tên, **Then** tên mới là `BudgetsView` ở cả file `.xaml`, file `.cs` và trong quá trình gọi từ `DashboardViewModel`.

---

### Edge Cases

- Điều gì xảy ra nếu các View con sử dụng DataContext riêng biệt hoặc ViewModel riêng biệt? Trả lời: Các View con hiện tại được cấp phát DataContext từ ViewModel tổng, sau khi gộp, việc binding vẫn sẽ diễn ra dựa trên DataContext của View chính.
- Điều gì xảy ra với các tệp `.cs` (code-behind) của các UserControl phụ? Trả lời: Mọi logic trong code-behind phụ (nếu có) phải được di chuyển sang code-behind của View chính, hoặc lý tưởng nhất là chuyển vào ViewModel.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Hệ thống PHẢI hợp nhất nội dung XAML của tất cả các UserControl con vào một tệp View duy nhất cho mỗi tính năng (Ví dụ: `Features/Budget` chỉ có `BudgetsView.xaml`, `Features/Dashboard` chỉ có `DashboardHomeView.xaml`).
- **FR-002**: Hệ thống PHẢI loại bỏ các tệp tin UserControl con (bao gồm `.xaml` và `.cs`) sau khi đã hợp nhất mã nguồn an toàn.
- **FR-003**: Hệ thống PHẢI đổi tên tất cả các file View chính bằng cách bỏ hậu tố "Control" (ví dụ: `BudgetsViewControl.xaml` -> `BudgetsView.xaml`).
- **FR-004**: Hệ thống PHẢI cập nhật logic định tuyến (Navigation) trong `DashboardViewModel.cs` để gọi đúng các View đã được hợp nhất và đổi tên.

### Key Entities

- **Feature Views**: Giao diện gốc của từng tính năng nằm trong thư mục tương ứng.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Số lượng tệp tin XAML trong các thư mục `WPF/Features/*` giảm xuống đúng bằng 1 tệp XAML trên mỗi thư mục tính năng (không tính các thư mục con Dialogs nếu có).
- **SC-002**: Dự án biên dịch (build) thành công 100% không có lỗi tham chiếu (missing references).
- **SC-003**: Ứng dụng khởi chạy và điều hướng thành công giữa tất cả các tính năng từ thanh menu mà không bị mất bất kỳ thành phần giao diện (UI) nào so với trước khi hợp nhất.

## Assumptions

- Các UserControl phụ hiện tại không chứa logic xử lý (code-behind) phức tạp nào ngoài `InitializeComponent()`. Nếu có, chúng có thể dễ dàng chuyển vào View chính hoặc ViewModel.
- Việc đổi tên tệp (rename) được hỗ trợ tốt bởi hệ thống git và IDE.
- Giao diện UI sẽ được copy-paste nguyên bản (gộp thẻ) nên sẽ giữ nguyên thiết kế và không ảnh hưởng đến trải nghiệm người dùng cuối.
