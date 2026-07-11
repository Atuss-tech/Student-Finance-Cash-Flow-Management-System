# CONTEXT.md — Quản Lý Ngân Sách (Budget Management)
# Người viết: AI Assistant | Ngày: 2026-07-06

## 1. PROBLEM STATEMENT
Sinh viên thường dễ rơi vào tình trạng chi tiêu mất kiểm soát vì không tự đặt ra một giới hạn cụ thể cho từng khoản mục (ví dụ: Ăn uống 2 triệu, Mua sắm 500k). Việc chỉ ghi chép lại giao dịch mang tính chất "sự đã rồi" (hậu kiểm), không giúp định hướng hành vi tiêu dùng tương lai. Cần có chức năng cho phép thiết lập và quản lý hạn mức tối đa cho từng tháng (ngân sách) để tự giám sát bản thân.

## 2. DOMAIN KNOWLEDGE
- **Ngân sách (Budget):** Số tiền tối đa (`AmountLimit`) dự kiến được phép chi tiêu trong một khoảng thời gian (cụ thể là một tháng của một năm) cho một danh mục chi tiêu nhất định.
- **Tiến độ ngân sách (Budget Progress):** Tỉ lệ phần trăm giữa Tổng thực chi (Actual Spent) trên Ngân sách. Giúp sinh viên biết mình còn bao nhiêu "dư địa" (Remaining Amount) để chi tiêu tiếp.
- **Rollover (Cộng dồn):** Tính năng mang số dư ngân sách xài không hết của tháng trước cộng sang tháng sau. Ở dự án này, để đơn giản hoá, mỗi tháng sẽ quản lý ngân sách độc lập (không cộng dồn).

## 3. STAKEHOLDERS
- **Sinh viên (End Users):** Người khởi tạo, chỉnh sửa và theo dõi tiến độ ngân sách để thiết lập kỷ luật tài chính cá nhân.
- **Nhà phát triển (Developers):** Người xây dựng giao diện CRUD (Tạo, Đọc, Sửa, Xoá) ngân sách và tính toán tiến độ dựa trên các giao dịch thực tế từ Local Database.

## 4. CONSTRAINTS (ràng buộc không thể thay đổi)
- **Tech:** 
  - Giao diện xây dựng bằng WPF XAML. Tương tác với Local DB (đọc ghi bảng `Budgets`).
- **Business:** 
  - Một người dùng (`UserId`) chỉ được tạo duy nhất một ngân sách cho một Danh mục (`CategoryId`) cụ thể trong cùng một Tháng (`Month`) và Năm (`Year`). Database đã có Constraint: `UQ_Budgets_User_Category_Month_Year`.
  - Chỉ áp dụng thiết lập ngân sách cho các danh mục thuộc loại "Chi tiêu" (`Expense`), không áp dụng cho danh mục "Thu nhập" (`Income`).

## 5. ASSUMPTIONS (giả định — cần confirm)
- Khi thêm một giao dịch chi tiêu mới, nếu chưa có Ngân sách cho danh mục đó, hệ thống sẽ bỏ qua việc tính tiến độ/cảnh báo.
- Người dùng có quyền chỉnh sửa Ngân sách (AmountLimit) bất cứ lúc nào, kể cả khi tháng đó đã trôi qua (để điều chỉnh lại báo cáo quá khứ nếu có sai sót).

## 6. OPEN QUESTIONS (câu hỏi chưa có câu trả lời)
- Không có. Logic tính năng hoàn toàn phù hợp với cấu trúc bảng `Budgets` hiện tại, và tự động liên kết với `FinanceTransactions` thông qua `CategoryId`, `Month` và `Year`.
