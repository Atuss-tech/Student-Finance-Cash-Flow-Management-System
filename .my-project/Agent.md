# AGENTS.md

# Bối cảnh dự án dành cho AI Agent

**Phiên bản:** 1.0
**Ngày cập nhật:** 11/07/2026
**Dự án:** Student Finance & Cash Flow Management System

> AI Agent phải đọc `CONSTITUTION.md`, `AGENTS.md` và tài liệu đặc tả của chức năng trước khi tạo hoặc chỉnh sửa code.

---

## 1. TỔNG QUAN DỰ ÁN

**Tên dự án:** Student Finance & Cash Flow Management System
**Tên tiếng Việt:** Ứng dụng Quản lý Chi tiêu và Phân tích Dòng tiền cho Sinh viên
**Loại:** Ứng dụng Desktop WPF
**Lĩnh vực:** Quản lý tài chính cá nhân
**Đối tượng sử dụng:** Sinh viên
**Giai đoạn:** Development
**Quy mô nhóm:** 2 thành viên

Hệ thống hỗ trợ các chức năng:

* Đăng ký, đăng nhập và đăng xuất.
* Quản lý ví tiền.
* Quản lý danh mục thu nhập và chi tiêu.
* Quản lý giao dịch thu nhập và chi tiêu.
* Xem và lọc lịch sử giao dịch.
* Quản lý ngân sách theo tháng.
* Cảnh báo khi sắp vượt hoặc đã vượt ngân sách.
* Phân tích tổng thu, tổng chi và dòng tiền ròng.
* Tính tỷ lệ tiết kiệm.
* Hiển thị Dashboard, báo cáo và biểu đồ.

Phiên bản 1.0 không triển khai:

* Kết nối ngân hàng thật.
* Chuyển tiền hoặc thanh toán thật.
* Tích hợp ví điện tử thật.
* Đầu tư chứng khoán.
* Phiên bản Web hoặc Mobile.
* Backend API riêng.

---

## 2. BỘ CÔNG NGHỆ BẮT BUỘC

AI Agent không được tự ý thay đổi bộ công nghệ sau:

**Runtime:** `.NET 8 LTS`
**Ngôn ngữ:** `C#`
**Giao diện:** `WPF + XAML`
**Kiến trúc:** `MVVM`
**Cơ sở dữ liệu:** `SQL Server`
**ORM:** `Entity Framework Core`
**Dependency Injection:** `Microsoft.Extensions.DependencyInjection`
**MVVM Toolkit:** `CommunityToolkit.Mvvm`
**Biểu đồ:** `LiveCharts2`
**Giao diện:** `MaterialDesignThemes` hoặc `MahApps.Metro`
**Mã hóa mật khẩu:** `BCrypt.Net-Next`
**Kiểm thử:** `xUnit + Moq`
**Quản lý package:** `NuGet`

Cấu trúc xử lý bắt buộc:

```text
View
→ ViewModel
→ Service
→ Repository
→ AppDbContext
→ SQL Server
```

AI Agent không được:

* Thay WPF bằng WinForms, JavaFX hoặc framework khác.
* Thay SQL Server bằng NoSQL.
* Truy cập database trực tiếp từ View hoặc ViewModel.
* Thêm package mới khi chưa được nhóm phê duyệt.
* Đặt nghiệp vụ tài chính trong code-behind.

---

## 3. NGUYÊN TẮC KIẾN TRÚC

Dự án phải tuân thủ mô hình `MVVM` và phân tầng rõ ràng.

### View

View chỉ chịu trách nhiệm:

* Hiển thị giao diện.
* Data Binding.
* Command Binding.
* Style, Template và DataTrigger.
* Hiển thị trạng thái loading, empty và error.

View không được:

* Gọi Service, Repository hoặc `AppDbContext`.
* Truy vấn database.
* Tính toán tài chính.
* Cập nhật số dư ví.
* Kiểm tra ngân sách.

### ViewModel

ViewModel chịu trách nhiệm:

* Nhận dữ liệu từ View.
* Cung cấp dữ liệu cho View.
* Quản lý trạng thái màn hình.
* Cung cấp Command.
* Gọi Service.
* Hiển thị lỗi phù hợp cho người dùng.

ViewModel phải sử dụng:

* `ObservableObject`
* `[ObservableProperty]`
* `[RelayCommand]`

ViewModel không được:

* Gọi Repository trực tiếp.
* Gọi `AppDbContext`.
* Viết truy vấn Entity Framework.
* Chứa nghiệp vụ tài chính phức tạp.
* Tạo Service bằng từ khóa `new`.

### Service

Service chịu trách nhiệm:

* Kiểm tra business rule.
* Kiểm tra quyền sở hữu dữ liệu.
* Điều phối Repository.
* Cập nhật số dư ví.
* Kiểm tra ngân sách.
* Tính tổng thu, tổng chi và dòng tiền ròng.
* Quản lý database transaction.

### Repository

Repository chịu trách nhiệm:

* Truy vấn database.
* Thêm, sửa và xóa dữ liệu.
* Lọc dữ liệu theo `UserId`.
* Sử dụng `AppDbContext`.
* Trả dữ liệu về Service.

Repository không được:

* Chứa logic giao diện.
* Hiển thị `MessageBox`.
* Tính trạng thái tài chính.
* Được gọi trực tiếp từ ViewModel.

### Xử lý lỗi

* Không được dùng `catch` rỗng.
* Không hiển thị stack trace cho người dùng.
* Không để lộ connection string hoặc thông tin database.
* ViewModel phải bắt lỗi từ Service và hiển thị thông báo dễ hiểu.
* Các thao tác tài chính bị lỗi phải rollback toàn bộ thay đổi.

### Dữ liệu tài chính

* Mọi giá trị tiền phải sử dụng `decimal`.
* Database sử dụng `DECIMAL(18,2)`.
* Không sử dụng `float` hoặc `double` để tính tiền.
* Thêm, sửa hoặc xóa giao dịch phải cập nhật số dư ví trong cùng database transaction.

---

## 4. QUY TẮC ĐẶT TÊN VÀ CẤU TRÚC FILE

Cấu trúc dự án:

```text
StudentFinance/
├── Models/
├── Views/
├── ViewModels/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
├── DataAccess/
│   └── AppDbContext.cs
├── Validators/
├── Resources/
├── appsettings.json
├── App.xaml
└── App.xaml.cs
```

AI Agent không được tự tạo thêm các thư mục sau nếu chưa được nhóm phê duyệt:

```text
Helpers/
Utils/
Common/
Commands/
Converters/
DTOs/
Exceptions/
Configurations/
Managers/
```

Quy tắc đặt tên:

* Class, Method, Property: `PascalCase`
* Biến cục bộ, tham số: `camelCase`
* Field private: `_camelCase`
* Interface: bắt đầu bằng `I`
* Method bất đồng bộ: kết thúc bằng `Async`
* View: kết thúc bằng `View`
* ViewModel: kết thúc bằng `ViewModel`
* Service: kết thúc bằng `Service`
* Repository: kết thúc bằng `Repository`
* Validator: kết thúc bằng `Validator`

Ví dụ:

```text
LoginView.xaml
LoginViewModel.cs
IAuthenticationService.cs
AuthenticationService.cs
IUserRepository.cs
UserRepository.cs
LoginValidator.cs
```

Quy tắc tổ chức code:

* Một method không nên vượt quá 40 dòng.
* Một class không nên vượt quá 300 dòng.
* Một class chỉ có một trách nhiệm chính.
* Không tạo class chung chung như `CommonService`, `FinanceHelper` hoặc `AppManager`.
* Không copy-paste logic giữa các Service hoặc ViewModel.
* Không sử dụng `.Result`, `.Wait()` hoặc `Thread.Sleep()` trên UI Thread.

---

## 5. CÁC MẪU BỊ CẤM

AI Agent tuyệt đối không được:

* Lưu mật khẩu dạng plain text.
* Sử dụng MD5 hoặc SHA1 để lưu mật khẩu.
* Hardcode connection string, mật khẩu, token hoặc API key.
* Commit secret vào Git.
* Sử dụng `float` hoặc `double` cho dữ liệu tiền.
* Đặt business logic trong View hoặc code-behind.
* Gọi Repository từ ViewModel.
* Gọi `AppDbContext` từ ViewModel hoặc View.
* Viết SQL trực tiếp trong View hoặc ViewModel.
* Bỏ qua validation dữ liệu.
* Bỏ điều kiện `UserId` khi truy vấn dữ liệu cá nhân.
* Cho phép người dùng xem dữ liệu của tài khoản khác.
* Cho phép chỉnh trực tiếp số dư hiện tại của ví.
* Xóa ví đã có giao dịch.
* Xóa danh mục đã được sử dụng.
* Thêm giao dịch nhưng không cập nhật số dư ví.
* Sửa giao dịch nhưng không hoàn tác giao dịch cũ.
* Xóa giao dịch nhưng không hoàn lại ảnh hưởng lên ví.
* Cập nhật nhiều dữ liệu tài chính mà không sử dụng database transaction.
* Thay đổi business rule khi chưa có spec được phê duyệt.
* Tạo thêm thư mục hoặc package không cần thiết.
* Xóa file hoặc dữ liệu khi chưa xác định rõ ảnh hưởng.
* Dùng `catch` rỗng.
* Để lại `TODO`, `FIXME`, code debug hoặc `Console.WriteLine`.
* Refactor toàn bộ project khi task chỉ yêu cầu sửa một chức năng.

Các business rule bắt buộc:

* Số tiền giao dịch phải lớn hơn `0`.
* Ngày giao dịch không được lớn hơn ngày hiện tại.
* Income làm tăng số dư ví.
* Expense làm giảm số dư ví.
* Không cho phép số dư ví âm.
* Danh mục Income chỉ dùng cho giao dịch Income.
* Danh mục Expense chỉ dùng cho giao dịch Expense.
* Ngân sách chỉ áp dụng cho danh mục Expense.
* Một danh mục chỉ có một ngân sách trong cùng tháng và năm.
* Từ 80% ngân sách phải cảnh báo sắp vượt.
* Từ 100% ngân sách phải cảnh báo đã vượt.
* Dòng tiền ròng bằng tổng thu trừ tổng chi.
* Không được chia cho `0` khi tính tỷ lệ tiết kiệm.

---

## 6. ĐỊNH NGHĨA HOÀN THÀNH CHO MỖI TASK

Một task chỉ được xem là hoàn thành khi:

```text
[ ] Đã đọc CONSTITUTION.md và AGENTS.md
[ ] Đã đọc CONTEXT.md và spec.md của chức năng
[ ] Code đúng phạm vi task
[ ] Tuân thủ kiến trúc MVVM
[ ] View không chứa business logic
[ ] ViewModel không gọi Repository hoặc AppDbContext
[ ] Service kiểm tra đầy đủ business rule
[ ] Repository lọc dữ liệu theo UserId
[ ] Dữ liệu tiền sử dụng decimal
[ ] Giao dịch và số dư ví được cập nhật đồng bộ
[ ] Có database transaction cho thao tác tài chính
[ ] Validation được thực hiện ở giao diện và Service
[ ] Có xử lý trường hợp lỗi
[ ] Không có catch rỗng
[ ] Không có secret trong source code
[ ] Không có TODO, FIXME hoặc debug code
[ ] Build thành công
[ ] Không có compiler warning chưa xử lý
[ ] Unit Test được bổ sung và chạy thành công
[ ] Các test hiện tại không bị hỏng
[ ] Giao diện có trạng thái loading, empty và error
[ ] Tài liệu liên quan đã được cập nhật
```

AI Agent không được tuyên bố task đã hoàn thành nếu chưa build hoặc chưa chạy test.

Nếu không thể build hoặc chạy test, Agent phải nói rõ phần chưa được xác minh.

---

## 7. QUY ƯỚC GIT

### Tên nhánh

```text
feat/<ten-chuc-nang>
fix/<ten-loi>
spec/<ten-tai-lieu>
refactor/<pham-vi>
test/<pham-vi>
chore/<cong-viec>
```

Ví dụ:

```text
feat/user-login
feat/wallet-management
feat/transaction-history
feat/cash-flow-analysis
fix/wallet-balance
spec/budget-management
```

Không push trực tiếp lên nhánh `main`.

### Commit message

Sử dụng Conventional Commits:

```text
feat(auth): add email login
feat(wallet): add wallet creation
fix(transaction): restore balance after deletion
test(budget): add warning threshold tests
docs(spec): update transaction business rules
refactor(report): simplify monthly summary
```

Commit phải:

* Mô tả rõ thay đổi.
* Chỉ tập trung vào một mục đích.
* Không dùng nội dung chung chung như `update`, `done` hoặc `fix code`.
* Không dài quá 72 ký tự ở dòng đầu.

### File không được commit

```text
bin/
obj/
.vs/
.idea/
*.user
*.suo
.env
secrets.json
appsettings.Development.json
TestResults/
coverage/
```

Không commit:

* Connection string thật.
* Mật khẩu database.
* Google Client Secret.
* API Key.
* Access Token.
* Refresh Token.
* File credential cá nhân.

---

## 8. BỐI CẢNH SPRINT HIỆN TẠI

**Sprint:** [ĐIỀN SPRINT HIỆN TẠI]

**Mục tiêu Sprint:**
[ĐIỀN MỤC TIÊU SPRINT TRONG MỘT CÂU]

**Chức năng đang thực hiện:**

* [ĐIỀN CHỨC NĂNG 1]
* [ĐIỀN CHỨC NĂNG 2]

**Đặc tả đang hoạt động:**

```text
[ĐƯỜNG DẪN CONTEXT.md]
[ĐƯỜNG DẪN spec.md]
[ĐƯỜNG DẪN plan.md]
[ĐƯỜNG DẪN tasks.md]
```

**Thành viên phụ trách:**

* [TÊN THÀNH VIÊN 1]
* [TÊN THÀNH VIÊN 2]

AI Agent chỉ được sửa những file nằm trong phạm vi task hoặc đặc tả hiện tại.

Trước khi thực hiện task lớn, AI Agent phải liệt kê:

```text
File sẽ tạo:
- ...

File sẽ sửa:
- ...

Business rule liên quan:
- ...

Ảnh hưởng đến database:
- ...

Test cần chạy:
- ...
```

Sau khi thực hiện, AI Agent phải báo cáo:

* File đã tạo hoặc chỉnh sửa.
* Business rule đã áp dụng.
* Thay đổi database nếu có.
* Kết quả build.
* Kết quả test.
* Nội dung chưa thể xác minh.
