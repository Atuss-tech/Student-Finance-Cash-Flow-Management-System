# Feature: Quản Lý Giao Dịch
Status: Draft
Author: AI Agent | Date: 2026-07-07
Priority: High

## 1. Business Context
Quản lý giao dịch là trái tim của hệ thống Student Finance. Nó cung cấp luồng dữ liệu chính cho người dùng nhập liệu mỗi khi họ tiêu tiền hoặc nhận tiền. Mọi tính năng khác như báo cáo tổng, quản lý ngân sách hay tính toán số dư đều phụ thuộc hoàn toàn vào dữ liệu của tính năng này.

## 2. User Stories
# Story 1 (Happy Path - Log Expense):
As a student, I want to quickly log a daily expense so that my current balance is accurately updated.
# Story 2 (View and Filter):
As a student, I want to view a list of my transactions and filter them by date or category so that I can easily find a specific past spending.
# Story 3 (Edge Case - Validation):
As a student, when I mistakenly type text into the amount field, I want the system to warn me before saving.

## 3. Acceptance Criteria (EARS)
# WHEN user submits a transaction with valid amount (>0), valid date (<= today), type, and category
# THE SYSTEM SHALL save the record in the database AND refresh the transaction list.
# WHEN user submits a transaction with a negative amount
# THE SYSTEM SHALL display an error message "Số tiền phải lớn hơn 0."
# WHEN user submits a transaction without a selected category
# THE SYSTEM SHALL display an error message "Vui lòng chọn danh mục."
# WHEN user attempts to enter text in the amount field
# THE SYSTEM SHALL prevent input OR display validation error "Vui lòng nhập số tiền hợp lệ."

## 4. API Contract / Technical Detail
# Không dùng REST API, sử dụng Layered Architecture nội bộ:
# - ITransactionRepository: Add(), Update(), Delete(), GetFiltered(...)
# - TransactionService: Apply validation rules (BR-001 -> BR-007) before calling Repository.
# - WPF ViewModel: TransactionViewModel
# Data model: Transaction { TransactionId, CategoryId, Amount, TransactionType, TransactionDate, Description }

## 5. Technical Constraints
# - Tối ưu hiệu năng hiển thị cho DataGrid khi sinh viên có quá nhiều giao dịch (vài ngàn record).
# - Validation trên UI phải cung cấp phản hồi lập tức (INotifyDataErrorInfo).

## 6. Out of Scope
# - Tự động đồng bộ giao dịch từ tin nhắn SMS hay thông báo Mobile Banking.
# - Gắn hình ảnh hóa đơn vào giao dịch.
