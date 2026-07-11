# Đặc tả Tính năng: Tổ chức lại Kiến trúc WPF theo Tính năng

**Feature Branch**: `003-wpf-feature-routing`

**Created**: 2026-07-10

**Status**: Draft

**Input**: User description: "bây giờ mình thấy folder WPF nó rất loạn bây giờ mình muốn view thì giao diện nào thì ra giao diện đấy, ví dụ giao diện tính năng ngân sách vậy trong giao diện sách có những màn hình nào đều nằm ở trong ngân sách, tương tự các tính năng khác cũng vậy ? khi mà thiết kế xong từng tính năng ý thì có 1 class main window là tổng hợp lại các tính năng theo đường là xong nó sẽ có bản hoàn chỉnh nhất."

## Kịch bản Người dùng & Kiểm thử *(bắt buộc)*

### Kịch bản 1 - Tổ chức thư mục theo tính năng (Độ ưu tiên: P1)

Các lập trình viên có thể dễ dàng tìm thấy các màn hình liên quan đến một tính năng cụ thể vì chúng được nhóm lại trong một thư mục tính năng riêng biệt (ví dụ: Ngân sách, Giao dịch, Cài đặt) thay vì nằm rải rác trong một thư mục "Views" phẳng.

**Lý do ưu tiên**: Đây là yêu cầu cốt lõi để giải quyết vấn đề "thư mục lộn xộn".

**Kiểm thử độc lập**: Có thể kiểm thử đầy đủ bằng cách kiểm tra trực quan cấu trúc thư mục của giải pháp và đảm bảo tất cả các view biên dịch thành công.

**Tiêu chí chấp nhận**:

1. **Cho trước** một cấu trúc thư mục WPF lộn xộn, **Khi** lập trình viên tổ chức lại các views theo tính năng, **Thì** mỗi tính năng có thư mục riêng chứa tất cả các views liên quan.
2. **Cho trước** các thư mục dựa trên tính năng, **Khi** biên dịch dự án (build), **Thì** việc biên dịch thành công mà không có lỗi thiếu tham chiếu view nào.

---

### Kịch bản 2 - Tổng hợp Tính năng & Định tuyến qua Main Window (Độ ưu tiên: P1)

Người dùng có thể điều hướng giữa các tính năng khác nhau bằng cách sử dụng một Main Window (cửa sổ chính) duy nhất đóng vai trò như một bộ chứa (container), định tuyến đến các màn hình của từng tính năng một cách linh hoạt.

**Lý do ưu tiên**: Cung cấp phiên bản "hoàn chỉnh nhất" theo yêu cầu của người dùng, biến các tính năng rời rạc thành một ứng dụng gắn kết.

**Kiểm thử độc lập**: Có thể kiểm thử đầy đủ bằng cách khởi chạy Main Window và nhấp vào các liên kết điều hướng để chuyển đổi giữa các tính năng.

**Tiêu chí chấp nhận**:

1. **Cho trước** Main Window đang mở, **Khi** người dùng nhấp vào nút điều hướng "Ngân sách", **Thì** giao diện Ngân sách được hiển thị bên trong bộ chứa của Main Window.
2. **Cho trước** Main Window đang hiển thị giao diện Ngân sách, **Khi** người dùng nhấp vào nút điều hướng "Giao dịch", **Thì** giao diện Giao dịch sẽ thay thế giao diện Ngân sách.

## Yêu cầu *(bắt buộc)*

### Yêu cầu chức năng

- **FR-001**: Hệ thống PHẢI tổ chức các WPF Views và UserControls vào các thư mục dành riêng cho tính năng (ví dụ: `WPF/Features/Budget/`, `WPF/Features/Transactions/`).
- **FR-002**: Hệ thống PHẢI có một `MainWindow` trung tâm đóng vai trò là vỏ/bộ chứa cho ứng dụng.
- **FR-003**: Hệ thống PHẢI cung cấp cơ chế điều hướng (ví dụ: thanh bên hoặc menu trong `MainWindow`) để chuyển đổi giữa các tính năng.
- **FR-004**: Hệ thống PHẢI tải động và hủy tải các giao diện tính năng trong khu vực nội dung của `MainWindow` dựa trên điều hướng.
- **FR-005**: Hệ thống PHẢI tải lại giao diện ở trạng thái mới nhất khi người dùng chuyển đổi tính năng để đảm bảo tính nhất quán (Reload fresh).

### Thực thể chính

- **MainWindow**: Cửa sổ ứng dụng chính chứa menu điều hướng và vùng nội dung hiển thị các tính năng.
- **Feature View**: Một `UserControl` đại diện cho giao diện chính cho một tính năng cụ thể (ví dụ: `BudgetView`, `DashboardView`).

## Tiêu chí Thành công *(bắt buộc)*

### Các kết quả có thể đo lường

- **SC-001**: 100% các views hiện tại được di chuyển vào các thư mục tính năng tương ứng.
- **SC-002**: Ứng dụng biên dịch và chạy thành công sau khi tổ chức lại thư mục.
- **SC-003**: Người dùng có thể điều hướng thành công đến ít nhất hai tính năng khác nhau từ Main Window mà không gặp lỗi.

## Các Giả định

- Mã nguồn hiện tại sử dụng tiêu chuẩn điều hướng của WPF (ví dụ: ContentControl với DataTemplates hoặc định tuyến Frame). Giả định sử dụng phương pháp chuyển đổi view dựa trên `ContentControl` để đơn giản.
- Các tính năng cần nhóm có thể được nhận diện dễ dàng từ tên view hiện tại của chúng.
