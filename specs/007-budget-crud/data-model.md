# Data Model

## Existing Entities

**BudgetData** (in `WPF/UIData.cs`)
- `CategoryName`: string
- `Icon`: string
- `TotalAmount`: decimal (Limit)
- `SpentAmount`: decimal
- *New Properties needed*:
  - `CategoryId`: int (to uniquely identify the category for update/delete)
  - `BudgetId`: int (Primary key for Budget record in DB)

**Budget** (in DB)
- `Id`: int (PK)
- `CategoryId`: int (FK)
- `AmountLimit`: decimal
- `Month`: int
- `Year`: int

## Operations

- `IBudgetRepository.AddBudget(Budget budget)`
- `IBudgetRepository.UpdateBudget(Budget budget)`
- `IBudgetRepository.DeleteBudget(int budgetId)`
- Similar methods in `IBudgetService`
