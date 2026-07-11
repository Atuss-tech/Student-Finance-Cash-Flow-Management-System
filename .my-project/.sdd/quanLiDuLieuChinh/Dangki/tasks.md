# Tasks: Hiệu ứng Động Automation cho Dashboard (WPF)

## Phase 1: Chuẩn bị XAML UI Elements (User Story 1)
- **Goal:** Đặt tên định danh (`x:Name`) và gắn `Transform` cho các thẻ UI cần tạo animation trong `Login.xaml`.
- **Independent Test Criteria:** Code XAML không báo lỗi, vẫn build được bình thường.

- [x] T001 [US1] Mở `Login.xaml`, gắn `x:Name="ProgressBarFill"` cho thẻ `Border` xanh của phần Tiến độ tiết kiệm. Đặt Width ban đầu = 0.
- [x] T002 [US1] Gắn `x:Name` từ `BarT1` đến `BarT6` cho 6 cột biểu đồ. Gắn `Height=0`. Đặt `RenderTransformOrigin="0.5,1"` để cột mọc từ dưới lên.
- [x] T003 [US1] Bọc biểu đồ Donut vào một `Grid` có `x:Name="DonutChart"`, gắn `<Grid.RenderTransform><RotateTransform Angle="0"/></Grid.RenderTransform>` và `RenderTransformOrigin="0.5,0.5"`.
- [x] T004 [US1] Bọc biểu đồ Đường (Line Chart & Ellipse) vào một `Grid` có `x:Name="LineChart"`, gắn `<Grid.RenderTransform><TranslateTransform Y="30"/></Grid.RenderTransform>`.

## Phase 2: Viết Storyboard & Triggers (User Story 2)
- **Goal:** Viết script animation (`DoubleAnimation`) chạy tự động khi Load form.
- **Independent Test Criteria:** Mở ứng dụng lên thấy chuyển động mượt mà, đúng thời gian, không lag.

- [x] T005 [US2] Thêm `<Window.Triggers><EventTrigger RoutedEvent="Window.Loaded"><BeginStoryboard><Storyboard>...` vào `Login.xaml`.
- [x] T006 [US2] Thêm `DoubleAnimation` cho `ProgressBarFill` (Width 0 -> 120, Duration 1.5s).
- [x] T007 [US2] Thêm 6 `DoubleAnimation` cho `BarT1` -> `BarT6` (Height 0 -> Target, Duration 1s, `BeginTime` lệch nhau 0.1s - 0.2s).
- [x] T008 [US2] Thêm `DoubleAnimation` xoay `DonutChart` (Angle 0 -> 360, Duration 1.5s) và làm rõ (Opacity 0 -> 1).
- [x] T009 [US2] Thêm `DoubleAnimation` trượt `LineChart` (Y 30 -> 0) và làm rõ (Opacity 0 -> 1, Duration 1s).
- [x] T010 [US2] Hoàn tất triển khai. Bạn có thể Build (F5) và nghiệm thu!

## Dependencies
- Phase 2 phụ thuộc vào Phase 1.

## Implementation Strategy
- Chỉ thay đổi trên UI Layer (`Login.xaml`), hoàn toàn không can thiệp vào logic C# (`.xaml.cs`) để đảm bảo không dính rủi ro lỗi logic nghiệp vụ.
