# Feature: Quản Lý Danh Mục
Status: Draft
Author: AI Agent | Date: 2026-07-07
Priority: High

## 1. Business Context
Việc phân loại giao dịch là nền tảng cốt lõi giúp sinh viên hiểu rõ dòng tiền của mình. Tính năng này cung cấp các danh mục thu/chi (Income/Expense) để gán cho từng giao dịch, hỗ trợ đắc lực cho việc xuất báo cáo và lập ngân sách sau này.

## 2. User Stories
# Story 1 (Happy Path - Add):
As a student, I want to add a new custom category so that I can track specific types of expenses unique to me.
# Story 2 (Happy Path - Edit):
As a student, I want to update my custom category's name so that it accurately reflects my current tracking needs.
# Story 3 (Edge Case - Delete):
As a student, when a category is no longer needed and has no associated transactions, I want to delete it.

## 3. Acceptance Criteria (EARS)
# WHEN user submits a new category with a valid name and valid type (Income/Expense)
# THE SYSTEM SHALL save the category AND immediately update the category list.
# WHEN user submits a category with an empty name
# THE SYSTEM SHALL display an error message "Vui lòng nhập tên danh mục."
# WHEN user attempts to delete a category that is currently linked to an existing transaction
# THE SYSTEM SHALL block the deletion AND show a warning message "Không thể xóa danh mục đang có giao dịch sử dụng."

## 4. API Contract / Technical Detail
# Không dùng REST API, sử dụng cấu trúc Layered:
# - ICategoryRepository: Add(), Update(), Delete(), GetAll()
# - CategoryService: AddCategory(), UpdateCategory(), DeleteCategory()
# - WPF ViewModel: CategoryViewModel
# Dữ liệu truyền: Category { CategoryId, CategoryName, CategoryType, Description }

## 5. Technical Constraints
# - Tên danh mục không được vượt quá độ dài cột trong DB (ví dụ: 100 ký tự).
# - Thao tác CRUD phải được xử lý qua try-catch hiển thị lỗi lịch sự.

## 6. Out of Scope
# - Icon hoặc màu sắc tùy chỉnh riêng cho từng danh mục.
# - Hệ thống danh mục đa cấp (parent-child categories).
