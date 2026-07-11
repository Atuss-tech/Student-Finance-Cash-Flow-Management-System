# Implementation Plan: Hiệu ứng Động Automation cho Dashboard (WPF)

## 1. Technical Context
- **Feature Spec**: Bổ sung hiệu ứng mượt mà (animation) cho 4 thẻ thông tin tĩnh trên màn hình Login.
- **Technologies**: XAML thuần túy, WPF (`Storyboard`, `DoubleAnimation`, `EventTrigger`).
- **Dependencies**: Không yêu cầu thư viện bên ngoài (No LiveCharts, No GIF).

## 2. Phase 0: Research & Outline
- `research.md`: Không yêu cầu nghiên cứu thêm vì phương án WPF DoubleAnimation đã được thống nhất là tối ưu nhất.

## 3. Phase 1: Design
### Data Model & Contracts
- Không áp dụng, tính năng này hoàn toàn nằm ở Frontend (XAML).

### Technical Design (XAML)
1. **Thanh Tiến trình (Tiết kiệm):**
   - Dùng `DoubleAnimation` biến thiên `Width` của thẻ `Border` từ 0 -> 120. `Duration = 0:0:1.5`.

2. **Biểu đồ Cột (Chi tiêu theo tháng):**
   - Đặt `x:Name` từ `BarT1` -> `BarT6`.
   - Dùng `DoubleAnimation` thay đổi `Height`. Dùng thuộc tính `BeginTime` tịnh tiến: 0s, 0.2s, 0.4s... để tạo hiệu ứng mọc lên.

3. **Biểu đồ Tròn (Chi tiêu):**
   - Đặt biểu đồ vào thẻ `Grid`. Dùng `RotateTransform.Angle` từ 0 -> 360 trong 2s.

4. **Biểu đồ Đường (Số dư khả dụng):**
   - Dùng `DoubleAnimation` trên `TranslateTransform.Y` từ 30 -> 0 kết hợp `Opacity` từ 0 -> 1.

## 4. Phase 2: Quickstart / Verification
- Bước 1: Mở dự án `WPF.csproj`.
- Bước 2: Chạy `dotnet run`.
- Bước 3: Quan sát ngay lúc form hiển thị (`Window.Loaded`), tất cả hiệu ứng sẽ được kích hoạt tuần tự.
