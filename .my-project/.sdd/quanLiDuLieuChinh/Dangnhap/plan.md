# Cập nhật Plan: Điều chỉnh Giao diện Đăng Nhập & Nút Mắt Mật Khẩu

## 1. Technical Context
- **Vấn đề 1 (Nút Đăng nhập):** Trong giao diện hiện tại, nút "Đăng nhập" bị ép sang lề phải do chia không gian với nút Google, làm thiết kế trông mất cân đối.
- **Vấn đề 2 (Mắt Mật Khẩu):** Trạng thái mặc định đang bị ngược. Mặc định mật khẩu bị ẩn, nhưng icon lại hiển thị "mắt mở" (không có vạch chéo). Animation có giá trị `StrokeDashOffset` chưa đồng bộ chính xác với chiều dài của Path (cần là `25` thay vì `100`).

## 2. Phase 1: Thay đổi Giao diện XAML (Login.xaml)
- **Nút Đăng nhập:**
  - Bỏ chia cột (Grid.ColumnDefinitions) ở khu vực chứa nút Đăng nhập.
  - Ẩn/xóa nút Google (hoặc đưa xuống dưới nếu cần). Tạm thời sẽ cho nút "Đăng nhập" trải rộng `HorizontalAlignment="Stretch"` toàn bộ chiều ngang để tạo sự cân bằng và dễ nhìn.
- **Nút Mắt Mật khẩu (Login & Register):**
  - Đổi giá trị mặc định của `StrokeDashOffset` thành `0` để ngay khi mở app, con mắt đã bị **gạch chéo** (vì mật khẩu đang ẩn).
  - Giá trị `StrokeDashArray` và `StrokeDashOffset` nên để `25` (chiều dài xấp xỉ của đoạn thẳng `M 2,2 L 18,18` là $\sqrt{16^2+16^2} \approx 22.6$, do đó dùng 25 là hợp lý).

## 3. Phase 2: Thay đổi Code-Behind Animation (Login.xaml.cs)
- Cập nhật tham số của `DoubleAnimation` cho đường gạch chéo (`SlashAnim`):
  - Khi `isVisible == true` (Xem mật khẩu): `To = 25` (để đường gạch chéo biến mất).
  - Khi `isVisible == false` (Ẩn mật khẩu): `To = 0` (để đường chéo vẽ ra che con mắt).
- Áp dụng cho cả 2 hàm `btnToggleLoginPassword_Click` và `btnToggleRegisterPassword_Click`.

## 4. Phase 3: Verification
- Chạy lại ứng dụng (F5).
- Kiểm tra nút Đăng nhập đã rộng ra và cân đối chưa.
- Kiểm tra nút mắt: Lúc mới mở thì mắt bị gạch chéo -> Bấm vào để xem -> Vạch chéo mờ đi (mở mắt) -> Bấm lần nữa -> Vẽ vạch chéo (đóng mắt).
