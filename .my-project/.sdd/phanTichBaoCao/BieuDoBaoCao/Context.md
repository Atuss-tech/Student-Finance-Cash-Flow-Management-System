# CONTEXT.md — Biểu Đồ Báo Cáo (Reporting Charts)
# Người viết: AI Assistant | Ngày: 2026-07-06

## 1. PROBLEM STATEMENT
Sinh viên thường gặp khó khăn trong việc hình dung tổng quan về tình hình tài chính của mình chỉ thông qua các con số và danh sách giao dịch khô khan. Họ không nhận biết được xu hướng chi tiêu (đang tăng hay giảm), không thấy rõ tỷ trọng các khoản chi (ví dụ: ăn uống chiếm bao nhiêu phần trăm) để từ đó điều chỉnh hành vi tiêu dùng kịp thời.

## 2. DOMAIN KNOWLEDGE
- **Cash Flow (Dòng tiền):** Sự luân chuyển của tiền vào (thu nhập) và tiền ra (chi phí) trong một khoảng thời gian nhất định.
- **Biểu đồ tỷ trọng (Pie/Donut Chart):** Thường dùng để biểu diễn cơ cấu chi tiêu theo danh mục, giúp nhận diện khoản mục tiêu tốn nhiều nhất.
- **Biểu đồ xu hướng (Line/Bar Chart):** Thường dùng để theo dõi sự biến động của thu/chi hoặc số dư qua các chu kỳ (tháng/tuần).

## 3. STAKEHOLDERS
- **Sinh viên (End Users):** Người trực tiếp sử dụng biểu đồ để theo dõi, phân tích và điều chỉnh chi tiêu.
- **Nhà phát triển (Developers):** Người sẽ implement các UI components, thư viện vẽ biểu đồ và đảm bảo hiệu năng.

## 4. CONSTRAINTS (ràng buộc không thể thay đổi)
- **Tech:** 
  - Phải sử dụng WPF XAML (do dự án đang sử dụng WPF cho giao diện).
  - Bắt buộc sử dụng thư viện biểu đồ bên thứ ba (đã xác nhận cần có, ví dụ: LiveCharts2 hoặc OxyPlot) để đảm bảo chất lượng hiển thị.
  - Biểu đồ phải mượt mà, không gây giật lag (lagging) khi chuyển đổi dữ liệu.
- **Business:** 
  - Biểu đồ phải phản ánh dữ liệu chính xác và nhất quán với các số liệu ở màn hình danh sách giao dịch.
  - **Bắt buộc** phải có tính năng Drill-down (bấm vào 1 phần của biểu đồ tròn để xem chi tiết các giao dịch trong danh mục đó).

## 5. ASSUMPTIONS (giả định — cần confirm)
- Hệ thống đã có sẵn Service layer và các lớp `Repository/DAO` truy xuất trực tiếp từ Local Database, cung cấp sẵn các hàm trả về dữ liệu tổng hợp (aggregated data) thay vì phải tự aggregate trên UI.
- Sinh viên chủ yếu muốn xem báo cáo theo "Tháng hiện tại" làm mặc định.

## 6. OPEN QUESTIONS (câu hỏi chưa có câu trả lời)
- Không có (Các câu hỏi về thư viện biểu đồ, tính năng drill-down và xuất file ảnh đã được chốt/giải quyết).
