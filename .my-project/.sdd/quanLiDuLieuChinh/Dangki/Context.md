# Context.md — User Registration & Login

Feature: User Registration & Login  
Status: Draft  
Author: Pham Anh  
Date: 2026-07-05  
Project: Student Finance & Cash Flow Management System  

---

## 1. Feature Context

Ứng dụng Student Finance & Cash Flow Management System là ứng dụng desktop WPF dùng để quản lý tài chính cá nhân cho sinh viên.

Vì dữ liệu tài chính là dữ liệu riêng của từng người dùng, hệ thống cần có chức năng đăng kí và đăng nhập trước khi cho người dùng truy cập vào màn hình chính.

Feature này cho phép người dùng vào hệ thống bằng 2 cách:

1. Đăng kí / đăng nhập bằng Gmail.
2. Đăng kí / đăng nhập bằng tên đăng nhập và mật khẩu.

Sau khi xác thực thành công, người dùng sẽ được chuyển vào màn hình chính của ứng dụng.

---

## 2. Business Problem

Sinh viên cần một tài khoản riêng để lưu trữ:

- Giao dịch thu nhập
- Giao dịch chi tiêu
- Danh mục thu/chi
- Ngân sách cá nhân
- Báo cáo tài chính
- Số dư hiện tại

Nếu không có tài khoản người dùng, hệ thống sẽ không thể phân biệt dữ liệu của từng người. Điều này có thể dẫn đến việc dữ liệu tài chính bị lẫn giữa nhiều người dùng.

---

## 3. Business Goals

Feature này được xây dựng để:

- Cho phép người dùng đăng kí tài khoản nhanh chóng.
- Cho phép người dùng đăng nhập trước khi sử dụng hệ thống.
- Hỗ trợ đăng kí nhanh bằng Gmail.
- Hỗ trợ đăng kí thủ công bằng username/password.
- Đảm bảo mỗi người dùng có dữ liệu tài chính riêng.
- Điều hướng người dùng vào Dashboard sau khi đăng nhập thành công.
- Ngăn người dùng chưa đăng nhập truy cập vào màn hình chính.

---

## 4. User Types

### 4.1 Gmail User

Đây là người dùng muốn đăng kí nhanh, không muốn tạo username/password riêng.

Hành vi mong muốn:

- Bấm nút "Đăng kí bằng Gmail".
- Nếu Gmail chưa tồn tại, hệ thống tạo tài khoản mới.
- Nếu Gmail đã tồn tại, hệ thống đăng nhập luôn.
- Sau đó hệ thống chuyển vào Dashboard.

---

### 4.2 Local User

Đây là người dùng không muốn dùng Gmail, muốn đăng kí bằng tên đăng nhập và mật khẩu.

Hành vi mong muốn:

- Nhập username.
- Nhập password.
- Bấm đăng kí.
- Sau khi đăng kí thành công, có thể đăng nhập bằng username/password.
- Sau khi đăng nhập thành công, hệ thống chuyển vào Dashboard.

---

## 5. Current Project Architecture Context

Project đang sử dụng kiến trúc:

```txt
BusinessObjects
DataAccess
Repositories
Services
WPF