# Graph Report - .  (2026-07-11)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 708 nodes · 1177 edges · 23 communities (22 shown, 1 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 57 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `f96e9918`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- FinanceTransaction
- Category
- Wallet
- Budget
- BusinessObjects.csproj
- DashboardHomeView
- BusinessObjects.Models
- UserControl
- UserControl
- UserControl
- User
- UserControl
- UserControl
- TransactionsView
- UserControl
- Window
- BudgetsView
- ReportsView
- .GetMonthlyReportAsync
- .GetMonthlyReportAsync
- TxStatCardModel
- PART_Indicator

## God Nodes (most connected - your core abstractions)
1. `FinanceTransaction` - 43 edges
2. `UserControl` - 42 edges
3. `UserControl` - 37 edges
4. `TransactionsView` - 36 edges
5. `BusinessObjects.Models` - 31 edges
6. `UserControl` - 31 edges
7. `Category` - 28 edges
8. `Wallet` - 24 edges
9. `TransactionService` - 24 edges
10. `UserControl` - 23 edges

## Surprising Connections (you probably didn't know these)
- `StudentFinanceDbContext` --references--> `Budget`  [EXTRACTED]
  DataAccess/StudentFinanceDbContext.cs → BusinessObjects/Models/Budget.cs
- `StudentFinanceDbContext` --references--> `Category`  [EXTRACTED]
  DataAccess/StudentFinanceDbContext.cs → BusinessObjects/Models/Category.cs
- `StudentFinanceDbContext` --references--> `FinanceTransaction`  [EXTRACTED]
  DataAccess/StudentFinanceDbContext.cs → BusinessObjects/Models/FinanceTransaction.cs
- `StudentFinanceDbContext` --references--> `Wallet`  [EXTRACTED]
  DataAccess/StudentFinanceDbContext.cs → BusinessObjects/Models/Wallet.cs
- `BudgetService` --references--> `ITransactionRepository`  [EXTRACTED]
  Services/BugetService.cs → Repositories/ITransactionRepository.cs

## Import Cycles
- None detected.

## Communities (23 total, 1 thin omitted)

### Community 0 - "FinanceTransaction"
Cohesion: 0.06
Nodes (19): FinanceTransaction, DateTime, DateTime, List, Task, TransactionDAO, DateOnly, DateTime (+11 more)

### Community 1 - "Category"
Cohesion: 0.06
Nodes (13): Category, DateTime, ICollection, List, CategoryDAO, List, CategoryRepository, List (+5 more)

### Community 2 - "Wallet"
Cohesion: 0.06
Nodes (14): Wallet, DateTime, ICollection, List, WalletDAO, List, IWalletRepository, List (+6 more)

### Community 3 - "Budget"
Cohesion: 0.07
Nodes (28): Budget, DateTime, Lazy, List, Task, BudgetDAO, List, Task (+20 more)

### Community 4 - "BusinessObjects.csproj"
Cohesion: 0.05
Nodes (43): net8.0, Microsoft.EntityFrameworkCore (9.0.17), Microsoft.EntityFrameworkCore.Design (9.0.17), Microsoft.EntityFrameworkCore.SqlServer (9.0.17), Microsoft.EntityFrameworkCore.Tools (9.0.17), Microsoft.Extensions.Configuration (9.0.17), Microsoft.Extensions.Configuration.Json (9.0.17), Microsoft.NET.Sdk (+35 more)

### Community 5 - "DashboardHomeView"
Cohesion: 0.06
Nodes (22): INotifyPropertyChanged, DateTime, List, Task, ITransactionService, Brush, decimal, BudgetStatCardModel (+14 more)

### Community 6 - "BusinessObjects.Models"
Cohesion: 0.09
Nodes (12): WPF.Features.Reports, BusinessObjects.Models, WPF.Models, Services, DataAccess, Student_Finance___Cash_Flow_Management_System, WPF.Features.Dashboard, Repositories (+4 more)

### Community 7 - "UserControl"
Cohesion: 0.05
Nodes (37): CardExpense.FormattedValue, CardExpense.Icon, CardIncome.FormattedValue, CardIncome.Icon, CardTotal.FormattedValue, CardTotal.Icon, CategoryPills, ChartXAxes (+29 more)

### Community 8 - "UserControl"
Cohesion: 0.06
Nodes (28): AvatarInitials, CashFlowByWalletSeries, ChangePasswordCommand, Email, FullName, IsAlertError, JoinDate, LegendTextPaint (+20 more)

### Community 9 - "UserControl"
Cohesion: 0.06
Nodes (32): AllBudgets, AreaChartSeries, AreaChartXAxes, AreaChartYAxes, BarChartSeries, BarChartXAxes, BarChartYAxes, CardRemaining.FormattedValue (+24 more)

### Community 10 - "User"
Cohesion: 0.10
Nodes (14): User, DateTime, ICollection, StudentFinanceDbContext, Lazy, UserDAO, DbContext, DbContextOptionsBuilder (+6 more)

### Community 11 - "UserControl"
Cohesion: 0.08
Nodes (21): BudgetHealthPercentage, BudgetLimit, BudgetLimitText, Categories, CategoryBreakdownSeries, CurrentSpendingText, HighestSpendingAmount, HighestSpendingCategory (+13 more)

### Community 12 - "UserControl"
Cohesion: 0.07
Nodes (27): CashFlowTrendSeries, CashFlowYAxes, DetailTransactions, FormattedDate, HasData, NetCashFlowReport.FormattedValue, NetCashFlowReport.Icon, NetCashFlowReport.Subtext (+19 more)

### Community 13 - "TransactionsView"
Cohesion: 0.13
Nodes (8): int, Axis, ISeries, List, RoutedEventArgs, SolidColorPaint, string, TransactionsView

### Community 14 - "UserControl"
Cohesion: 0.08
Nodes (23): BudgetProgresses, DateString, RecentTransactions, ShowTransactionsCommand, XAxes, YAxes, PART_Indicator, UserControl (+15 more)

### Community 15 - "Window"
Cohesion: 0.22
Nodes (10): ActiveIndicator, MainContentControl, PageTitle, Window, RoutedEventArgs, UserControl, MainWindow, Border (+2 more)

### Community 16 - "BudgetsView"
Cohesion: 0.19
Nodes (9): Axis, bool, ISeries, ObservableCollection, RoutedEventArgs, SolidColorPaint, string, Task (+1 more)

### Community 17 - "ReportsView"
Cohesion: 0.19
Nodes (8): Axis, ISeries, ObservableCollection, RoutedEventArgs, SolidColorPaint, string, Task, ReportsView

### Community 18 - ".GetMonthlyReportAsync"
Cohesion: 0.21
Nodes (8): Balance, Dictionary, Month, Task, TotalExpense, TotalIncome, Year, IReportService

### Community 19 - ".GetMonthlyReportAsync"
Cohesion: 0.24
Nodes (8): Balance, Dictionary, Month, Task, TotalExpense, TotalIncome, Year, ReportService

### Community 20 - "TxStatCardModel"
Cohesion: 0.20
Nodes (7): bool, Brush, decimal, ObservableCollection, CategoryPillModel, TransactionGroupModel, TxStatCardModel

## Knowledge Gaps
- **191 isolated node(s):** `net8.0`, `Microsoft.EntityFrameworkCore (9.0.17)`, `Microsoft.EntityFrameworkCore.Design (9.0.17)`, `Microsoft.EntityFrameworkCore.SqlServer (9.0.17)`, `Microsoft.EntityFrameworkCore.Tools (9.0.17)` (+186 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **1 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `TransactionsView` connect `TransactionsView` to `DashboardHomeView`, `BusinessObjects.Models`, `UserControl`, `UserControl`, `TxStatCardModel`?**
  _High betweenness centrality (0.183) - this node is a cross-community bridge._
- **Why does `ITransactionService` connect `DashboardHomeView` to `FinanceTransaction`, `ReportsView`, `TransactionsView`, `BusinessObjects.Models`?**
  _High betweenness centrality (0.165) - this node is a cross-community bridge._
- **Why does `FinanceTransaction` connect `FinanceTransaction` to `Category`, `User`, `Wallet`, `DashboardHomeView`?**
  _High betweenness centrality (0.162) - this node is a cross-community bridge._
- **What connects `net8.0`, `Microsoft.EntityFrameworkCore (9.0.17)`, `Microsoft.EntityFrameworkCore.Design (9.0.17)` to the rest of the system?**
  _191 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `FinanceTransaction` be split into smaller, more focused modules?**
  _Cohesion score 0.05661005661005661 - nodes in this community are weakly interconnected._
- **Should `Category` be split into smaller, more focused modules?**
  _Cohesion score 0.06428571428571428 - nodes in this community are weakly interconnected._
- **Should `Wallet` be split into smaller, more focused modules?**
  _Cohesion score 0.05878084179970972 - nodes in this community are weakly interconnected._