# CONTEXT.md — Quản Lý Ví / Số Dư (Wallet/Balance Management)
# Người viết: AI Agent | Ngày: 2026-07-07

## 1. PROBLEM STATEMENT
Người dùng cần một nơi để biết ngay lập tức tổng số tiền mình đang có (Số dư / Balance) mà không cần phải tự thực hiện phép tính nhẩm thủ công (tổng thu trừ đi tổng chi). Nếu thiếu con số này, họ sẽ không thể đưa ra quyết định mua sắm tức thời.

## 2. DOMAIN KNOWLEDGE
- **Balance (Số dư)**: Bằng Tổng thu nhập - Tổng chi tiêu.
- **Wallet (Ví)**: Có thể coi là nơi chứa tiền (trong scope này chỉ quản lý số dư tổng quát, không chia thành các loại ví cụ thể).

## 3. STAKEHOLDERS
- **Sinh viên**: Muốn xem ngay số dư hiện tại trên Dashboard mỗi khi mở ứng dụng.

## 4. CONSTRAINTS (Ràng buộc)
- Dữ liệu Số dư không được lưu tĩnh trong database mà phải tính toán động từ tổng các giao dịch (để tránh lệch số liệu).
- Số dư có thể âm (trong trường hợp vay nợ), nhưng cần cảnh báo hiển thị.

## 5. ASSUMPTIONS (Giả định)
- Không quản lý nhiều tài khoản ngân hàng hoặc ví điện tử (chỉ tính 1 số dư tổng hợp).
- Giao dịch bị xóa sẽ lập tức cập nhật lại số dư.

## 6. OPEN QUESTIONS
- Liệu có cần mở rộng hỗ trợ nhiều ví (Tiền mặt, Momo, Vietcombank) trong tương lai không? (Với scope bài tập sinh viên, giữ 1 ví chung là an toàn nhất).
