# Tasks: Hợp nhất các View thành phần

**Input**: Design documents from `/specs/004-consolidate-feature-views/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, quickstart.md

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2)
- Include exact file paths in descriptions

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [x] T001 Validate current WPF project structure builds without errors.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

- [x] T002 Ensure all UI files are located inside `WPF/Features/` before proceeding with consolidation.

**Checkpoint**: Foundation ready - user story implementation can now begin.

---

## Phase 3: User Story 1 - Hợp nhất các View phân mảnh (Priority: P1) 🎯 MVP

**Goal**: Gộp tất cả các control nhỏ thuộc cùng 1 tính năng vào chung 1 tệp XAML chính.

**Independent Test**: XAML của các view con được di chuyển hoàn toàn vào XAML chính và project không bị lỗi giao diện.

### Implementation for User Story 1

- [x] T003 [P] [US1] Hợp nhất `BudgetProgressControl` vào `BudgetsViewControl.xaml` trong `WPF/Features/Budget/`
- [x] T004 [P] [US1] Hợp nhất `SummaryCardControl` và logic vào `DashboardHomeControl.xaml` trong `WPF/Features/Dashboard/`
- [x] T005 [P] [US1] Hợp nhất `RecentTransactionControl` vào `TransactionsViewControl.xaml` trong `WPF/Features/Transactions/`
- [x] T006 [P] [US1] Hợp nhất bất kỳ UserControl con nào khác (nếu có) vào các view tương ứng (Categories, Reports, Wallets, Profile).

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently.

---

## Phase 4: User Story 2 - Đổi tên View cho ngắn gọn và nhất quán (Priority: P2)

**Goal**: Đổi tên các View chính bỏ hậu tố `Control` để tên ngắn gọn hơn.

**Independent Test**: Tên class và tệp được cập nhật đồng bộ, dự án build thành công.

### Implementation for User Story 2

- [x] T007 [P] [US2] Đổi tên tệp và class `BudgetsViewControl` thành `BudgetsView` (`WPF/Features/Budget/BudgetsView.xaml` và `.cs`)
- [x] T008 [P] [US2] Đổi tên tệp và class `DashboardHomeControl` thành `DashboardHomeView` (`WPF/Features/Dashboard/DashboardHomeView.xaml` và `.cs`)
- [x] T009 [P] [US2] Đổi tên tệp và class `TransactionsViewControl` thành `TransactionsView` (`WPF/Features/Transactions/TransactionsView.xaml` và `.cs`)
- [x] T010 [P] [US2] Đổi tên tệp và class `CategoriesViewControl` thành `CategoriesView` (`WPF/Features/Categories/CategoriesView.xaml` và `.cs`)
- [x] T011 [P] [US2] Đổi tên tệp và class `ReportsViewControl` thành `ReportsView` (`WPF/Features/Reports/ReportsView.xaml` và `.cs`)
- [x] T012 [P] [US2] Đổi tên tệp và class `ProfileViewControl` thành `ProfileView` (`WPF/Features/Profile/ProfileView.xaml` và `.cs`)
- [x] T013 [P] [US2] Đổi tên tệp và class `WalletsViewControl` thành `WalletsView` (`WPF/Features/Wallets/WalletsView.xaml` và `.cs`)

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently.

---

## Phase 5: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [x] T014 Cập nhật references trong `WPF/ViewModels/DashboardViewModel.cs` để trỏ tới các View mới (đã loại bỏ hậu tố Control).
- [x] T015 Cập nhật namespaces và UI references trong `WPF/Views/DashboardWindow.xaml` nếu cần thiết (loại bỏ xmlns của UserControls cũ nếu còn sót lại).
- [x] T016 Xóa bỏ tất cả các tệp tin UserControl con dư thừa sau khi đã hợp nhất mã nguồn (các file như `BudgetProgressControl.xaml/cs`, `RecentTransactionControl.xaml/cs`, `SummaryCardControl.xaml/cs` v.v.).
- [x] T017 Run quickstart.md validation để đảm bảo dự án build thành công.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - Phải hợp nhất view (US1) trước khi đổi tên toàn bộ (US2).
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### Parallel Opportunities

- Việc gộp view trong từng thư mục tính năng (T003 - T006) có thể làm song song vì mỗi tính năng ở thư mục riêng.
- Tương tự việc đổi tên tệp (T007 - T013) có thể làm song song trên từng tính năng.

---

## Notes

- [P] tasks = different files, no dependencies
- Verify XAML layout does not break when replacing `UserControl` references with direct raw layout items.
- Ensure any ViewModels backing the small sub-controls are properly injected or accessed inside the consolidated views.
