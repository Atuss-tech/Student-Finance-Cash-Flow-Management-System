# Research: Navigate to Transactions from Dashboard

## 1. How to navigate between views in the current architecture?
- **Decision**: Use a standard .NET `event EventHandler NavigateToTransactionsRequested;` exposed by the `DashboardHomeView` user control, and subscribe to it in `MainWindow.xaml.cs`.
- **Rationale**: Currently, view switching is handled in `MainWindow.xaml.cs` by listening to `RadioButton.Checked` events (e.g., `Nav_Transactions_Checked`) and calling `SetView()`. The `DashboardHomeView` cannot directly access `MainWindow` or `MainContentControl`. Exposing an event is the most idiomatic WPF way to bubble up an action from a UserControl to its hosting Window without tightly coupling them or introducing an EventAggregator/Messenger pattern.
- **Alternatives considered**: Passing a delegate `Action` to the `DashboardHomeView` constructor. While simple, events are more standard in WPF for UserControl interactions. Using an MVVM framework (like Prism) was rejected as it's overkill for this small navigation task.

## 2. Handling the "Xem tất cả" button click
- **Decision**: Replace `Command="{Binding ShowTransactionsCommand}"` with a standard `Click="ViewAllTransactions_Click"` event handler in `DashboardHomeView.xaml`.
- **Rationale**: Since we are using code-behind events to communicate with MainWindow, handling the button click directly in `DashboardHomeView.xaml.cs` allows us to simply invoke the `NavigateToTransactionsRequested` event. The current `ShowTransactionsCommand` is undefined and non-functional.
- **Alternatives considered**: Implementing an `ICommand` in `DashboardHomeView.cs` that triggers the event. This is also valid, but slightly more boilerplate than a simple Click handler for a purely UI-driven navigation action.
