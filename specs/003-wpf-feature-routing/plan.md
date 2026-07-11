# Kế hoạch Triển khai: Tổ chức lại Kiến trúc WPF theo Tính năng

**Branch**: `003-wpf-feature-routing` | **Date**: 2026-07-10 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `specs/003-wpf-feature-routing/spec.md`

## Summary

Tính năng này nhằm tổ chức lại thư mục WPF lộn xộn hiện tại thành cấu trúc dựa trên tính năng (feature-based). Mỗi tính năng (ví dụ: Ngân sách, Giao dịch) sẽ có thư mục riêng chứa tất cả các views liên quan. Đồng thời, một `MainWindow` sẽ được tạo/chỉnh sửa để hoạt động như bộ chứa định tuyến đến các views tính năng này thông qua menu điều hướng, sử dụng cơ chế thay đổi nội dung (ContentControl).

## Technical Context

**Language/Version**: C# / .NET 8 (Giả định theo codebase WPF hiện tại)

**Primary Dependencies**: WPF (Windows Presentation Foundation)

**Storage**: N/A (Chỉ thay đổi UI)

**Testing**: Kiểm thử trực quan giao diện UI.

**Target Platform**: Windows Desktop

**Project Type**: Desktop Application (WPF)

**Performance Goals**: Hiển thị mượt mà khi người dùng điều hướng giữa các màn hình.

**Constraints**: Việc chuyển đổi view phải diễn ra nhanh chóng, giao diện bị hủy (reload fresh) khi chuyển tính năng khác để tránh tốn bộ nhớ.

**Scale/Scope**: Tái cấu trúc lại toàn bộ các views hiện tại (UserControls) vào thư mục Features.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Dự án hiện tại không có vi phạm nào đối với các nguyên tắc hiến pháp được định nghĩa trong `constitution.md`. Tính năng này tập trung vào tổ chức mã nguồn và định tuyến UI, tuân thủ các chuẩn mực phát triển WPF mà không phá vỡ quy trình hiện tại.

## Project Structure

### Documentation (this feature)

```text
specs/003-wpf-feature-routing/
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
│   │   ├── BudgetView.xaml
│   │   └── ...
│   ├── Transactions/
│   │   └── TransactionsView.xaml
│   ├── Dashboard/
│   │   └── ...
│   └── Settings/
│       └── ...
├── Views/ (Thư mục cũ, có thể chỉ chứa các thành phần dùng chung hoặc sẽ bị xóa)
└── MainWindow.xaml (Shell chứa Navigation và ContentControl)
```

**Structure Decision**: Chuyển đổi từ cấu trúc "Views" dạng phẳng sang cấu trúc phân cấp "Features" trong WPF project, với một `MainWindow.xaml` làm container (vỏ bọc) để chứa và định tuyến đến các UI của các tính năng.
