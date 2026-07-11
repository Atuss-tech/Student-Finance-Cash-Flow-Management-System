# Hướng dẫn Kiểm tra Nhanh: Kiến trúc WPF theo Tính năng

Tính năng này là tái cấu trúc giao diện ứng dụng WPF, không phải là API. Cách xác thực:

1. **Kiểm tra biên dịch**:
   - Mở Terminal/Command Prompt trong thư mục gốc.
   - Chạy `dotnet build`.
   - Kết quả mong đợi: Biên dịch thành công, không có lỗi "Missing view" hoặc "Cannot resolve XAML".

2. **Kiểm tra cấu trúc thư mục**:
   - Mở dự án trong Visual Studio hoặc File Explorer.
   - Điều hướng tới thư mục `WPF/Features/`.
   - Kiểm tra xem có các thư mục tính năng chuyên biệt (ví dụ: `Budget`, `Transactions`, `Dashboard`) và chứa các views tương ứng bên trong hay không.

3. **Kiểm tra ứng dụng thực tế**:
   - Khởi chạy ứng dụng bằng Visual Studio hoặc lệnh `dotnet run --project WPF`.
   - Kiểm tra Main Window hiển thị bình thường.
   - Nhấn vào các mục điều hướng ở thanh menu bên trái/trên (Ví dụ: Ngân sách, Giao dịch).
   - Đảm bảo vùng nội dung thay đổi tương ứng theo từng tính năng được chọn mà ứng dụng không bị crash.
