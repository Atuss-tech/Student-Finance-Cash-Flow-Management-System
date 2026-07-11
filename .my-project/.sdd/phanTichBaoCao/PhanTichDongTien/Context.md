# CONTEXT.md — Phân Tích Dòng Tiền (Cash Flow Analysis)
# Người viết: AI Assistant | Ngày: 2026-07-06

## 1. PROBLEM STATEMENT
Sinh viên thường chỉ nhìn vào "Số dư ví" (Balance) để biết mình còn bao nhiêu tiền, nhưng lại không hiểu được "Dòng tiền" (Cash Flow) của mình đang ở trạng thái âm hay dương. Nếu một tháng thu nhập 5 triệu nhưng chi tiêu tới 6 triệu, số dư ví vẫn có thể còn (nhờ tiền tích lũy từ tháng trước), nhưng thực chất dòng tiền tháng đó đang thâm hụt. Cần có một công cụ phân tích rõ ràng chênh lệch giữa Thu - Chi để sinh viên nhận thức được mức độ thâm hụt hoặc tích lũy thực tế qua các kỳ.

## 2. DOMAIN KNOWLEDGE
- **Dòng tiền vào (Cash Inflow / Income):** Tổng các khoản tiền thu được trong một kỳ (lương, gia đình gửi, thưởng...).
- **Dòng tiền ra (Cash Outflow / Expense):** Tổng các khoản tiền chi ra trong một kỳ (ăn uống, học phí, nhà trọ...).
- **Dòng tiền ròng (Net Cash Flow):** Chênh lệch giữa Dòng tiền vào và Dòng tiền ra (`Total Income - Total Expense`). 
  - Nếu dương (Positive): Có thặng dư/tiết kiệm. 
  - Nếu âm (Negative): Đang tiêu lẹm vào vốn/số dư cũ.

## 3. STAKEHOLDERS
- **Sinh viên (End Users):** Người xem báo cáo phân tích để đánh giá "sức khỏe tài chính" thực sự qua từng tháng hoặc năm.
- **Nhà phát triển (Developers):** Người xử lý logic tính toán gom nhóm (Group By) dữ liệu theo tháng/năm hiệu quả từ Local Database và biểu diễn UI (biểu đồ kép, số âm/dương).

## 4. CONSTRAINTS (ràng buộc không thể thay đổi)
- **Tech:** 
  - Giao diện xây dựng bằng WPF XAML.
  - Xử lý dữ liệu hoàn toàn trên Local Database (SQLite / SQL Server LocalDB), không sử dụng API hay Backend server.
- **Business:** 
  - Số liệu Thu, Chi và Dòng tiền ròng phải khớp tuyệt đối với tổng của các giao dịch trong bảng `FinanceTransactions`. Công thức tính Dòng tiền ròng bắt buộc là `Total Income - Total Expense`.

## 5. ASSUMPTIONS (giả định — cần confirm)
- Kỳ phân tích xu hướng (Trend) mặc định được sinh viên quan tâm nhất là "Theo tháng" trong một năm (So sánh 12 tháng của năm cụ thể). 
- Nếu không có giao dịch thu/chi nào phát sinh trong một tháng bất kỳ, giá trị Thu/Chi của tháng đó tự động được nội suy là 0.

## 6. OPEN QUESTIONS (câu hỏi chưa có câu trả lời)
- Không có. Logic tính toán hoàn toàn có thể được giải quyết bằng các hàm tổng hợp `SUM()` và `GROUP BY` từ bảng `FinanceTransactions`.
