# CONTEXT.md — Quản Lý Giao Dịch (Transaction Management)
# Người viết: AI Agent | Ngày: 2026-07-07

## 1. PROBLEM STATEMENT
Sinh viên thường xuyên chi tiêu nhỏ lẻ mỗi ngày (ăn vặt, gửi xe) hoặc nhận các khoản tiền không cố định. Không có công cụ ghi nhận nhanh chóng và hệ thống hóa, họ sẽ mau chóng quên các khoản này và luôn rơi vào trạng thái "chưa tiêu gì đã hết tiền".

## 2. DOMAIN KNOWLEDGE
- **Transaction (Giao dịch)**: Ghi nhận sự kiện tiền ra (chi tiêu) hoặc tiền vào (thu nhập).
- **Amount (Số tiền)**: Giá trị của giao dịch, bắt buộc là số dương.
- **TransactionDate**: Ngày phát sinh giao dịch. Không được ghi nhận giao dịch của ngày tương lai (nếu giả định chỉ ghi nhận thực tế).

## 3. STAKEHOLDERS
- **Sinh viên**: Ghi nhận và theo dõi các giao dịch hàng ngày để đối soát tài chính.
- **Hệ thống**: Tổng hợp mọi giao dịch để tính toán số dư và cảnh báo nếu vượt Budget.

## 4. CONSTRAINTS (Ràng buộc)
- Dữ liệu tuân theo Business Rules (BR-001 đến BR-007).
- Một giao dịch bắt buộc phải thuộc một Danh mục (Category) và có loại rõ ràng (Income/Expense).

## 5. ASSUMPTIONS (Giả định)
- Ứng dụng chỉ xử lý duy nhất 1 loại tiền tệ (VND).
- Không có chức năng chuyển tiền giữa các ví (do ứng dụng không thiết kế cấu trúc ví phức tạp đa tài khoản).

## 6. OPEN QUESTIONS
- Có cho phép ghi lại giao dịch trong tương lai (ví dụ lịch đóng học phí) không? (Hiện tại quy định BR-004 không cho phép lớn hơn ngày hiện tại, cần làm rõ xem có mở rộng sau này không).
