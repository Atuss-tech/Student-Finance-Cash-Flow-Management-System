# Hợp nhất và Hoàn thiện Màn hình Giao dịch (Transaction Management)

Màn hình Giao dịch (`TransactionsView.xaml`) hiện tại đã được cấu trúc sơ bộ theo Dark Theme ở phiên bản trước. Tuy nhiên, dựa trên thiết kế gốc từ **Stitch AI** (bản chụp giao diện Transaction Management), chúng ta cần đối chiếu và tinh chỉnh lại một số phần tử UI cho khớp hoàn toàn (pixel-perfect) hoặc bổ sung các tương tác tĩnh còn thiếu.

## User Review Required

Bạn vui lòng kiểm tra lại ảnh chụp `transactions_screen.png` mà mình vừa tải về từ hệ thống Stitch AI. 
Do màn hình `TransactionsView.xaml` hiện tại đã có một số Card như `TỔNG GIAO DỊCH`, `TỔNG THU`, `TỔNG CHI` và một biểu đồ, bạn có muốn GIỮ NGUYÊN cấu trúc này, hay muốn ĐẬP ĐI XÂY LẠI hoàn toàn theo đúng 100% bố cục của ảnh `transactions_screen.png` mới nhất từ Stitch AI?

## Open Questions

1. Có cần làm màn hình phụ/popup "Thêm giao dịch mới" (Add Transaction) ngay trong kế hoạch này không, hay chỉ cần giao diện hiển thị danh sách?
2. Biểu đồ trong tab Giao dịch có cần chuyển đổi từ cột sang dạng đường (Line chart) như một số thiết kế quản lý dòng tiền không?

## Proposed Changes

### Tính năng Giao dịch (Transactions)

#### [MODIFY] [TransactionsView.xaml](file:///F:/SU26/Prn212/Practice/Student%20Finance%20&%20Cash%20Flow%20Management%20System/WPF/Features/Transactions/TransactionsView.xaml)
- **Header & Layout**: Căn chỉnh lại padding/margin cho toàn bộ trang để khớp với Design System spacing (ví dụ: `card-gap: 20px`).
- **Thanh lọc và Tab điều hướng**: 
  - Đảm bảo các nút "Tất cả", "Thu nhập", "Chi tiêu" hiển thị rõ trạng thái Active (Màu nền ngọc bích `#10b981` opacity 10%, chữ sáng) và Inactive (chữ xám `#86948a`).
- **Danh sách Giao dịch (Data Grid / ListBox)**:
  - Tinh chỉnh khoảng cách giữa các hàng.
  - Cập nhật định dạng Icon danh mục (ví dụ: Icon có bo tròn nền phụ).
  - Cập nhật màu sắc hiển thị Số tiền: Màu xanh lá (`#4edea3`) cho Thu nhập, và Đỏ (`#ffb4ab`) cho Chi tiêu.

## Verification Plan

### Manual Verification
- Kiểm tra lại giao diện sau khi chạy `dotnet run` và so sánh trực tiếp 1-1 với ảnh chụp `transactions_screen.png` từ Stitch AI.
- Xác nhận các Tab lọc, nút chuyển đổi hoạt động (hiệu ứng hover/active UI).
