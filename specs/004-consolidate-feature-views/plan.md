# Implementation Plan: Hợp nhất các View thành phần

**Branch**: `[004-consolidate-feature-views]` | **Date**: 2026-07-10 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `/specs/004-consolidate-feature-views/spec.md`

## Summary

Hợp nhất các UserControl bị phân mảnh của mỗi tính năng thành một View duy nhất (ví dụ: `BudgetProgressControl.xaml` gộp vào `BudgetsView.xaml`). Loại bỏ hậu tố `Control` để đạt sự đồng nhất.

## Technical Context

**Language/Version**: C# 12 / .NET 8.0
**Primary Dependencies**: WPF, LiveChartsCore
**Testing**: Không yêu cầu unit test đặc thù cho thay đổi này (thuần túy XAML/UI refactor)
**Target Platform**: Windows Desktop (WPF)
**Project Type**: Desktop App
**Constraints**: Bảo toàn giao diện và MVVM DataBinding hiện có

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Quy tắc Kiến trúc**: Các tính năng độc lập, cấu trúc rõ ràng. (PASS - Refactor giúp đạt được mục tiêu này tốt hơn)
- **Tách biệt Data/Logic**: Thay đổi chỉ ảnh hưởng đến XAML, không vi phạm MVVM. (PASS)

## Project Structure

### Documentation (this feature)

```text
specs/004-consolidate-feature-views/
├── plan.md              # This file (/speckit-plan command output)
├── research.md          # Phase 0 output (/speckit-plan command)
├── data-model.md        # Phase 1 output (/speckit-plan command)
├── quickstart.md        # Phase 1 output (/speckit-plan command)
└── tasks.md             # Phase 2 output (/speckit-tasks command - NOT created by /speckit-plan)
```

### Source Code (repository root)

```text
WPF/
├── Features/
│   ├── Budget/
│   │   └── BudgetsView.xaml (.cs)
│   ├── Categories/
│   │   └── CategoriesView.xaml (.cs)
│   ├── Dashboard/
│   │   └── DashboardHomeView.xaml (.cs)
│   ├── Profile/
│   │   └── ProfileView.xaml (.cs)
│   ├── Reports/
│   │   └── ReportsView.xaml (.cs)
│   ├── Transactions/
│   │   └── TransactionsView.xaml (.cs)
│   └── Wallets/
│       └── WalletsView.xaml (.cs)
├── ViewModels/
│   └── DashboardViewModel.cs
└── Views/
    └── DashboardWindow.xaml (.cs)
```

**Structure Decision**: Hợp nhất tệp tin XAML và code-behind; các control phụ bị xóa đi, view chính loại bỏ chữ `Control`.

## Stitch AI Automation Execution Strategy

Dựa trên yêu cầu đảm bảo độ chính xác tuyệt đối (giống 100% bản thiết kế FinanceOS từ Stitch AI), chiến lược thực thi XAML sẽ bao gồm:

1. **Design System**: 
   - **Màu sắc**: Trích xuất mã màu Hex từ dự án Stitch AI (Ví dụ: Background `#051424`, Surface `#122131`, Primary `#4edea3`, Secondary/Purple `#c0c1ff`, Text `#d4e4fa`).
   - **Typography**: Sử dụng font "Be Vietnam Pro" / "Inter" cho toàn bộ UI. Định nghĩa rõ các cấp độ text (Header, Title, Body, Caption) theo size từ JSON.
   - **Border Radius**: Sử dụng bo góc 8px - 12px chuẩn theo hệ thống `ROUND_EIGHT` của thiết kế.

2. **Cấu trúc Layout (DashboardWindow & Views)**:
   - **Sidebar**: Gắn cố định bên trái (chiều rộng ~240px). Nền `#051424`, các item có hover/active state bo góc đẹp mắt.
   - **Header**: Thanh tìm kiếm, nút thông báo, nút "Thêm giao dịch".
   - **DashboardHomeView**:
     - 4 thẻ thống kê (Số dư, Thu nhập, Chi tiêu, Tiết kiệm) xếp ngang.
     - Biểu đồ dòng tiền (Line chart - LiveChartsCore) và Phân bổ chi tiêu (Doughnut chart - LiveChartsCore).
     - Danh sách giao dịch gần đây & Ngân sách tháng ở dưới cùng.

3. **Pixel-Perfect Guarantee**:
   Mặc dù WPF rendering có khác biệt vi mô so với Web/React, việc mô phỏng lại 99% cấu trúc, màu sắc, font, độ đổ bóng, tỷ lệ khoảng cách (padding/margin) được cam kết thực hiện chính xác theo ảnh tải về từ Stitch.

## Open Questions
- Bạn có muốn mình tiến hành tạo và thực thi file `tasks.md` để tự động đập đi xây lại toàn bộ XAML (Dashboard, Wallet, Budget...) theo chuẩn Stitch AI ngay bây giờ luôn không?
