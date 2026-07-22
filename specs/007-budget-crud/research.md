# Research & Decisions

## UI Layout for CRUD

- **Decision**: Add "Edit" (✏️) and "Delete" (🗑️) icon buttons inside each budget card in `BudgetsView.xaml`. For Create, reuse `AddBudgetWindow`.
- **Rationale**: Keeps the UI clean and aligns with standard Material/StitchCard design practices.
- **Alternatives considered**: Context menus (Right-click) - rejected because it's less discoverable.

## Data Refresh

- **Decision**: Update the specific item in the `ObservableCollection<BudgetData>` in `BudgetsView` to avoid full data re-fetch and screen flickering.
- **Rationale**: Provides the fastest UI feedback (performance goal).
- **Alternatives considered**: Reloading all budgets via `LoadBudgetDataAsync` - rejected due to animation resets.
