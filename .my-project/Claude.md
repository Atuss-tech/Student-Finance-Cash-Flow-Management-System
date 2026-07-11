# CLAUDE.md — Claude Code Project Memory

# Đọc file AGENTS.md trước để hiểu full project context

## MANUAL MEMORY (human-maintained)

### Project Overview

# Project name: Student Finance & Cash Flow Management System
# Topic: Ứng dụng WPF quản lý tài chính và dòng tiền cho sinh viên
# Main goal: Giúp sinh viên quản lý thu nhập, chi tiêu, ngân sách, giao dịch và báo cáo tài chính cá nhân
# Main platform: Desktop application
# Main technologies: C#, WPF, .NET 8, SQL Server
# UI framework: WPF
# Database: SQL Server
# Architecture style: Layered Architecture + MVVM
# Main layers:
# - BusinessObjects
# - DataAccess
# - Repositories
# - Services
# - WPF

---

## Project Structure

### BusinessObjects

# Folder: BusinessObjects
# Purpose: Chứa các class đại diện cho dữ liệu chính của hệ thống.
# Đây là tầng Model/Entity của project.
# Không xử lý giao diện.
# Không xử lý truy vấn database.
# Không chứa logic nghiệp vụ phức tạp.

# Ví dụ class nên đặt ở đây:
# - User.cs
# - Transaction.cs
# - Category.cs
# - Budget.cs
# - Wallet.cs
# - FinancialReport.cs

# Rule:
# - Class dùng PascalCase.
# - Property dùng PascalCase.
# - Chỉ chứa property, constructor đơn giản và validation rất cơ bản nếu cần.
# - Không gọi SQL Server trong BusinessObjects.

---

### DataAccess

# Folder: DataAccess
# Purpose: Chứa phần kết nối và thao tác trực tiếp với SQL Server.
# Đây là tầng thấp nhất làm việc với database.
# Repository sẽ gọi DataAccess, không để ViewModel gọi trực tiếp DataAccess.

# Có thể chứa:
# - DbContext nếu dùng Entity Framework Core
# - DatabaseHelper nếu dùng ADO.NET
# - Connection helper
# - SQL query helper
# - Các class xử lý truy cập database mức thấp

# Ví dụ:
# - AppDbContext.cs
# - DatabaseHelper.cs
# - DbConnectionFactory.cs

# Rule:
# - Không viết logic giao diện trong DataAccess.
# - Không viết business rule ở đây.
# - Luôn dùng parameterized query nếu dùng ADO.NET.
# - Không hard-code connection string ở nhiều file.
# - Connection string nên quản lý tập trung.

---

### Repositories

# Folder: Repositories
# Purpose: Chứa các class xử lý CRUD và truy vấn dữ liệu theo từng entity.
# Repository là lớp trung gian giữa Services và DataAccess.
# Services gọi Repository để lấy/lưu dữ liệu.

# Ví dụ:
# - UserRepository.cs
# - TransactionRepository.cs
# - CategoryRepository.cs
# - BudgetRepository.cs

# Repository chịu trách nhiệm:
# - Add
# - Update
# - Delete
# - GetById
# - GetAll
# - Search / Filter dữ liệu
# - Mapping dữ liệu từ database sang BusinessObjects nếu cần

# Rule:
# - Không xử lý nghiệp vụ phức tạp trong Repository.
# - Không hiển thị MessageBox trong Repository.
# - Không gọi trực tiếp View hoặc ViewModel.
# - Không validate UI ở đây.
# - Repository chỉ tập trung vào dữ liệu.

---

### Services

# Folder: Services
# Purpose: Chứa logic nghiệp vụ chính của hệ thống.
# ViewModel sẽ gọi Service.
# Service sẽ gọi Repository.

# Ví dụ:
# - AuthService.cs
# - TransactionService.cs
# - BudgetService.cs
# - ReportService.cs
# - CategoryService.cs

# Service chịu trách nhiệm:
# - Validate dữ liệu nghiệp vụ
# - Kiểm tra số tiền hợp lệ
# - Kiểm tra ngày giao dịch
# - Tính tổng thu nhập
# - Tính tổng chi tiêu
# - Tính số dư
# - Kiểm tra vượt ngân sách
# - Xử lý báo cáo tài chính
# - Gọi Repository để lưu hoặc lấy dữ liệu

# Rule:
# - Business rule phải ưu tiên đặt trong Service.
# - Không viết SQL trực tiếp trong Service nếu đã có Repository.
# - Không xử lý giao diện trong Service.
# - Không gọi MessageBox trong Service.
# - Service trả kết quả rõ ràng cho ViewModel.

---

### WPF

# Folder: WPF
# Purpose: Chứa phần giao diện desktop của ứng dụng.
# Đây là tầng Presentation.
# Sử dụng MVVM để tách View và logic hiển thị.

# Có thể chứa:
# - Views
# - ViewModels
# - Commands
# - Converters
# - Resources
# - App.xaml
# - MainWindow.xaml

# Ví dụ:
# - Views/LoginView.xaml
# - Views/DashboardView.xaml
# - Views/TransactionView.xaml
# - Views/BudgetView.xaml
# - Views/ReportView.xaml

# - ViewModels/LoginViewModel.cs
# - ViewModels/DashboardViewModel.cs
# - ViewModels/TransactionViewModel.cs
# - ViewModels/BudgetViewModel.cs
# - ViewModels/ReportViewModel.cs

# - Commands/RelayCommand.cs
# - Converters/MoneyFormatConverter.cs

# Rule:
# - View chỉ hiển thị giao diện.
# - ViewModel xử lý binding, command và trạng thái màn hình.
# - Không gọi trực tiếp Repository từ View.
# - Không gọi trực tiếp DataAccess từ View.
# - Không viết logic nghiệp vụ dài trong file .xaml.cs.
# - Ưu tiên Binding và ICommand thay vì click event trong code-behind.

---

### Build Output

# Folder: obj/Debug/net8.0-windows
# Purpose: Đây là thư mục build tự sinh bởi .NET.
# Không chỉnh sửa thủ công.
# Không xem đây là tầng kiến trúc.
# Nên được ignore trong Git cùng với bin/ và obj/.

---

## Architecture Decisions

# ADR-001: Chọn WPF vì project là ứng dụng desktop quản lý tài chính sinh viên, cần giao diện trực quan và phù hợp với C#/.NET.
# ADR-002: Chọn SQL Server vì dễ tích hợp với C#, ổn định và phù hợp với dữ liệu giao dịch tài chính.
# ADR-003: Chọn Layered Architecture để tách rõ trách nhiệm giữa giao diện, nghiệp vụ và dữ liệu.
# ADR-004: Chọn MVVM cho tầng WPF để giảm code-behind và giúp UI dễ bảo trì.
# ADR-005: BusinessObjects chỉ chứa entity/model, không chứa logic truy cập database.
# ADR-006: DataAccess chỉ xử lý kết nối và thao tác database mức thấp.
# ADR-007: Repositories chịu trách nhiệm CRUD và truy vấn dữ liệu.
# ADR-008: Services chịu trách nhiệm xử lý nghiệp vụ và validate dữ liệu.
# ADR-009: WPF chỉ làm presentation layer, không truy vấn database trực tiếp.
# ADR-010: Không để ViewModel gọi trực tiếp DataAccess; ViewModel chỉ nên gọi Service.

---

## Main Data Models

# User:
# - UserId
# - FullName
# - Email
# - Username
# - PasswordHash
# - CreatedAt

# Category:
# - CategoryId
# - CategoryName
# - CategoryType
# - Description

# Transaction:
# - TransactionId
# - UserId
# - CategoryId
# - Amount
# - TransactionType
# - TransactionDate
# - Description
# - CreatedAt
# - UpdatedAt

# Budget:
# - BudgetId
# - UserId
# - CategoryId
# - Month
# - Year
# - LimitAmount
# - CreatedAt
# - UpdatedAt

# FinancialReport:
# - TotalIncome
# - TotalExpense
# - Balance
# - Month
# - Year

---

## Core Features

# Authentication:
# - Đăng nhập
# - Đăng xuất
# - Kiểm tra tài khoản người dùng

# Transaction Management:
# - Thêm giao dịch thu nhập
# - Thêm giao dịch chi tiêu
# - Cập nhật giao dịch
# - Xóa giao dịch
# - Xem danh sách giao dịch
# - Lọc giao dịch theo ngày
# - Lọc giao dịch theo loại Income/Expense
# - Lọc giao dịch theo danh mục

# Category Management:
# - Xem danh mục thu nhập
# - Xem danh mục chi tiêu
# - Thêm/sửa/xóa danh mục nếu được yêu cầu

# Budget Management:
# - Tạo ngân sách theo tháng
# - Tạo ngân sách theo danh mục
# - Kiểm tra số tiền đã dùng
# - Cảnh báo khi vượt ngân sách

# Report Management:
# - Tổng thu nhập theo tháng
# - Tổng chi tiêu theo tháng
# - Số dư còn lại
# - Thống kê chi tiêu theo danh mục
# - Hiển thị báo cáo bằng bảng hoặc biểu đồ nếu có

---

## Business Rules

# BR-001: Số tiền giao dịch không được để trống.
# BR-002: Số tiền giao dịch phải là số và lớn hơn 0.
# BR-003: Ngày giao dịch không được để trống.
# BR-004: Ngày giao dịch không được lớn hơn ngày hiện tại nếu chỉ ghi nhận giao dịch đã xảy ra.
# BR-005: Mỗi giao dịch bắt buộc phải có loại giao dịch: Income hoặc Expense.
# BR-006: Mỗi giao dịch bắt buộc phải thuộc một danh mục hợp lệ.
# BR-007: Tổng số dư = Tổng thu nhập - Tổng chi tiêu.
# BR-008: Ngân sách tháng phải lớn hơn 0.
# BR-009: Không nên tạo trùng ngân sách cho cùng một danh mục trong cùng tháng.
# BR-010: Khi tổng chi tiêu vượt ngân sách, hệ thống cần hiển thị cảnh báo cho người dùng.
# BR-011: Không được xóa danh mục nếu danh mục đó đang được dùng bởi giao dịch, trừ khi có rule xử lý rõ ràng.
# BR-012: Không lưu mật khẩu dạng plain text nếu project có chức năng đăng nhập thật.

---

## Patterns To Follow

### BusinessObjects Pattern

# File pattern:
# BusinessObjects/[EntityName].cs

# Example:
# BusinessObjects/Transaction.cs

# Entity chỉ nên chứa:
# - Properties
# - Constructor nếu cần
# - Không chứa SQL
# - Không chứa MessageBox
# - Không chứa logic UI

---

### DataAccess Pattern

# File pattern:
# DataAccess/[Name]DAO.cs
# hoặc:
# DataAccess/AppDbContext.cs

# DataAccess dùng để:
# - Mở kết nối database
# - Thực hiện query
# - Trả dữ liệu về Repository
# - Quản lý connection string

---

### Repository Pattern

# File pattern:
# Repositories/[EntityName]Repository.cs

# Example:
# Repositories/TransactionRepository.cs

# Method nên có:
# - GetAll()
# - GetById(int id)
# - Add(Entity entity)
# - Update(Entity entity)
# - Delete(int id)
# - Search(...)
# - Filter(...)

---

### Service Pattern

# File pattern:
# Services/[FeatureName]Service.cs

# Example:
# Services/TransactionService.cs

# Method nên có:
# - AddTransaction(...)
# - UpdateTransaction(...)
# - DeleteTransaction(...)
# - GetTransactions(...)
# - GetTotalIncome(...)
# - GetTotalExpense(...)
# - GetBalance(...)

# Service phải validate trước khi gọi Repository.

---

### WPF MVVM Pattern

# View pattern:
# WPF/Views/[Name]View.xaml

# ViewModel pattern:
# WPF/ViewModels/[Name]ViewModel.cs

# Command pattern:
# WPF/Commands/RelayCommand.cs

# Rule:
# - Button dùng ICommand.
# - TextBox dùng Binding.
# - ComboBox dùng Binding.
# - DataGrid dùng ItemsSource.
# - SelectedItem dùng Binding.
# - Không xử lý nghiệp vụ trong code-behind.

---

## Naming Rules

# Class name: PascalCase
# Example:
# - TransactionService
# - BudgetViewModel
# - UserRepository

# Method name: PascalCase
# Example:
# - AddTransaction()
# - UpdateBudget()
# - GetMonthlyReport()

# Variable name: camelCase
# Example:
# - totalIncome
# - selectedTransaction
# - transactionList

# XAML control name:
# - Button: btnAddTransaction
# - TextBox: txtAmount
# - ComboBox: cboCategory
# - DataGrid: dgTransactions
# - DatePicker: dpTransactionDate

---

## UI Rules

# Dashboard screen nên hiển thị:
# - Tổng thu nhập
# - Tổng chi tiêu
# - Số dư hiện tại
# - Giao dịch gần đây
# - Cảnh báo vượt ngân sách nếu có

# Transaction screen nên có:
# - Form thêm/sửa giao dịch
# - DataGrid hiển thị danh sách giao dịch
# - Bộ lọc theo ngày
# - Bộ lọc theo loại giao dịch
# - Bộ lọc theo danh mục
# - Button Add
# - Button Update
# - Button Delete
# - Button Clear

# Budget screen nên có:
# - Chọn tháng/năm
# - Chọn danh mục
# - Nhập hạn mức ngân sách
# - Hiển thị ngân sách đã dùng
# - Hiển thị ngân sách còn lại

# Report screen nên có:
# - Tổng thu theo tháng
# - Tổng chi theo tháng
# - Số dư
# - Chi tiêu theo danh mục
# - Biểu đồ nếu kịp triển khai

---

## Error Handling Rules

# Luôn dùng try-catch khi thao tác với database.
# Không hiển thị lỗi kỹ thuật quá dài cho người dùng.
# Thông báo lỗi phải dễ hiểu.
# Error messages nên gom vào một nơi nếu có thể.
# Khi lỗi database, hiển thị:
# "Không thể kết nối cơ sở dữ liệu. Vui lòng thử lại sau."

# Không để app crash khi:
# - Người dùng nhập số tiền sai định dạng
# - Người dùng bỏ trống trường bắt buộc
# - Database mất kết nối
# - Không có dữ liệu trong bảng
# - Người dùng chưa chọn item trong DataGrid

---

## Validation Rules

# Amount:
# - Required
# - Must be numeric
# - Must be greater than 0

# TransactionDate:
# - Required
# - Must not be greater than current date

# Category:
# - Required
# - Must exist in database

# TransactionType:
# - Required
# - Only Income or Expense

# Budget:
# - Required
# - Must be greater than 0
# - Should not duplicate same category in same month

# User:
# - Username required
# - Password required
# - Email format should be valid if email is used

---

## Testing Notes

# Cần test các case chính:
# - Thêm giao dịch hợp lệ
# - Thêm giao dịch thiếu số tiền
# - Thêm giao dịch số tiền âm
# - Thêm giao dịch nhập chữ vào ô số tiền
# - Thêm giao dịch không chọn danh mục
# - Thêm giao dịch không chọn ngày
# - Sửa giao dịch hợp lệ
# - Xóa giao dịch
# - Lọc giao dịch theo ngày
# - Lọc giao dịch theo Income
# - Lọc giao dịch theo Expense
# - Tạo ngân sách hợp lệ
# - Tạo ngân sách bị trùng tháng và danh mục
# - Vượt ngân sách
# - Database mất kết nối
# - Bảng giao dịch rỗng

---

## Git Rules

# Không commit thư mục:
# - bin/
# - obj/
# - .vs/

# Nên commit:
# - BusinessObjects/
# - DataAccess/
# - Repositories/
# - Services/
# - WPF/
# - README.md
# - AGENTS.md
# - CLAUDE.md
# - .gitignore
# - file .sln
# - file .csproj

---

## Current Sprint Notes

# Sprint focus: Xây dựng module quản lý giao dịch thu chi.
# Main tasks:
# - Tạo BusinessObjects cho Transaction, Category, Budget, User
# - Tạo DataAccess kết nối SQL Server
# - Tạo Repository cho Transaction
# - Tạo Service xử lý nghiệp vụ giao dịch
# - Tạo WPF View và ViewModel cho màn hình Transaction
# - Hiển thị danh sách giao dịch bằng DataGrid
# - Thêm/Sửa/Xóa giao dịch
# - Lọc giao dịch theo ngày, loại giao dịch và danh mục

# Blocked:
# - Cần chốt database schema cuối cùng
# - Cần thống nhất danh mục thu/chi mặc định
# - Cần xác định dùng ADO.NET hay Entity Framework Core

# Next:
# - Xây dựng Dashboard
# - Xây dựng Budget Management
# - Xây dựng Report Management
# - Thêm biểu đồ thống kê nếu còn thời gian

---

## AUTO MEMORY

# [Claude Code sẽ tự động thêm entries khi bạn làm việc]