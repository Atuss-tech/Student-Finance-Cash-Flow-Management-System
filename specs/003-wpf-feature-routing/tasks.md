# Tasks: Tổ chức lại Kiến trúc WPF theo Tính năng

**Input**: Design documents from `/specs/003-wpf-feature-routing/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, quickstart.md

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [x] T001 Tạo cấu trúc thư mục cốt lõi `WPF/Features` và các thư mục con: `Budget`, `Dashboard`, `Categories`, `Profile`, `Transactions`, `Reports`, `Wallets`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T002 Tạo lớp `NavigationItem` trong `WPF/Models/NavigationItem.cs` (nếu chưa có) để phục vụ cho việc bind dữ liệu menu điều hướng
- [x] T003 Cập nhật / Refactor `WPF/Views/DashboardWindow.xaml` để sử dụng `ContentControl` thay vì chứa cứng các control, biến nó thành bộ vỏ (shell) chính của ứng dụng
- [x] T004 Cập nhật code-behind `WPF/Views/DashboardWindow.xaml.cs` (hoặc ViewModel tương ứng) để hỗ trợ việc thay đổi thuộc tính `CurrentView` dựa trên item được chọn từ menu điều hướng (sử dụng Reload fresh logic)

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Tổ chức thư mục theo tính năng (Priority: P1) 🎯 MVP

**Goal**: Di chuyển tất cả các UserControls phân tán về đúng các thư mục Feature tương ứng.

**Independent Test**: Biên dịch thành công dự án mà không có lỗi thiếu tham chiếu view sau khi di chuyển các tệp tin.

### Implementation for User Story 1

- [x] T005 [P] [US1] Di chuyển `BudgetsViewControl.xaml` (.cs) và `BudgetProgressControl.xaml` (.cs) sang `WPF/Features/Budget/` và cập nhật namespace/tham chiếu tương ứng
- [x] T006 [P] [US1] Di chuyển `CategoriesViewControl.xaml` (.cs) sang `WPF/Features/Categories/` và cập nhật namespace/tham chiếu tương ứng
- [x] T007 [P] [US1] Di chuyển `DashboardHomeControl.xaml` (.cs) và `SummaryCardControl.xaml` (.cs) sang `WPF/Features/Dashboard/` và cập nhật namespace/tham chiếu
- [x] T008 [P] [US1] Di chuyển `ProfileViewControl.xaml` (.cs) sang `WPF/Features/Profile/` và cập nhật namespace/tham chiếu
- [x] T009 [P] [US1] Di chuyển `TransactionsViewControl.xaml` (.cs) và `RecentTransactionControl.xaml` (.cs) sang `WPF/Features/Transactions/` và cập nhật namespace
- [x] T010 [P] [US1] Di chuyển `ReportsViewControl.xaml` (.cs) sang `WPF/Features/Reports/` và cập nhật namespace
- [x] T011 [P] [US1] Di chuyển `WalletsViewControl.xaml` (.cs) sang `WPF/Features/Wallets/` và cập nhật namespace

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently (dự án biên dịch thành công sau khi tổ chức lại).

---

## Phase 4: User Story 2 - Tổng hợp Tính năng & Định tuyến qua Main Window (Priority: P1)

**Goal**: Đảm bảo các navigation links trên `DashboardWindow.xaml` (Main Window) có thể định tuyến và load đúng các views từ các thư mục Feature.

**Independent Test**: Khởi chạy ứng dụng và click vào từng nút trên thanh menu để chuyển đổi thành công giữa các giao diện tính năng.

### Implementation for User Story 2

- [x] T012 [P] [US2] Cập nhật event handlers hoặc Commands trong `WPF/Views/DashboardWindow.xaml.cs` cho menu "Tổng quan" để gán `ContentControl.Content` thành một instance mới của `DashboardHomeControl`
- [x] T013 [P] [US2] Cập nhật event handlers/Commands trong `DashboardWindow.xaml.cs` cho menu "Giao dịch" để tải `TransactionsViewControl`
- [x] T014 [P] [US2] Cập nhật event handlers/Commands trong `DashboardWindow.xaml.cs` cho menu "Ví" để tải `WalletsViewControl`
- [x] T015 [P] [US2] Cập nhật event handlers/Commands trong `DashboardWindow.xaml.cs` cho menu "Ngân sách" để tải `BudgetsViewControl`
- [x] T016 [P] [US2] Cập nhật event handlers/Commands trong `DashboardWindow.xaml.cs` cho menu "Báo cáo" để tải `ReportsViewControl`
- [x] T017 [P] [US2] Cập nhật event handlers/Commands trong `DashboardWindow.xaml.cs` cho menu "Hạng mục" để tải `CategoriesViewControl`
- [x] T018 [P] [US2] Cập nhật event handlers/Commands trong `DashboardWindow.xaml.cs` cho menu "Tài khoản/Profile" để tải `ProfileViewControl`

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently.

---

## Phase 5: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [x] T019 Kiểm tra và dọn dẹp các namespaces bị thừa hoặc lỗi thời trong tất cả các file sau khi di chuyển thư mục.
- [x] T020 Run quickstart.md validation để đảm bảo biên dịch và chạy đúng như mong đợi.
- [x] T021 Xóa bỏ cấu trúc thư mục thừa (ví dụ `WPF/Views/UserControls` nếu trống).

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User Story 1 (Tổ chức thư mục) và User Story 2 (Định tuyến) có thể làm tuần tự hoặc xen kẽ.
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Có thể bắt đầu ngay sau Phase 1.
- **User Story 2 (P1)**: Phụ thuộc vào Phase 2 (Cơ sở hạ tầng định tuyến). Sẽ tốt hơn nếu thực hiện sau US1 để import đúng namespaces.

### Parallel Opportunities

- Việc di chuyển các file UI (T005 - T011) có thể chạy song song (mỗi view là độc lập).
- Việc gán các command/sự kiện cho menu (T012 - T018) có thể thực hiện liên tục và độc lập.

---

## Implementation Strategy

### MVP First (User Story 1 & 2)

1. Hoàn thành cấu trúc thư mục ban đầu và code nền tảng cho `DashboardWindow` (Phase 1, 2).
2. Di chuyển các views vào đúng thư mục (Phase 3).
3. Đấu nối logic điều hướng trong DashboardWindow để hiển thị view tương ứng (Phase 4).
4. Dọn dẹp thư mục trống và kiểm tra ứng dụng chạy (Phase 5).
