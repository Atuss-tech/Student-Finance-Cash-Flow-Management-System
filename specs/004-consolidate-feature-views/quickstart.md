# Quickstart & Validation Guide: Hợp nhất các View thành phần

Hướng dẫn này giúp xác minh rằng tất cả các View thành phần đã được hợp nhất thành công và ứng dụng không bị mất bất kỳ chi tiết giao diện nào.

## 1. Prerequisites

- Mã nguồn đã được cấu trúc lại hoàn toàn (Phase 1, 2)
- Có .NET 8 SDK cài đặt trên máy.

## 2. Validation: Biên dịch ứng dụng (Compile Check)

**Mục đích**: Xác nhận không có lỗi về class name, thiếu thư viện hay thiếu references.

**Lệnh thực thi**:
```powershell
dotnet build WPF/WPF.csproj
```

**Kết quả mong đợi**:
- Build thành công `0 Error(s)`.

## 3. Validation: Kiểm tra Giao diện (UI Check)

**Mục đích**: Chạy ứng dụng và xem bằng mắt thường rằng giao diện không bị hỏng sau khi hợp nhất XAML.

**Thao tác**:
1. Khởi chạy ứng dụng: `dotnet run --project WPF/WPF.csproj`
2. Đăng nhập vào màn hình chính.
3. Chuyển đổi qua lại giữa các menu (Dashboard, Budget, Transactions, v.v.).
4. Đảm bảo Dashboard vẫn hiển thị đủ các thẻ tóm tắt, biểu đồ, và danh sách giao dịch gần đây.
5. Đảm bảo màn hình Budget vẫn hiển thị đủ các thanh tiến trình ngân sách.

**Kết quả mong đợi**:
- Mọi thành phần UI hiển thị đầy đủ, không bị thiếu layout. Các thanh tiến trình/biểu đồ hoạt động và load đúng dữ liệu binding.
