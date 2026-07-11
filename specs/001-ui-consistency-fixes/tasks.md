# Tasks: UI Consistency Fixes

## Phase 1: Setup

*Setup and project initialization tasks.*

- [/] T001 Define specific Color and Brush resources in `WPF/Resources/DarkTheme.xaml` to match the exact hex codes from Stitch AI HTML (e.g. Momo red, TCB blue, specific container background colors).

## Phase 2: Foundational

*Core components and prerequisites.*

- [x] T002 Update `ProductUiCardDark` style or create new specific styles in `WPF/Resources/Styles.xaml` to support Bento box layouts and split layouts if needed.

## Phase 3: Wallets Screen UI Update

**User Story 1: Consistent UI Styling across all views**

*Goal*: Make Wallets screen match Stitch AI HTML precisely.
*Independent Test*: Build WPF and verify Wallets screen has Top Summary box, 2/3 and 1/3 split charts at the bottom, and sparklines in cards.

- [x] T003 [P] [US1] Update `WalletsViewModel.cs` to include new properties: `TotalBalance`, `TotalIncome`, `TotalExpense`, and `WalletAllocationSeries`, `CashFlowByWalletSeries` for charts.
- [x] T004 [US1] Rewrite `WPF/Views/UserControls/WalletsViewControl.xaml`: Add a top Grid for the Summary (Total Balance, Income, Expense).
- [x] T005 [US1] Update `WPF/Views/UserControls/WalletsViewControl.xaml`: Modify the `ItemsControl` ItemTemplate for Wallets to include specific icons, colors, and the sparkline placeholder.
- [x] T006 [US1] Update `WPF/Views/UserControls/WalletsViewControl.xaml`: Add the bottom split layout with `CartesianChart` (for Dòng tiền) and `PieChart` (for Phân bổ nguồn tiền).

## Phase 4: Categories Screen UI Update

**User Story 1: Consistent UI Styling across all views**

*Goal*: Make Categories screen match Stitch AI HTML precisely.
*Independent Test*: Build WPF and verify Categories screen has Top Summary Bento boxes, Left side list with progress bars, Right side Pie Chart.

- [x] T007 [P] [US1] Update `CategoriesViewModel.cs` to include new properties: `TotalCategories`, `HighestSpendingCategory`, `BudgetHealthPercentage`, and chart series. Add progress bar properties to Category items.
- [x] T008 [US1] Rewrite `WPF/Views/UserControls/CategoriesViewControl.xaml`: Add the top 3-column Bento box Summary section.
- [x] T009 [US1] Update `WPF/Views/UserControls/CategoriesViewControl.xaml`: Create a 2-column Split Layout (Grid).
- [x] T010 [US1] Update `WPF/Views/UserControls/CategoriesViewControl.xaml`: On the left column, display the Categories list where each item has an icon, text, and a progress bar.
- [x] T011 [US1] Update `WPF/Views/UserControls/CategoriesViewControl.xaml`: On the right column, add the PieChart placeholder (Cơ cấu chi tiêu) and the Action card.

## Phase 5: Polish & Final Verification

- [x] T012 Run the application and manually verify Wallets and Categories screens using the steps in `quickstart.md`.
- [x] T013 Ensure all spacing (margins, padding) exactly matches the standard metrics defined in `Styles.xaml`.

## Dependencies

- **User Story 1 (Wallets)** depends on Phase 1 & 2.
- **User Story 1 (Categories)** depends on Phase 1 & 2.
- **Polish** depends on all User Stories.

## Parallel Execution Examples

- **Example 1**: T003 (WalletsViewModel) and T007 (CategoriesViewModel) can be done in parallel before doing the XAML updates.
