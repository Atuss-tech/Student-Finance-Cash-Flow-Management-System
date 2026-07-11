# Feature: Quên Mật Khẩu
Status: Draft
Author: AI Agent | Date: 2026-07-07
Priority: Medium

## 1. Business Context
Đảm bảo trải nghiệm người dùng không bị gián đoạn và giúp người dùng không bị mất trắng dữ liệu tài chính chỉ vì một phút lơ đãng quên thông tin đăng nhập.

## 2. User Stories
# Story 1:
As a user who forgot their password, I want to answer my predefined security question so that I can set a new password for my account safely.

## 3. Acceptance Criteria (EARS)
# WHEN user clicks "Forgot Password" AND provides a registered username
# THE SYSTEM SHALL display their configured security question.
# WHEN user submits the correct answer to the security question
# THE SYSTEM SHALL allow them to input a new password AND save the hashed password.
# WHEN user answers the security question incorrectly
# THE SYSTEM SHALL display an error message "Câu trả lời không chính xác."

## 4. API Contract / Technical Detail
# Lớp logic C#:
# - AuthService: VerifySecurityAnswer(username, answer), ResetPassword(username, newPasswordHash).
# - User repository cần có thêm cột `SecurityQuestion` và `SecurityAnswer` trong database nếu chưa có.

## 5. Technical Constraints
# - Hệ thống tuyệt đối không hiển thị lại mật khẩu cũ.
# - Mật khẩu mới bắt buộc phải được mã hóa (ví dụ SHA256 hoặc BCrypt) trước khi lưu (BR-014).

## 6. Out of Scope
# - Gửi mã xác thực qua SMS (SMS OTP).
# - Quản lý token khôi phục phức tạp qua JWT (do đây là app WPF local DB).
