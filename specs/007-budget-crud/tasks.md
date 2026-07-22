# Tasks: Budget CRUD

**Input**: Design documents from `/specs/007-budget-crud/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure (Skipped as project already exists)

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T001 [P] Add `CategoryId` and `BudgetId` properties to `BudgetData` model in `f:\SU26\Prn212\Practice\Student Finance & Cash Flow Management System\WPF\UIData.cs`
- [x] T002 Update `Budget` entity model (if not already having PK and FK properties) in Repository layer
- [x] T003 Implement `AddBudget`, `UpdateBudget`, `DeleteBudget` methods in `IBudgetRepository.cs` and `BudgetRepository.cs`
- [x] T004 Implement `AddBudget`, `UpdateBudget`, `DeleteBudget` methods in `IBudgetService.cs` and `BudgetService.cs`
- [x] T005 Modify `f:\SU26\Prn212\Practice\Student Finance & Cash Flow Management System\WPF\Features\Budget\AddBudgetWindow.xaml.cs` to support both Add and Edit modes (e.g., pass an existing budget to constructor).

**Checkpoint**: Foundation ready - CRUD logic in services and UI dialog structure is ready.

---

## Phase 3: User Story 1 - Add a New Budget (Priority: P1) 🎯 MVP

**Goal**: Users want to add a new monthly budget limit for a specific spending category.

**Independent Test**: Can be fully tested by creating a budget and verifying it appears in the Budgets dashboard list with the correct category and amount.

### Implementation for User Story 1

- [x] T006 [US1] Wire up the "Lưu Ngân Sách" button in `AddBudgetWindow.xaml.cs` to call `BudgetService.AddBudget` for new budgets.
- [x] T007 [US1] In `BudgetsView.xaml.cs`, refresh the specific budget item or the list when a budget is added successfully.
- [x] T008 [US1] Add basic validation (amount > 0, category selected) in `AddBudgetWindow.xaml.cs`.

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently.

---

## Phase 4: User Story 2 - Update an Existing Budget (Priority: P2)

**Goal**: Users want to update their budget limit for an existing category.

**Independent Test**: Can be tested by changing the amount of an existing budget and observing the updated progress bar.

### Implementation for User Story 2

- [x] T009 [US2] Add an "Edit" (✏️) button to the budget card data template in `f:\SU26\Prn212\Practice\Student Finance & Cash Flow Management System\WPF\Features\Budget\BudgetsView.xaml`.
- [x] T010 [US2] Implement the Edit button click handler in `BudgetsView.xaml.cs` to open `AddBudgetWindow` with the selected budget's data.
- [x] T011 [US2] Wire up the "Lưu Ngân Sách" button in `AddBudgetWindow.xaml.cs` to call `BudgetService.UpdateBudget` for existing budgets.
- [x] T012 [US2] Update the UI bindings in `BudgetsView.xaml.cs` to reflect the edited budget immediately without full reload.

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently.

---

## Phase 5: User Story 3 - Delete a Budget (Priority: P3)

**Goal**: Users want to delete a budget for a category if they no longer wish to track it.

**Independent Test**: Can be tested by deleting a budget and verifying it is removed from the dashboard.

### Implementation for User Story 3

- [x] T013 [US3] Add a "Delete" (🗑️) button next to the Edit button in `f:\SU26\Prn212\Practice\Student Finance & Cash Flow Management System\WPF\Features\Budget\BudgetsView.xaml`.
- [x] T014 [US3] Implement the Delete button click handler in `BudgetsView.xaml.cs` to show a confirmation dialog.
- [x] T015 [US3] If confirmed, call `BudgetService.DeleteBudget` and remove the item from the `ObservableCollection` in `BudgetsView.xaml.cs`.

**Checkpoint**: All user stories should now be independently functional.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [x] T016 Run quickstart.md validation manually to ensure all CRUD operations flow smoothly in the UI.

---

## Dependencies & Execution Order

- **Foundational (Phase 2)**: BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User Story 1 (P1): Can start after Foundational
  - User Story 2 (P2): Depends on US1 (needs a budget to edit)
  - User Story 3 (P3): Depends on US1 (needs a budget to delete)

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 2: Foundational
2. Complete Phase 3: User Story 1
3. **STOP and VALIDATE**: Test adding a budget independently.
