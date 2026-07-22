# Quickstart Validation

## Prerequisites
- MS SQL Server or MS Access Database running with the current connection string.
- Application compiles successfully via `dotnet build`.

## Validation Scenarios

### Scenario 1: Create Budget
1. Run application and navigate to the "Ngân sách" tab.
2. Click the `+` button in the header or the `Add Budget` button.
3. Select a category, enter `1,000,000`, and Save.
4. Verify the new budget appears as a card in the "Chi tiết Danh mục" section.

### Scenario 2: Edit Budget
1. Click the Edit (✏️) icon on the newly created budget card.
2. Change the limit to `2,000,000` and Save.
3. Verify the card updates immediately to reflect the new total and the progress bar updates.

### Scenario 3: Delete Budget
1. Click the Delete (🗑️) icon on the budget card.
2. Confirm the deletion prompt.
3. Verify the card is removed from the "Chi tiết Danh mục" list.
