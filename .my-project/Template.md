# CONTEXT.md — [Feature/Module Name]
# Người viết: @name | Ngày: YYYY-MM-DD
## 1. PROBLEM STATEMENT
<!-- Vấn đề thực sự là gì? User đang bị đau ở đâu? -->
<!-- Tránh solution thinking ở bước này. Chỉ mô tả pain. -->
## 2. DOMAIN KNOWLEDGE
<!-- Các thuật ngữ domain-specific mà AI cần biết -->
<!-- Ví dụ: "invoice" trong hệ thống này nghĩa là gì? -->
<!-- Các quy tắc nghiệp vụ bất thành văn -->
## 3. STAKEHOLDERS
<!-- Ai được lợi? Ai chịu ảnh hưởng? Ai có quyền quyết định? -->
## 4. CONSTRAINTS (ràng buộc không thể thay đổi)
<!-- Tech: "Phải dùng PostgreSQL vì infrastructure hiện tại" -->
<!-- Business: "Phải comply với Thông tư 06/2023/TT-NHNN" -->
<!-- Time: "Phải live trước 30/06" -->
## 5. ASSUMPTIONS (giả định — cần confirm)
<!-- Những điều bạn assume là đúng nhưng chưa confirm -->
<!-- Mỗi assumption là một rủi ro nếu sai -->
## 6. OPEN QUESTIONS (câu hỏi chưa có câu trả lời)
<!-- Những điều cần clarify với stakeholder trước khi viết spec -->





📗 LIGHT SPEC — Cho CRUD đơn giản, UI component, bug fix
📄 feature-[name].light.spec.md
# Feature: [Tên tính năng]
Status: Draft | Review | Approved
Author: [Tên] | Date: [YYYY-MM-DD]
## User Story
# As a [user type], I want to [action] so that [benefit].
## Acceptance Criteria (EARS notation)
# WHEN [trigger] THE SYSTEM SHALL [action].
# WHEN [error trigger] THE SYSTEM SHALL [error handling].
## Technical Notes
# - API endpoint: [METHOD] /api/v1/[resource]
# - DB changes: [none / add column X to table Y]
# - Validation: [list input validation rules]

📘 STANDARD SPEC — Cho feature có business logic
📄 feature-[name].spec.md
# Feature: [Tên tính năng]
Status: Draft | Review | Approved
Author: [Tên] | Reviewer: [Tên] | Date: [YYYY-MM-DD]
Priority: High | Medium | Low
## 1. Business Context
# [Giải thích tại sao feature này cần tồn tại — 2-3 câu]
# [Liên kết với business goal nào của dự án]
## 2. User Stories

LinhNDM | Playbook: Spec-Driven & Agent-Driven Development | Trang 362

# Story 1 (Happy Path):
# As a [user type], I want to [action] so that [benefit].
# Story 2 (Edge Case):
# As a [user type], when [condition], I want to [action].
## 3. Acceptance Criteria (EARS)
# WHEN user submits [form] with valid data
# THE SYSTEM SHALL [action] AND return [response].
# WHEN user submits [form] with invalid [field]
# THE SYSTEM SHALL return HTTP 400 with error code [CODE].
# WHILE user is [state], THE SYSTEM SHALL [restriction].
## 4. API Contract
# Endpoint: POST /api/v1/[resource]
# Request: { field1: string (required), field2: number (optional) }
# Response 201: { success: true, data: { id, ...fields } }
# Response 400: { success: false, error: { code, message } }
# Response 401: unauthorized
## 5. Technical Constraints
# - Max response time: 500ms (p95)
# - Rate limit: 100 requests/minute per user
# - [Other constraints]
## 6. Out of Scope
# - [Feature X — will be in Sprint N+1]
# - [Integration with Y — separate spec]

📕 FULL SPEC — Cho core module, security-critical, high-risk
📄 feature-[name].full.spec.md
# Feature: [Tên tính năng] — FULL SPECIFICATION
Status: Draft | Review | Approved | Implemented
Author: [Tên] | Tech Lead Approval: [Tên] | Date: [YYYY-MM-DD]
Risk Level: High | Related Specs: [list file names]
## 1. Business Context & Goals
## 2. Stakeholders & User Personas
## 3. User Stories (all paths)
## 4. Acceptance Criteria (EARS — exhaustive)
## 5. API Contracts (full OpenAPI schema)
## 6. Data Models & DB Schema Changes
## 7. Non-Functional Requirements
# - Performance: [SLA targets]
# - Security: [auth, data handling, compliance]
# - Scalability: [expected load]
# - Availability: [uptime requirement]
## 8. Error Handling Matrix
# [List all error codes, messages, HTTP status, retry behavior]
## 9. Edge Cases & Corner Cases
## 10. Dependencies & Integration Points
## 11. Testing Requirements
# [Unit / Integration / E2E test requirements]
## 12. Rollout Plan
# [Feature flag? Gradual rollout? Migration plan?]
## 13. Open Questions (must resolve before implementation)

LinhNDM | Playbook: Spec-Driven & Agent-Driven Development | Trang 363

# Q1: [Question] — Owner: [Name] — Due: [Date]