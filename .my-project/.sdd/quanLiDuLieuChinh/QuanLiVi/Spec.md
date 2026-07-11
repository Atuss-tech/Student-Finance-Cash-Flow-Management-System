# Feature: Quản Lý Số Dư / Ví (Wallet)
Status: Draft
Author: AI Agent | Date: 2026-07-07
Priority: Medium

## 1. Business Context
Tính năng cung cấp cái nhìn tổng quát và ngay lập tức về tình trạng tài chính hiện tại của sinh viên. Bằng việc tính toán chênh lệch giữa các khoản thu và chi, hệ thống tự động đưa ra số dư khả dụng, giúp người dùng an tâm quản lý dòng tiền của mình.

## 2. User Stories
# Story 1 (Happy Path):
As a student, I want to see my current total balance on the dashboard so that I know exactly how much money I currently have to spend.

## 3. Acceptance Criteria (EARS)
# WHEN user views the dashboard
# THE SYSTEM SHALL calculate and display Balance = Total Income - Total Expense.
# WHEN user adds, updates, or deletes any transaction
# THE SYSTEM SHALL dynamically recalculate the balance to reflect the newest data.

## 4. API Contract / Technical Detail
# Không dùng API, tính toán qua Service C#:
# - TransactionService.GetBalance() hoặc ReportService.GetBalance()
# - ViewModel: DashboardViewModel
# Trả về: Biến số kiểu decimal đại diện cho Số dư.

## 5. Technical Constraints
# - Tối ưu hóa câu truy vấn: Có thể query tổng thu và tổng chi trực tiếp bằng SQL (`SUM(Amount)`) thay vì load tất cả giao dịch vào RAM để tính toán.
# - UI update tự động sử dụng Data Binding (INotifyPropertyChanged).

## 6. Out of Scope
# - Chức năng Đa ví (Multi-wallets: tiền mặt, tài khoản ngân hàng).
# - Tính năng chuyển tiền (Transfer) giữa các ví nội bộ.
