# Feature Specification: Quản lý Giao dịch (Transaction Management)

**Feature Branch**: `[005-transaction-management]`

**Created**: 2026-07-10

**Status**: Draft

**Input**: User description: "Thiết kế màn hình giao dịch dựa trên ảnh chụp từ Stitch AI. Quản lý Giao dịch bao gồm các tab như Income, Expense, danh sách hiển thị, biểu đồ hoặc công cụ lọc cơ bản."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Xem danh sách Giao dịch (Priority: P1)

Với tư cách là người dùng, tôi muốn xem danh sách các giao dịch (thu/chi) trên một màn hình quản lý tập trung, được thiết kế theo giao diện Dark Theme của Stitch AI, để tôi có thể dễ dàng kiểm soát dòng tiền.

**Why this priority**: Xem danh sách giao dịch là chức năng cốt lõi của ứng dụng quản lý tài chính.

**Independent Test**: Màn hình `TransactionsView.xaml` hiển thị đúng layout và tông màu của Stitch AI.

**Acceptance Scenarios**:

1. **Given** người dùng mở màn hình Giao dịch, **When** giao diện được tải lên, **Then** người dùng thấy danh sách giao dịch hiển thị rõ ràng thông tin (tên, ngày tháng, danh mục, số tiền) theo thiết kế dạng Card.

---

### User Story 2 - Chuyển đổi giữa các loại Giao dịch (Priority: P1)

Với tư cách là người dùng, tôi muốn có thể chuyển đổi nhanh giữa tab Income và Expense (hoặc Transfer), để tập trung vào một loại giao dịch cụ thể.

**Why this priority**: Phân loại giao dịch giúp người dùng dễ dàng phân tích chi tiêu.

**Independent Test**: Các thẻ điều hướng (Tabs) hiển thị và hoạt động đúng cách, tuân thủ UI design system (highlight màu xanh ngọc `#10b981`).

**Acceptance Scenarios**:

1. **Given** màn hình Giao dịch, **When** người dùng bấm vào tab "Expense", **Then** trạng thái tab được active (đổi màu) và chỉ hiển thị các giao dịch chi tiêu.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Hệ thống PHẢI thiết kế màn hình `TransactionsView.xaml` tuân thủ Dark Theme Design System (nền `#051424`, Card `#122131`, primary `#10b981`).
- **FR-002**: Hệ thống PHẢI sử dụng layout tương tự bản thiết kế Stitch AI (có header, thanh lọc/tab, bảng danh sách giao dịch).
- **FR-003**: Hệ thống PHẢI đảm bảo các thành phần UI (Search box, nút Filter, nút Add) được hiển thị đầy đủ và có định dạng bo góc chuẩn (ví dụ `12px`).

### Key Entities

- **Transaction**: Đối tượng giao dịch gồm Số tiền, Ngày, Danh mục, Ghi chú.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `TransactionsView.xaml` được thiết kế lại và hiển thị chuẩn 100% về mã màu và bố cục so với ảnh chụp màn hình Stitch AI.
- **SC-002**: Không có lỗi giao diện (UI) khi resize cửa sổ ứng dụng (sử dụng Grid linh hoạt).
- **SC-003**: Ứng dụng build thành công 100% không có lỗi xaml.

## Assumptions

- Giao diện được thiết kế chủ yếu ở dạng tĩnh để đảm bảo thẩm mỹ (UI fidelity) trước, sau đó ViewModel sẽ được gắn vào sau.
- Chỉ thiết kế lại View (`TransactionsView.xaml`), không thay đổi cấu trúc DataModel nếu không cần thiết.
