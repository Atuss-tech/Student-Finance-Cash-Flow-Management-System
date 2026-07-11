# Data Model

This feature is entirely focused on UI and presentation layer modifications. The core data models (Entities, DTOs, DbContext) remain completely unchanged.

## UI View Models

However, we need to adapt the ViewModels to bind correctly to the new UI structures:

**WalletsViewModel Updates:**
- Add Summary metrics: `TotalBalance`, `TotalIncome`, `TotalExpense`
- Add dynamic properties for `Wallet` items: `Icon` (or `IconKey`), `ColorHex`
- Add Chart series data for `CashFlowByWalletSeries` and `WalletAllocationSeries`.

**CategoriesViewModel Updates:**
- Add Summary metrics: `TotalCategories`, `HighestSpendingCategory`, `BudgetHealthPercentage`.
- Add dynamic properties for `Category` items: `Icon`, `ColorHex`, `CurrentSpending`, `BudgetLimit`, `ProgressPercentage`.
- Add Chart series data for `CategoryBreakdownSeries`.
