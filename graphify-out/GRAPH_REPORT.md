# Graph Report - .  (2026-07-29)

## Corpus Check
- Corpus is ~33,264 words - fits in a single context window. You may not need a graph.

## Summary
- 949 nodes · 1603 edges · 37 communities (32 shown, 5 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 84 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Budget Module
- Namespace:09a52be9f76e28b7 Module
- Datetime Module
- Category Module
- Financetransaction Module
- Namespace:027118b4671f3eb6 Module
- Businessobjects Module
- Budgethealthpercentage Module
- Wallet Module
- Formattedvalue Module
- List Module
- Cashflowbywalletseries Module
- Datetime Module
- Axis Module
- Barwidth Module
- User Module
- Allbudgets Module
- Avatarinitials Module
- List Module
- Budgetprogresses Module
- Addbudgetwindow Module
- Axis Module
- Uidata Module
- Alltransactions Module
- Ireportservice Module
- Bool Module
- Axis Module
- Axis Module
- List Module
- Brush Module
- Decimal Module
- Spendinggroupseries Module
- Indicator Module
- Indicator Module
- Mousebuttoneventargs Module
- Border Module

## God Nodes (most connected - your core abstractions)
1. `UserControl` - 49 edges
2. `FinanceTransaction` - 44 edges
3. `TransactionsView` - 40 edges
4. `BusinessObjects.Models` - 33 edges
5. `Category` - 33 edges
6. `UserControl` - 32 edges
7. `UserControl` - 31 edges
8. `CategoriesView` - 30 edges
9. `Wallet` - 24 edges
10. `TransactionService` - 24 edges

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
  Services/BudgetService.cs → Repositories/ITransactionRepository.cs

## Import Cycles
- None detected.

## Communities (37 total, 5 thin omitted)

### Community 0 - "Budget Module"
Cohesion: 0.05
Nodes (38): Budget, DateTime, Lazy, List, Task, BudgetDAO, List, Task (+30 more)

### Community 1 - "Namespace:09a52be9f76e28b7 Module"
Cohesion: 0.07
Nodes (15): WPF.Features.Categories, WPF.Features.Reports, BusinessObjects.Models, WPF.Features.Wallets, Services, DataAccess, Student_Finance___Cash_Flow_Management_System, WPF.Features.Dashboard (+7 more)

### Community 2 - "Datetime Module"
Cohesion: 0.08
Nodes (16): DateTime, List, Task, ITransactionRepository, Balance, Dictionary, Month, Task (+8 more)

### Community 3 - "Category Module"
Cohesion: 0.08
Nodes (11): Category, DateTime, ICollection, List, CategoryDAO, List, CategoryRepository, List (+3 more)

### Community 4 - "Financetransaction Module"
Cohesion: 0.07
Nodes (15): FinanceTransaction, DateTime, DateTime, List, Task, TransactionDAO, DateOnly, DateTime (+7 more)

### Community 5 - "Namespace:027118b4671f3eb6 Module"
Cohesion: 0.07
Nodes (34): WPF.Common, Window, CancelBtn, ConfirmBtn, HighlightBorder, HighlightTextBlock, IconBadge, IconText (+26 more)

### Community 6 - "Businessobjects Module"
Cohesion: 0.05
Nodes (44): net8.0, Microsoft.EntityFrameworkCore (9.0.17), Microsoft.EntityFrameworkCore.Design (9.0.17), Microsoft.EntityFrameworkCore.SqlServer (9.0.17), Microsoft.EntityFrameworkCore.Tools (9.0.17), Microsoft.Extensions.Configuration (9.0.17), Microsoft.Extensions.Configuration.Json (9.0.17), Microsoft.NET.Sdk (+36 more)

### Community 7 - "Budgethealthpercentage Module"
Cohesion: 0.07
Nodes (26): BudgetHealthPercentage, BudgetLimit, BudgetLimitText, Categories, CategoryBreakdownSeries, CurrentSpendingText, HighestSpendingAmount, HighestSpendingCategory (+18 more)

### Community 8 - "Wallet Module"
Cohesion: 0.07
Nodes (12): Wallet, DateTime, ICollection, List, WalletDAO, List, IWalletRepository, List (+4 more)

### Community 9 - "Formattedvalue Module"
Cohesion: 0.05
Nodes (40): CardExpense.FormattedValue, CardIncome.FormattedValue, CardTotal.FormattedValue, CategoryPills, ChartXAxes, ChartYAxes, Date, DateLabel (+32 more)

### Community 10 - "List Module"
Cohesion: 0.07
Nodes (25): List, ICategoryService, CategoryNameTextBox, ExpenseRadio, IncomeRadio, NoteTextBox, Window, MouseButtonEventArgs (+17 more)

### Community 11 - "Cashflowbywalletseries Module"
Cohesion: 0.07
Nodes (27): CashFlowByWalletSeries, FormattedBalance, FormattedTotalBalance, IconBackgroundBrush, IconForegroundBrush, IconText, LegendTextPaint, StatusBackgroundBrush (+19 more)

### Community 12 - "Datetime Module"
Cohesion: 0.10
Nodes (20): DateTime, AmountTextBox, CategoryComboBox, ExpenseRadio, IncomeRadio, NoteTextBox, SaveButton, TitleTextBlock (+12 more)

### Community 13 - "Axis Module"
Cohesion: 0.12
Nodes (9): Axis, int, ISeries, List, RoutedEventArgs, SolidColorPaint, string, Task (+1 more)

### Community 14 - "Barwidth Module"
Cohesion: 0.07
Nodes (28): BarWidth, CashFlowTrendSeries, ColorBrush, DetailTransactions, LightBrush, NetCashFlowReport.FormattedValue, NetCashFlowReport.Subtext, NetCashFlowReport.Title (+20 more)

### Community 15 - "User Module"
Cohesion: 0.10
Nodes (14): User, DateTime, ICollection, StudentFinanceDbContext, Lazy, UserDAO, DbContext, DbContextOptionsBuilder (+6 more)

### Community 16 - "Allbudgets Module"
Cohesion: 0.07
Nodes (27): AllBudgets, AreaChartSeries, AreaChartXAxes, AreaChartYAxes, BarChartSeries, BarChartXAxes, BarChartYAxes, CardRemaining.FormattedValue (+19 more)

### Community 17 - "Avatarinitials Module"
Cohesion: 0.09
Nodes (21): AvatarInitials, ChangePasswordCommand, Email, FullName, IsAlertError, JoinDate, Phone, SubscriptionPlan (+13 more)

### Community 18 - "List Module"
Cohesion: 0.11
Nodes (19): List, IWalletService, BalanceLabelBlock, BalanceTextBox, DeleteButton, NoteTextBox, SaveButton, SubtitleBlock (+11 more)

### Community 19 - "Budgetprogresses Module"
Cohesion: 0.10
Nodes (20): BudgetProgresses, DateString, FormattedSpent, FormattedTotal, RecentTransactions, XAxes, YAxes, UserControl (+12 more)

### Community 20 - "Addbudgetwindow Module"
Cohesion: 0.12
Nodes (15): AmountTextBox, CategoryComboBox, MonthComboBox, NoteTextBox, TitleBlock, Window, BudgetData, int (+7 more)

### Community 21 - "Axis Module"
Cohesion: 0.18
Nodes (10): Axis, bool, double, ISeries, ObservableCollection, RoutedEventArgs, SolidColorPaint, string (+2 more)

### Community 22 - "Uidata Module"
Cohesion: 0.15
Nodes (8): decimal, Dictionary, SolidColorBrush, string, BudgetData, CategoryIconHelper, DashboardSummaryData, WalletData

### Community 23 - "Alltransactions Module"
Cohesion: 0.14
Nodes (12): AllTransactions, Window, AmountColorBrush, Category, FormattedAmount, FormattedDate, Icon, IconBackground (+4 more)

### Community 24 - "Ireportservice Module"
Cohesion: 0.22
Nodes (8): Balance, Dictionary, Month, Task, TotalExpense, TotalIncome, Year, IReportService

### Community 25 - "Bool Module"
Cohesion: 0.15
Nodes (9): bool, Brush, decimal, ObservableCollection, CategoryPillModel, TransactionGroupModel, TxStatCardModel, DateTime (+1 more)

### Community 26 - "Axis Module"
Cohesion: 0.18
Nodes (10): Axis, DateTime, ISeries, ObservableCollection, SolidColorBrush, SolidColorPaint, string, ReportPeriod (+2 more)

### Community 27 - "Axis Module"
Cohesion: 0.22
Nodes (6): Axis, ISeries, ObservableCollection, RoutedEventArgs, SolidColorPaint, DashboardHomeView

### Community 28 - "List Module"
Cohesion: 0.38
Nodes (3): List, RoutedEventArgs, Task

### Community 29 - "Brush Module"
Cohesion: 0.40
Nodes (3): Brush, decimal, BudgetStatCardModel

### Community 31 - "Spendinggroupseries Module"
Cohesion: 0.67
Nodes (3): SpendingGroupSeries, SpendingGroupChart, PieChart

## Knowledge Gaps
- **223 isolated node(s):** `net8.0`, `Microsoft.EntityFrameworkCore (9.0.17)`, `Microsoft.EntityFrameworkCore.Design (9.0.17)`, `Microsoft.EntityFrameworkCore.SqlServer (9.0.17)`, `Microsoft.EntityFrameworkCore.Tools (9.0.17)` (+218 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **5 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `TransactionsView` connect `Axis Module` to `Namespace:09a52be9f76e28b7 Module`, `Financetransaction Module`, `Budgethealthpercentage Module`, `Formattedvalue Module`, `List Module`, `Avatarinitials Module`, `List Module`, `Bool Module`?**
  _High betweenness centrality (0.151) - this node is a cross-community bridge._
- **Why does `CategoriesView` connect `Budgethealthpercentage Module` to `Ireportservice Module`, `Avatarinitials Module`, `Datetime Module`, `List Module`?**
  _High betweenness centrality (0.111) - this node is a cross-community bridge._
- **Why does `FinanceTransaction` connect `Financetransaction Module` to `Datetime Module`, `Category Module`, `Wallet Module`, `User Module`, `List Module`?**
  _High betweenness centrality (0.111) - this node is a cross-community bridge._
- **What connects `net8.0`, `Microsoft.EntityFrameworkCore (9.0.17)`, `Microsoft.EntityFrameworkCore.Design (9.0.17)` to the rest of the system?**
  _223 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Budget Module` be split into smaller, more focused modules?**
  _Cohesion score 0.052403846153846155 - nodes in this community are weakly interconnected._
- **Should `Namespace:09a52be9f76e28b7 Module` be split into smaller, more focused modules?**
  _Cohesion score 0.0734006734006734 - nodes in this community are weakly interconnected._
- **Should `Datetime Module` be split into smaller, more focused modules?**
  _Cohesion score 0.07686932215234102 - nodes in this community are weakly interconnected._