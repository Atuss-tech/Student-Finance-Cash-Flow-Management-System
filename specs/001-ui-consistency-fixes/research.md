# UI Structure Research

## Clarifications Resolved

**Gap Analysis: WPF vs Stitch AI HTML**

1. **Wallets Screen (`590949d4b5654da3977d6f8038165756`)**:
   - *Current WPF*: Just a list of wallet cards with a generic bank icon.
   - *Stitch AI*: Includes a Summary section (Total Balance, Total Income, Total Expense), specific wallet icons/colors (e.g. Red for Momo), sparkline charts on each card, a "Add New Wallet" dashed card, and two charts at the bottom ("Dòng tiền theo ví" bar chart, "Phân bổ nguồn tiền" donut chart).
   - *Decision*: We need to update `WalletsViewControl.xaml` to add a top Grid for the Summary, update the `ItemTemplate` to support dynamic icons/colors, and add the bottom charts section (using LiveChartsCore).

2. **Categories Screen (`a96b6cd04aa0464398a25607b04bd7ea`)**:
   - *Current WPF*: Simple WrapPanel of Categories.
   - *Stitch AI*: Includes a Bento-box style Summary section (Total Categories, Most Spent, Budget Health). A Split Layout with "Danh sách phân bổ" on the left and "Cơ cấu chi tiêu" (Pie Chart) on the right. Category cards have progress bars.
   - *Decision*: We need to completely rewrite the XAML layout in `CategoriesViewControl.xaml` to match this 2-column split layout and add progress bars to the category item template.

3. **General Layout**:
   - *Stitch AI*: Has a Sidebar (240px width) and a TopAppBar.
   - *WPF App*: The Sidebar is currently handled by `MainWindow.xaml` or a main shell view.
   - *Decision*: We will focus on matching the *content area* of the screens (everything to the right of the Sidebar and below the TopAppBar) within our `UserControl`s.

## Design Decisions

- **Layout Structure**: We will use `Grid` heavily to recreate the Bento box and Split layouts.
- **Charts**: We will use `LiveChartsCore.SkiaSharpView.WPF` placeholders to represent the visual charts shown in the HTML designs.
- **Icons**: We will map the Material Symbols from the HTML to standard text emojis or specific paths if necessary, but keep the structure identical.
