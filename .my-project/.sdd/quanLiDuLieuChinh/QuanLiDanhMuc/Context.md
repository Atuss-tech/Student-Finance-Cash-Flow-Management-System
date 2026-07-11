# CONTEXT.md — Quản Lý Danh Mục (Category Management)
# Người viết: AI Agent | Ngày: 2026-07-07

## 1. PROBLEM STATEMENT
Sinh viên có rất nhiều khoản chi tiêu (ăn uống, học phí, nhà trọ) và nguồn thu (gia đình gửi, làm thêm). Việc không phân loại được các khoản này khiến họ không biết tiền của mình đang đi đâu về đâu, dẫn đến mất kiểm soát dòng tiền.

## 2. DOMAIN KNOWLEDGE
- **Category (Danh mục)**: Các nhóm phân loại cho thu nhập và chi tiêu.
- **Income (Thu nhập)**: Các dòng tiền đi vào.
- **Expense (Chi tiêu)**: Các dòng tiền đi ra.
- Mỗi giao dịch trong hệ thống bắt buộc phải gán với một danh mục cụ thể.

## 3. STAKEHOLDERS
- **Sinh viên (Người dùng chính)**: Cần tạo, sửa, xóa danh mục để phân loại chi tiêu theo thói quen cá nhân.
- **Hệ thống**: Dựa vào danh mục để lên các báo cáo thống kê và biểu đồ chi tiêu.

## 4. CONSTRAINTS (Ràng buộc)
- Phải tuân theo Architecture Rule: Không truy vấn DB trực tiếp từ giao diện (WPF), qua Repository và Service.
- Không được xóa danh mục nếu danh mục đang được sử dụng bởi bất kỳ giao dịch nào (đảm bảo tính toàn vẹn dữ liệu - BR-011).

## 5. ASSUMPTIONS (Giả định)
- Hệ thống sẽ cung cấp sẵn một vài danh mục mặc định (Ví dụ: Ăn uống, Tiền nhà, Lương) không cho phép xóa, hoặc user phải tạo từ đầu (cần xác nhận).

## 6. OPEN QUESTIONS
- Có cần hỗ trợ danh mục con (Sub-category) không? (Tạm định là không, thiết kế phẳng 1 cấp cho đơn giản).
