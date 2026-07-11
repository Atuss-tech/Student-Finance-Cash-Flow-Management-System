# UI Structure Verification Guide

## Purpose
This guide helps verify that the newly implemented WPF UI perfectly matches the Stitch AI HTML design structure.

## Verification Scenarios

### Scenario 1: Validate Wallets Screen Structure
1. Launch the WPF Application: `dotnet run --project WPF`
2. Navigate to the **Ví (Wallets)** tab in the sidebar.
3. **Verify Header**: Ensure the title is "Ví" and subtitle is "Quản lý các tài khoản và nguồn tiền của bạn".
4. **Verify Summary Box**: A full-width bento box should display "Tổng số dư", "Tổng thu (Tháng này)", and "Tổng chi (Tháng này)".
5. **Verify Wallet Cards**: 
   - Each card must display a dynamic icon and color (e.g., Momo in Red, Techcombank in Blue).
   - Each card must have a sparkline (mini chart) placeholder below the balance.
6. **Verify Analytics Section**: At the bottom of the screen, there must be a split layout containing a "Dòng tiền theo ví" bar chart placeholder (taking 2/3 width) and a "Phân bổ nguồn tiền" donut chart placeholder (taking 1/3 width).

### Scenario 2: Validate Categories Screen Structure
1. Navigate to the **Danh mục (Categories)** tab.
2. **Verify Summary Bento Box**: A 3-column top section displaying "Tổng số danh mục", "Chi tiêu cao nhất", and "Sức khỏe ngân sách" with a progress bar.
3. **Verify Split Layout**:
   - The left column (2/3 width) must contain the "Danh sách phân bổ" with category cards.
   - The right column (1/3 width) must contain the "Cơ cấu chi tiêu" pie chart placeholder and "Quản lý danh mục" action box.
4. **Verify Category Cards**: Each category card in the list must display a progress bar showing spending vs limit, and a dedicated icon/color scheme.
