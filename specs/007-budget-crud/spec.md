# Feature Specification: Budget CRUD

**Feature Branch**: `[007-budget-crud]`

**Created**: 2026-07-22

**Status**: Draft

**Input**: User description: "thêm chức năng crud vào kiểu thêm ngân sách, sửa xóa cho mình chứ và update chứ nhỉ"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Add a New Budget (Priority: P1)

Users want to add a new monthly budget limit for a specific spending category so that they can control their expenses for that category.

**Why this priority**: Creating a budget is the foundation of budget management.

**Independent Test**: Can be fully tested by creating a budget and verifying it appears in the Budgets dashboard list with the correct category and amount.

**Acceptance Scenarios**:

1. **Given** the user is on the Budgets view, **When** they click "Add Budget", select a category, enter an amount limit, and save, **Then** a new budget is created and displayed in the budget list.
2. **Given** the user is adding a budget, **When** they leave the amount blank or select no category, **Then** an error message is shown and the budget is not saved.
3. **Given** a budget already exists for a category in the current month, **When** the user tries to create another budget for the same category, **Then** they are prompted to update the existing budget instead.

---

### User Story 2 - Update an Existing Budget (Priority: P2)

Users want to update their budget limit for an existing category if their financial situation or goal changes.

**Why this priority**: Goals can change mid-month; users need flexibility to adjust targets.

**Independent Test**: Can be tested by changing the amount of an existing budget and observing the updated progress bar and remaining amount.

**Acceptance Scenarios**:

1. **Given** an existing budget, **When** the user clicks "Edit", changes the amount limit, and saves, **Then** the budget amount is updated and the progress is recalculated.

---

### User Story 3 - Delete a Budget (Priority: P3)

Users want to delete a budget for a category if they no longer wish to track it.

**Why this priority**: Less critical than creating/updating, but necessary for data management.

**Independent Test**: Can be tested by deleting a budget and verifying it is removed from the dashboard.

**Acceptance Scenarios**:

1. **Given** an existing budget, **When** the user clicks "Delete" and confirms the action, **Then** the budget is permanently removed from the view.

### Edge Cases

- What happens when a user sets a budget lower than the already spent amount for that category? (The progress should show over 100% and an over-budget alert).
- How does the system handle deleting a budget when there are already transactions tied to that category? (The budget limit is removed, but transactions remain intact).

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow users to create a budget limit for a specific category for the current month.
- **FR-002**: System MUST allow users to edit the amount limit of an existing budget.
- **FR-003**: System MUST allow users to delete an existing budget after a confirmation prompt.
- **FR-004**: System MUST validate that the budget amount is a positive number.
- **FR-005**: System MUST prevent creating multiple duplicate budgets for the same category in the same month.

### Key Entities

- **Budget**: Represents a spending limit for a specific category within a specific time period (month/year). Key attributes include Category, Limit Amount, Month, Year.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can successfully create a new budget in under 30 seconds.
- **SC-002**: Budgets correctly reflect the "over-budget" status immediately if the edited limit is lower than current spending.
- **SC-003**: 100% of budget CRUD operations immediately update the UI without requiring a manual page refresh.

## Assumptions

- Users only set budgets on a monthly basis (current month).
- The "Add Category" and "Add Transaction" functionalities are already working and provide the necessary category data and spent amounts.
- Deleting a budget does not delete transactions associated with its category.
