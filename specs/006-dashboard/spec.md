# Feature Specification: Navigate to Transactions from Dashboard

## 1. Overview
The goal is to enable navigation from the Dashboard's "Recent Transactions" section to the full Transactions list view when the user clicks the "Xem tất cả" (See all) button.

## 2. Requirements
- When the user clicks the "Xem tất cả" button on the DashboardHomeView, the application should switch the current active view to the Transactions view.
- The button is currently bound to `ShowTransactionsCommand` in the ViewModel. This command needs to trigger a navigation event or view switch in the Main window/ViewModel.

## 3. Current Implementation Details
- **DashboardHomeView.xaml**: Contains a button with `Command="{Binding ShowTransactionsCommand}"`.
- **DashboardHomeView.xaml.cs**: Code-behind for the view. The ViewModel is likely set here or inherited from the DataContext.
- Navigation typically happens in the MainViewModel or a navigation service that can change the content of the main window.
