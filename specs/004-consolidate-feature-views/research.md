# Research & Decisions: Hợp nhất các View thành phần

## 1. Phương pháp Hợp nhất XAML (XAML Consolidation)
- **Decision**: Sao chép nội dung XAML của các UserControl phụ (như `BudgetProgressControl`) trực tiếp vào tệp XAML chính (ví dụ `BudgetsView.xaml`), thay thế các thẻ `<uc:BudgetProgressControl/>` bằng trực tiếp các thẻ XAML cấu thành nên nó.
- **Rationale**: Đảm bảo tất cả giao diện và Data Binding được duy trì, loại bỏ sự phụ thuộc vào tệp phụ, giảm số lượng tệp quản lý. Dễ dàng thực hiện do WPF cho phép gộp các layout vào chung một hệ thống Grid/StackPanel.
- **Alternatives considered**: Sử dụng `DataTemplate` cục bộ hoặc `ResourceDictionary` - Bị loại vì không giảm số tệp tin thực sự và làm tăng độ phức tạp của resources.

## 2. Xử lý Code-behind
- **Decision**: Gộp toàn bộ event handlers (nếu có) từ code-behind của tệp phụ sang tệp chính.
- **Rationale**: Duy trì hành vi hiện tại của UI, mặc dù trong mô hình MVVM, code-behind thường chỉ chứa `InitializeComponent()`.
- **Alternatives considered**: Xóa luôn code-behind phụ - Tốt nhưng có thể gây lỗi biên dịch nếu có logic UI cụ thể (animation, focus, etc.).

## 3. Quy chuẩn Đặt tên (Naming Convention)
- **Decision**: Loại bỏ hậu tố `Control` ở tất cả các view. Ví dụ: `BudgetsViewControl` trở thành `BudgetsView`.
- **Rationale**: Tuân thủ chuẩn chung, tên ngắn gọn, dễ gọi từ ViewModel (e.g. `Features.Budget.BudgetsView`).
- **Alternatives considered**: Giữ nguyên tên hiện tại - Bị loại do user story US2 yêu cầu phải đổi tên.
