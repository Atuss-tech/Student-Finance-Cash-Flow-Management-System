# Context.md — Login Feature

Feature: Login  
Status: Draft  
Author: Pham Anh  
Date: 2026-07-05  
Project: Student Finance & Cash Flow Management System  

---

## 1. Feature Context

Student Finance & Cash Flow Management System là ứng dụng desktop WPF giúp sinh viên quản lý tài chính cá nhân.

Chức năng Đăng nhập dùng để xác thực người dùng trước khi cho phép truy cập vào trang chính của hệ thống. Sau khi đăng nhập thành công, người dùng sẽ được chuyển vào Dashboard để xem và quản lý dữ liệu tài chính cá nhân.

Hệ thống hỗ trợ 2 cách đăng nhập:

1. Đăng nhập bằng Gmail.
2. Đăng nhập bằng tên đăng nhập và mật khẩu.

---

## 2. Business Problem

Ứng dụng quản lý tài chính cá nhân cần phân biệt dữ liệu của từng người dùng.

Nếu không có đăng nhập, hệ thống không thể biết giao dịch, ngân sách và báo cáo tài chính thuộc về người dùng nào. Điều này có thể làm dữ liệu bị lẫn giữa nhiều tài khoản.

Chức năng Đăng nhập giúp đảm bảo:

- Chỉ người dùng hợp lệ mới được vào hệ thống.
- Dữ liệu tài chính được gắn với đúng người dùng.
- Người dùng chưa đăng nhập không được truy cập Dashboard, Transaction, Budget và Report.

---

## 3. Business Goals

Chức năng Đăng nhập cần đạt các mục tiêu sau:

- Cho phép người dùng đăng nhập bằng username/password.
- Cho phép người dùng đăng nhập bằng Gmail.
- Kiểm tra thông tin đăng nhập có tồn tại trong SQL Server.
- Sau khi đăng nhập thành công, chuyển người dùng vào Dashboard.
- Khi đăng nhập thất bại, hiển thị thông báo lỗi dễ hiểu.
- Không để app crash khi người dùng nhập sai hoặc database lỗi.
- Lưu trạng thái người dùng hiện tại sau khi đăng nhập thành công.

---

## 4. User Types

### 4.1 Gmail User

Người dùng đã có tài khoản tạo bằng Gmail.

Hành vi mong muốn:

- Bấm nút "Đăng nhập bằng Gmail".
- Chọn hoặc nhập Gmail.
- Hệ thống kiểm tra Gmail trong database.
- Nếu Gmail tồn tại, đăng nhập thành công.
- Nếu Gmail chưa tồn tại, hệ thống thông báo tài khoản chưa được đăng kí.

---

### 4.2 Local User

Người dùng đã có tài khoản tạo bằng username/password.

Hành vi mong muốn:

- Nhập username.
- Nhập password.
- Bấm nút "Đăng nhập".
- Nếu thông tin đúng, hệ thống chuyển vào Dashboard.
- Nếu thông tin sai, hệ thống hiển thị lỗi.

---

## 5. Architecture Context

Project sử dụng kiến trúc:

```txt
BusinessObjects
DataAccess
Repositories
Services
WPF