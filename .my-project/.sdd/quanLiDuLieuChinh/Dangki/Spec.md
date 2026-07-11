# Feature Specification: Giao diện Đăng nhập & Hiệu ứng Động Automation

## 1. Tên tính năng (Short Name)
`login-dashboard-animation`

## 2. Mục tiêu (Goals)
- Bổ sung hiệu ứng động (automation animations) cho khu vực Dashboard mô phỏng ở trang Đăng nhập / Đăng ký.
- Tạo cảm giác mượt mà, sống động ("automation effect") để thu hút người dùng ngay khi ứng dụng vừa khởi động.

## 3. Yêu cầu Hiệu ứng Động (Animation Requirements)
Khu vực Dashboard bên phải sẽ chạy hiệu ứng động ngay khi màn hình xuất hiện (`Window.Loaded`):

1. **Thẻ Số dư khả dụng (Top Card):**
   - Biểu đồ đường (Line Chart): Hiệu ứng tự động vẽ đường (sử dụng `StrokeDashArray` / `StrokeDashOffset` animation) hoặc trượt từ dưới lên kèm Fade in.
   - Các điểm dữ liệu (Data points): Phóng to (Scale) từ 0 lên 1.

2. **Thẻ Tiết kiệm (Middle Left Card):**
   - Thanh tiến trình (Progress Bar): Trượt dài từ chiều rộng `0` đến `120px` trong vòng 1-1.5 giây (Sử dụng `DoubleAnimation` trên `Width`).

3. **Thẻ Chi tiêu (Middle Right Card):**
   - Biểu đồ Donut: Hiệu ứng xoay tròn (RotateTransform từ 0 đến 360 độ) hoặc phóng to dần (ScaleTransform) và hiện rõ lên (Fade in).

4. **Thẻ Chi tiêu theo tháng (Bottom Card):**
   - Biểu đồ cột (Bar Chart): Các cột (T1 đến T6) sẽ bắt đầu với chiều cao `0` và từ từ mọc lên (tăng `Height`) đến giá trị mục tiêu (60, 70, 50, 80, 75, 100). Hiệu ứng này sẽ chạy nối tiếp hoặc so le nhau để tạo cảm giác biểu đồ đang load dữ liệu thực tế.

## 4. Ràng buộc Kỹ thuật (Technical Constraints)
- Sử dụng công cụ `Storyboard` và `DoubleAnimation` thuần túy của WPF XAML.
- Không sử dụng file GIF hay video ngoài để đảm bảo ứng dụng nhẹ, khởi động nhanh và sắc nét (Vector).
- Animation được kích hoạt qua `EventTrigger` tại sự kiện `Window.Loaded`.

## 5. Tiêu chí Thành công (Success Criteria)
- [ ] Khi khởi động app, biểu đồ cột tự động mọc lên.
- [ ] Thanh tiến trình mục tiêu tự động dài ra.
- [ ] Không gây giật lag hoặc kéo dài thời gian load form đăng nhập.
