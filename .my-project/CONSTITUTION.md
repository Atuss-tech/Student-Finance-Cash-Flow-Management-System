# CONSTITUTION.md

## HIẾN PHÁP DỰ ÁN

### Student Finance & Cash Flow Management System

**Tên tiếng Việt:** Ứng dụng Quản lý Chi tiêu và Phân tích Dòng tiền cho Sinh viên
**Loại ứng dụng:** Desktop Application
**Nền tảng:** Windows
**Thời gian thực hiện:** 15 tuần
**Ngày phê chuẩn:** 11/07/2026
**Nhóm:** [Tú, Huy]
**Phiên bản:** 1.0

> **QUY TẮC CỐT LÕI:** Mọi thành viên và AI Agent tham gia dự án phải tuân thủ tài liệu này. Mọi thay đổi ảnh hưởng đến công nghệ, kiến trúc, bảo mật, dữ liệu hoặc quy tắc nghiệp vụ phải được cả nhóm thống nhất trước khi thực hiện.

---

# ĐIỀU 1 – PHẠM VI VÀ MỤC TIÊU DỰ ÁN

## 1.1. Mục tiêu

Hệ thống được xây dựng nhằm giúp sinh viên:

* Quản lý các ví tiền cá nhân.
* Ghi nhận giao dịch thu nhập và chi tiêu.
* Phân loại giao dịch theo danh mục.
* Thiết lập và theo dõi ngân sách theo tháng.
* Phân tích tổng thu, tổng chi và dòng tiền ròng.
* Theo dõi tỷ lệ tiết kiệm.
* Nhận cảnh báo khi sắp vượt hoặc đã vượt ngân sách.
* Xem báo cáo và biểu đồ tài chính trực quan.

## 1.2. Phạm vi bắt buộc

Các chức năng bắt buộc của dự án gồm:

1. Đăng ký tài khoản.
2. Đăng nhập và đăng xuất.
3. Quản lý ví tiền.
4. Quản lý danh mục thu nhập và chi tiêu.
5. Quản lý giao dịch thu nhập.
6. Quản lý giao dịch chi tiêu.
7. Xem và lọc lịch sử giao dịch.
8. Dashboard tổng quan tài chính.
9. Quản lý ngân sách.
10. Cảnh báo ngân sách.
11. Phân tích dòng tiền.
12. Báo cáo và biểu đồ.

## 1.3. Ngoài phạm vi

Dự án không thực hiện:

* Kết nối trực tiếp với ngân hàng thật.
* Chuyển tiền hoặc thanh toán thật.
* Tích hợp ví điện tử thật.
* Giao dịch chứng khoán hoặc tiền mã hóa.
* Tư vấn đầu tư chuyên nghiệp.
* Thu thập thông tin tài chính từ tài khoản ngân hàng của người dùng.
* Xây dựng phiên bản Web hoặc Mobile trong phạm vi phiên bản 1.0.

Các chức năng ngoài phạm vi chỉ được bổ sung khi toàn bộ chức năng bắt buộc đã hoàn thành và được kiểm thử.

---

# ĐIỀU 2 – BỘ CÔNG NGHỆ

Bộ công nghệ dưới đây được xem là cấu hình chuẩn của dự án.

## 2.1. Nền tảng và ngôn ngữ

* Runtime: `.NET 8 LTS`
* Ngôn ngữ: `C#`
* Giao diện desktop: `WPF`
* Giao diện khai báo: `XAML`
* Hệ điều hành mục tiêu: `Windows 10/11`
* Trình quản lý package: `NuGet`

Không tự ý chuyển sang WinForms, JavaFX, Electron hoặc framework khác khi chưa được cả nhóm chấp thuận.

## 2.2. Kiến trúc

Dự án bắt buộc áp dụng:

* Mô hình `MVVM`.
* Phân tầng `View – ViewModel – Service – Repository – DataAccess`.
* Dependency Injection.
* Repository Pattern cho truy cập dữ liệu.
* Service Layer cho xử lý nghiệp vụ.
* Command Binding cho thao tác từ giao diện.

Không đặt nghiệp vụ tài chính trực tiếp trong View hoặc code-behind.

## 2.3. Thư viện chính

* `CommunityToolkit.Mvvm`: MVVM, ObservableProperty và RelayCommand.
* `Microsoft.Extensions.DependencyInjection`: Dependency Injection.
* `Microsoft.Extensions.Configuration`: Đọc cấu hình ứng dụng.
* `Entity Framework Core`: Truy cập dữ liệu.
* `Microsoft.EntityFrameworkCore.SqlServer`: Kết nối SQL Server.
* `LiveCharts2`: Biểu đồ báo cáo.
* `MaterialDesignThemes` hoặc `MahApps.Metro`: Hỗ trợ giao diện.
* `BCrypt.Net-Next`: Băm mật khẩu.
* `xUnit`: Unit Test.
* `Moq`: Mock dependency trong kiểm thử.

Không thêm package mới nếu package đó:

* Không có mục đích rõ ràng.
* Trùng chức năng với package hiện có.
* Không được bảo trì.
* Có lỗ hổng bảo mật nghiêm trọng.
* Làm tăng độ phức tạp không cần thiết.

## 2.4. Cơ sở dữ liệu

* Hệ quản trị cơ sở dữ liệu: `SQL Server`.
* Truy cập dữ liệu thông qua `Entity Framework Core`.
* Mọi thay đổi schema phải được quản lý thống nhất bằng EF Core Migration hoặc script SQL đã được cả nhóm phê duyệt.
* Nếu sử dụng EF Core Migration, các file do công cụ tự sinh không được xem là một tầng kiến trúc riêng và không cần khai báo trong cấu trúc thư mục chuẩn.
* Không sửa trực tiếp database production khi chưa có lịch sử thay đổi schema tương ứng.
* Không sử dụng NoSQL trong phiên bản 1.0.
* Không đặt câu lệnh SQL trực tiếp trong View hoặc ViewModel.

---

# ĐIỀU 3 – CẤU TRÚC DỰ ÁN

Dự án phải được tổ chức theo mô hình `MVVM` và phân tách rõ trách nhiệm giữa giao diện, trạng thái giao diện, xử lý nghiệp vụ và truy cập dữ liệu.

Cấu trúc thư mục chuẩn:

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

Đây là cấu trúc bắt buộc của phiên bản hiện tại. Không tạo sẵn các thư mục `DTOs`, `Exceptions`, `Commands`, `Converters`, `Helpers`, `Configurations`, `Migrations`, `Common` hoặc `Utils` nếu chưa có nhu cầu thực tế và chưa được cả nhóm thống nhất.

Việc không có thư mục riêng không có nghĩa là cấm hoàn toàn kỹ thuật tương ứng:

* Command được tạo bằng `CommunityToolkit.Mvvm` ngay trong ViewModel.
* Định dạng hiển thị đơn giản được xử lý bằng `StringFormat`, `Style` hoặc `DataTrigger` trong XAML.
* Lỗi nghiệp vụ có thể sử dụng exception chuẩn hoặc class lỗi cụ thể đặt cạnh thành phần liên quan khi thực sự cần.
* Cấu hình Entity Framework Core đơn giản được khai báo bằng Data Annotation trong Model.
* Cấu hình quan hệ hoặc ràng buộc phức tạp được khai báo trong `AppDbContext`.
* File EF Core Migration do công cụ tự sinh có thể xuất hiện khi dự án sử dụng Migration nhưng không được xem là một tầng nghiệp vụ của hệ thống.

## 3.1. Models

Thư mục `Models` chứa các class đại diện cho dữ liệu chính của hệ thống, ví dụ:

```text
Models/
├── User.cs
├── Wallet.cs
├── Category.cs
├── Transaction.cs
└── Budget.cs
```

Model có trách nhiệm:

* Đại diện cho dữ liệu nghiệp vụ.
* Khai báo thuộc tính và quan hệ giữa các đối tượng.
* Sử dụng Data Annotation cho cấu hình dữ liệu cơ bản như `Required`, `MaxLength`, khóa chính, khóa ngoại và kiểu dữ liệu tiền.
* Có thể chứa hành vi đơn giản gắn trực tiếp với chính đối tượng đó.

Model không được:

* Truy cập database.
* Gọi Service hoặc Repository.
* Tham chiếu đến View hoặc ViewModel.
* Hiển thị `MessageBox`.
* Chứa logic giao diện.
* Chứa nghiệp vụ tài chính phức tạp như tính dòng tiền, kiểm tra ngân sách hoặc cập nhật số dư ví.

## 3.2. Views

Thư mục `Views` chứa các `Window`, `Page` hoặc `UserControl` của WPF.

View có trách nhiệm:

* Khai báo giao diện bằng XAML.
* Hiển thị dữ liệu từ ViewModel.
* Sử dụng Data Binding.
* Binding nút bấm với Command.
* Khai báo Style, Template và DataTrigger.
* Hiển thị trạng thái loading, empty, success và error.

View không được:

* Truy cập `AppDbContext`.
* Gọi Repository.
* Truy vấn database.
* Tính tổng thu, tổng chi hoặc dòng tiền.
* Kiểm tra ngân sách.
* Cập nhật số dư ví.
* Xử lý nghiệp vụ đăng nhập, giao dịch hoặc báo cáo.

Code-behind chỉ được dùng cho xử lý thuần giao diện mà XAML hoặc Binding không giải quyết hợp lý, chẳng hạn như kéo cửa sổ, focus control hoặc animation.

## 3.3. ViewModels

Thư mục `ViewModels` chứa trạng thái và hành vi của từng màn hình.

ViewModel có trách nhiệm:

* Cung cấp dữ liệu cho View.
* Nhận dữ liệu người dùng nhập.
* Quản lý trạng thái hiển thị.
* Gọi Service để thực hiện nghiệp vụ.
* Cung cấp Command cho View.
* Quản lý thông báo validation và lỗi hiển thị.
* Tải lại dữ liệu sau thao tác thêm, sửa hoặc xóa.

ViewModel phải sử dụng `CommunityToolkit.Mvvm` khi phù hợp, bao gồm:

* `ObservableObject`
* `[ObservableProperty]`
* `[RelayCommand]`
* `[AsyncRelayCommand]` hoặc Command bất đồng bộ tương đương

ViewModel không được:

* Gọi trực tiếp `AppDbContext`.
* Gọi trực tiếp Repository.
* Viết SQL hoặc truy vấn Entity Framework.
* Cập nhật số dư ví trực tiếp.
* Chứa business rule phức tạp.
* Tham chiếu trực tiếp đến control WPF.
* Tự khởi tạo Service bằng từ khóa `new`.

Dependency phải được truyền qua constructor.

## 3.4. Services

Thư mục `Services` chứa logic nghiệp vụ của hệ thống.

```text
Services/
├── Interfaces/
└── Implementations/
```

`Interfaces` khai báo hợp đồng của Service. `Implementations` chứa phần triển khai cụ thể.

Service có trách nhiệm:

* Kiểm tra business rule.
* Kiểm tra quyền sở hữu dữ liệu theo `UserId`.
* Điều phối một hoặc nhiều Repository.
* Xử lý đăng ký và đăng nhập.
* Cập nhật số dư ví.
* Kiểm tra loại danh mục.
* Kiểm tra số dư trước khi chi tiêu.
* Kiểm tra và tính trạng thái ngân sách.
* Tính tổng thu, tổng chi, dòng tiền ròng và tỷ lệ tiết kiệm.
* Quản lý database transaction khi một thao tác cập nhật nhiều dữ liệu liên quan.
* Trả kết quả xử lý về ViewModel.

Service không được:

* Tham chiếu View hoặc control WPF.
* Hiển thị `MessageBox`.
* Chứa XAML.
* Thực hiện định dạng chỉ phục vụ giao diện.

## 3.5. Repositories

Thư mục `Repositories` chịu trách nhiệm truy cập dữ liệu.

```text
Repositories/
├── Interfaces/
└── Implementations/
```

`Interfaces` khai báo các thao tác dữ liệu. `Implementations` sử dụng `AppDbContext` để thực thi các thao tác đó.

Repository có trách nhiệm:

* Truy vấn database.
* Thêm, cập nhật và xóa hoặc ngừng sử dụng dữ liệu.
* Lọc dữ liệu theo `UserId`.
* Sử dụng Entity Framework Core.
* Sử dụng `AsNoTracking()` cho truy vấn chỉ đọc khi phù hợp.
* Trả dữ liệu về Service.

Repository không được:

* Bị gọi trực tiếp từ View hoặc ViewModel.
* Hiển thị thông báo giao diện.
* Quyết định trạng thái tài chính.
* Tính dòng tiền.
* Kiểm tra cảnh báo ngân sách.
* Điều phối toàn bộ luồng nghiệp vụ của nhiều đối tượng.

## 3.6. DataAccess

Thư mục `DataAccess` chỉ chứa lớp quản lý Entity Framework Core của dự án:

```text
DataAccess/
└── AppDbContext.cs
```

`AppDbContext` có trách nhiệm:

* Kế thừa `DbContext`.
* Khai báo các `DbSet`.
* Quản lý kết nối database.
* Ánh xạ Model với bảng dữ liệu.
* Khai báo quan hệ và ràng buộc phức tạp trong `OnModelCreating()`.
* Khai báo unique index nhiều cột.
* Hỗ trợ transaction và lưu thay đổi.

Các cấu hình đơn giản được đặt trực tiếp trong Model bằng Data Annotation. Dự án hiện tại không sử dụng thư mục `Configurations`.

`AppDbContext` không được:

* Chứa logic giao diện.
* Chứa nghiệp vụ đăng nhập.
* Tính tổng thu, tổng chi hoặc dòng tiền.
* Kiểm tra ngân sách.
* Bị gọi trực tiếp từ View hoặc ViewModel.

## 3.7. Validators

Thư mục `Validators` chứa các class kiểm tra tính hợp lệ của dữ liệu đầu vào.

Validator có thể kiểm tra:

* Trường bắt buộc.
* Định dạng email.
* Độ dài mật khẩu.
* Xác nhận mật khẩu.
* Số tiền phải lớn hơn `0`.
* Ngày giao dịch không được nằm trong tương lai.
* Tên ví hoặc tên danh mục không được để trống.
* Tháng, năm và số tiền ngân sách hợp lệ.

Validator chỉ kiểm tra dữ liệu đầu vào và không thay thế business rule trong Service.

Ví dụ:

* Kiểm tra `Amount > 0` có thể đặt trong Validator.
* Kiểm tra ví có thuộc người dùng không phải đặt trong Service.
* Kiểm tra số dư ví có đủ không phải đặt trong Service.
* Kiểm tra ngân sách có trùng không phải được bảo vệ bởi Service và database.

## 3.8. Resources

Thư mục `Resources` chứa tài nguyên giao diện dùng chung, chẳng hạn như:

* Màu sắc.
* Font chữ.
* Style.
* ControlTemplate.
* Icon.
* Hình ảnh.
* ResourceDictionary.

Resources không được chứa business logic, dữ liệu người dùng, connection string hoặc thông tin bí mật.

## 3.9. appsettings.json

File `appsettings.json` chứa cấu hình ứng dụng như:

* Tên ứng dụng.
* Đơn vị tiền tệ mặc định.
* Cấu hình logging.
* Connection string trong môi trường development khi được bảo vệ phù hợp.
* Các cấu hình không nhạy cảm khác.

Không được commit mật khẩu database thật, Client Secret, API Key, Access Token hoặc Refresh Token vào file này.

## 3.10. App.xaml

`App.xaml` chứa tài nguyên dùng chung ở cấp ứng dụng và đăng ký các `ResourceDictionary`.

File này không được chứa business logic.

## 3.11. App.xaml.cs

`App.xaml.cs` là điểm khởi động của ứng dụng và có trách nhiệm:

* Cấu hình Dependency Injection.
* Đăng ký `AppDbContext`.
* Đăng ký Repository.
* Đăng ký Service.
* Đăng ký ViewModel và View.
* Khởi tạo màn hình đầu tiên.
* Quản lý vòng đời ứng dụng.

`App.xaml.cs` không được chứa nghiệp vụ tài chính, xử lý đăng nhập, truy vấn phục vụ giao diện hoặc connection string thật được hardcode.

## 3.12. Luồng phụ thuộc chuẩn

Luồng xử lý bắt buộc:

```text
View
→ ViewModel
→ Service Interface
→ Service Implementation
→ Repository Interface
→ Repository Implementation
→ AppDbContext
→ SQL Server
```

Dữ liệu trả về theo chiều ngược lại:

```text
SQL Server
→ AppDbContext
→ Repository
→ Service
→ ViewModel
→ View
```

Không được phép tồn tại các quan hệ sau:

```text
View → Repository
View → AppDbContext
ViewModel → Repository
ViewModel → AppDbContext
Model → View
Model → ViewModel
Repository → ViewModel
Service → View
```

Mỗi class phải có trách nhiệm rõ ràng. Không tạo class chung chung như `CommonService`, `DatabaseHelper`, `FinanceHelper`, `AppManager` hoặc `Utils` nếu không thể xác định một trách nhiệm duy nhất.

---

# ĐIỀU 4 – TIÊU CHUẨN VIẾT CODE

## 4.1. Cấu hình C#

Project phải bật:

```xml
<Nullable>enable</Nullable>
<ImplicitUsings>enable</ImplicitUsings>
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
```

Không được bỏ qua cảnh báo compiler nếu chưa xác định rõ nguyên nhân.

## 4.2. Quy tắc đặt tên

* Class, Method, Property: `PascalCase`
* Biến cục bộ và tham số: `camelCase`
* Interface: bắt đầu bằng chữ `I`
* Field private: `_camelCase`
* Method bất đồng bộ: kết thúc bằng `Async`
* View: kết thúc bằng `View`
* ViewModel: kết thúc bằng `ViewModel`
* Service: kết thúc bằng `Service`
* Repository: kết thúc bằng `Repository`

Ví dụ:

```csharp
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(
        ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Transaction> CreateTransactionAsync(
        Transaction transaction)
    {
        // Business logic
    }
}
```

## 4.3. Giới hạn code

* Một method không nên vượt quá 40 dòng.
* Một class không nên vượt quá 300 dòng.
* Một ViewModel không được quản lý nhiều màn hình không liên quan.
* Một method chỉ nên thực hiện một nhiệm vụ chính.
* Không tạo các class chung chung như `Helper`, `Common` hoặc `Utils`; mỗi class phải có trách nhiệm rõ ràng.
* Không copy-paste logic giữa các ViewModel hoặc Service.

Nếu vượt giới hạn, phải xem xét tách method, class hoặc service.

## 4.4. Comments

Comment chỉ được dùng để giải thích:

* Tại sao phải xử lý theo cách đó.
* Quy tắc nghiệp vụ đặc biệt.
* Giới hạn kỹ thuật không thể hiện rõ bằng code.

Không comment để mô tả lại điều code đã thể hiện rõ.

Không để lại:

* `TODO`
* `FIXME`
* Code đã comment.
* Debug code.
* `Console.WriteLine` không cần thiết.

trước khi merge vào nhánh chính.

## 4.5. Async/Await

Các thao tác sau phải được thực hiện bất đồng bộ:

* Truy vấn database.
* Ghi dữ liệu.
* Đọc hoặc xuất file.
* Tải dữ liệu báo cáo.
* Tác vụ có thể làm treo giao diện.

Không sử dụng:

```csharp
.Result
.Wait()
Thread.Sleep()
```

trên UI Thread.

---

# ĐIỀU 5 – QUY TẮC DỮ LIỆU TÀI CHÍNH

## 5.1. Kiểu dữ liệu tiền tệ

Mọi giá trị tiền phải sử dụng kiểu:

```csharp
decimal
```

Tuyệt đối không sử dụng `float` hoặc `double` để lưu và tính toán tiền.

Database phải sử dụng kiểu dữ liệu tương đương:

```sql
DECIMAL(18,2)
```

## 5.2. Quy tắc làm tròn

* Giá trị tiền được làm tròn tối đa hai chữ số thập phân.
* Quy tắc làm tròn phải nhất quán trong toàn bộ hệ thống.
* Không làm tròn ở nhiều tầng khác nhau gây sai lệch dữ liệu.
* Số liệu hiển thị phải khớp với số liệu được lưu trong database.

## 5.3. Quy tắc ngày tháng

* `CreatedAt` và `UpdatedAt` phải được lưu theo UTC.
* Ngày giao dịch được lưu dưới dạng ngày nghiệp vụ.
* Ngày giao dịch không được lớn hơn ngày hiện tại.
* Việc lọc theo tháng phải bao gồm đầy đủ giao dịch từ ngày đầu đến ngày cuối tháng.

## 5.4. Tính toàn vẹn dữ liệu

Mọi thao tác làm thay đổi giao dịch và số dư ví phải được thực hiện trong cùng một database transaction.

Ví dụ khi thêm giao dịch chi tiêu:

1. Kiểm tra người dùng sở hữu ví.
2. Kiểm tra danh mục thuộc loại Expense.
3. Kiểm tra số tiền hợp lệ.
4. Kiểm tra số dư ví.
5. Lưu giao dịch.
6. Trừ số dư ví.
7. Kiểm tra ngân sách.
8. Commit transaction.

Nếu bất kỳ bước nào thất bại, toàn bộ thao tác phải rollback.

---

# ĐIỀU 6 – QUY TẮC NGHIỆP VỤ BẮT BUỘC

## BR01 – Xác thực người dùng

Người dùng phải đăng nhập trước khi truy cập các chức năng quản lý tài chính.

## BR02 – Cô lập dữ liệu

Mỗi người dùng chỉ được phép xem và thao tác với dữ liệu thuộc tài khoản của mình.

Mọi truy vấn Wallet, Category, Transaction và Budget phải có điều kiện `UserId`.

Không được dựa hoàn toàn vào dữ liệu `UserId` gửi từ giao diện.

## BR03 – Số tiền giao dịch

Số tiền giao dịch phải lớn hơn `0`.

## BR04 – Ngày giao dịch

Ngày giao dịch không được lớn hơn ngày hiện tại.

## BR05 – Giao dịch thu nhập

Giao dịch Income phải:

* Sử dụng danh mục Income.
* Làm tăng số dư ví.
* Được tính vào tổng thu.

## BR06 – Giao dịch chi tiêu

Giao dịch Expense phải:

* Sử dụng danh mục Expense.
* Làm giảm số dư ví.
* Được tính vào tổng chi.

## BR07 – Không cho phép âm ví

Trong phiên bản 1.0, hệ thống không cho phép số dư ví âm.

Giao dịch chi tiêu không được lớn hơn số dư hiện tại của ví.

## BR08 – Danh mục

Một danh mục chỉ được thuộc một loại:

* `Income`
* `Expense`

Không được thay đổi loại danh mục nếu danh mục đã được sử dụng trong giao dịch.

## BR09 – Xóa danh mục

Không xóa vật lý danh mục đã được sử dụng.

Danh mục đã có giao dịch phải được chuyển sang trạng thái không hoạt động.

## BR10 – Xóa ví

Không xóa vật lý ví đã phát sinh giao dịch.

Ví không còn sử dụng phải được chuyển sang trạng thái `IsActive = false`.

## BR11 – Ngân sách

* Ngân sách chỉ được áp dụng cho danh mục Expense.
* Số tiền ngân sách phải lớn hơn `0`.
* Một người dùng chỉ có một ngân sách cho một danh mục trong cùng tháng và năm.

Database phải có unique constraint tương ứng với:

```text
UserId + CategoryId + Month + Year
```

## BR12 – Cảnh báo ngân sách

* Dưới 80%: Bình thường.
* Từ 80% đến dưới 100%: Sắp vượt ngân sách.
* Từ 100% trở lên: Đã vượt ngân sách.

## BR13 – Dòng tiền ròng

```text
Dòng tiền ròng = Tổng thu - Tổng chi
```

## BR14 – Tỷ lệ tiết kiệm

```text
Tỷ lệ tiết kiệm = Dòng tiền ròng / Tổng thu × 100
```

Nếu tổng thu bằng `0`, hệ thống không được thực hiện phép chia.

Trong trường hợp này, tỷ lệ tiết kiệm được hiển thị là `0%` hoặc `Chưa có dữ liệu`.

## BR15 – Chi tiêu trung bình

```text
Chi tiêu trung bình mỗi ngày
= Tổng chi trong tháng / Số ngày đã trôi qua trong tháng
```

## BR16 – Dự báo chi tiêu cuối tháng

```text
Chi tiêu dự báo
= Chi tiêu trung bình mỗi ngày × Tổng số ngày trong tháng
```

Dự báo chỉ mang tính tham khảo và phải được ghi rõ trên giao diện.

## BR17 – Trạng thái tài chính

| Điều kiện                                   | Trạng thái |
| ------------------------------------------- | ---------- |
| Dòng tiền ròng > 0 và tỷ lệ tiết kiệm ≥ 20% | Tốt        |
| Dòng tiền ròng > 0 và tỷ lệ tiết kiệm < 20% | Trung bình |
| Dòng tiền ròng = 0                          | Cân bằng   |
| Dòng tiền ròng < 0                          | Cảnh báo   |

## BR18 – Sửa giao dịch

Khi sửa giao dịch, hệ thống phải:

1. Hoàn tác ảnh hưởng của giao dịch cũ.
2. Kiểm tra dữ liệu mới.
3. Áp dụng ảnh hưởng của giao dịch mới.
4. Cập nhật số dư ví.
5. Tính lại ngân sách và báo cáo.

Tất cả bước phải nằm trong cùng một database transaction.

## BR19 – Xóa giao dịch

Khi xóa giao dịch:

* Xóa Income phải trừ lại số tiền đã cộng vào ví.
* Xóa Expense phải hoàn lại số tiền đã trừ khỏi ví.
* Phải cập nhật lại ngân sách và báo cáo.
* Không được để số dư ví sai lệch với lịch sử giao dịch.

## BR20 – Số dư ví

Không được cho phép người dùng sửa trực tiếp số dư hiện tại của ví.

Mọi thay đổi số dư sau khi tạo ví phải đến từ:

* Giao dịch Income.
* Giao dịch Expense.
* Giao dịch điều chỉnh được hệ thống ghi nhận rõ ràng.

---

# ĐIỀU 7 – CHÍNH SÁCH BẢO MẬT

## 7.1. Mật khẩu

* Mật khẩu không được lưu dưới dạng plain text.
* Không sử dụng MD5 hoặc SHA1 để lưu mật khẩu.
* Sử dụng `BCrypt.Net-Next`.
* Work factor tối thiểu là `12`.
* Không ghi mật khẩu vào log.
* Không trả `PasswordHash` về ViewModel hoặc bất kỳ dữ liệu nào dùng để hiển thị.

## 7.2. Đăng nhập Gmail

Nếu triển khai Google OAuth:

* Không hardcode Client ID hoặc Client Secret.
* Không commit file chứa OAuth secret.
* Token phải được bảo vệ bằng cơ chế phù hợp của Windows.
* Không lưu Access Token dạng plain text nếu không thực sự cần thiết.
* Chỉ yêu cầu các scope tối thiểu phục vụ đăng nhập.
* Không yêu cầu quyền truy cập Gmail hoặc Google Drive.

## 7.3. Chuỗi kết nối

Connection string phải được lưu trong một trong các vị trí:

* User Secrets trong môi trường development.
* Biến môi trường.
* File cấu hình cục bộ không được commit.
* Windows Credential Manager khi phù hợp.

Không commit:

```text
Password=
User Id=
ClientSecret=
ApiKey=
AccessToken=
```

lên GitHub.

## 7.4. SQL Injection

* Ưu tiên Entity Framework Core.
* Câu SQL thủ công phải sử dụng parameter.
* Không nối chuỗi dữ liệu người dùng vào câu SQL.
* Không xây dựng câu truy vấn bằng string interpolation với dữ liệu đầu vào.

## 7.5. Validation

Mọi dữ liệu đầu vào phải được kiểm tra ở hai tầng:

1. ViewModel: kiểm tra để hiển thị lỗi cho người dùng.
2. Service: kiểm tra lại trước khi xử lý nghiệp vụ.

Không tin tưởng dữ liệu chỉ vì dữ liệu đã được kiểm tra ở giao diện.

## 7.6. Logging

Log không được chứa:

* Mật khẩu.
* Password hash.
* OAuth token.
* Connection string.
* Dữ liệu nhạy cảm không cần thiết.

Thông báo lỗi hiển thị cho người dùng phải dễ hiểu nhưng không để lộ cấu trúc database hoặc stack trace.

---

# ĐIỀU 8 – QUY TẮC MVVM

## 8.1. View

View chỉ được phép chứa:

* XAML.
* Binding.
* Style và Template.
* Animation giao diện.
* Xử lý thuần giao diện không thể thực hiện hợp lý qua binding.

## 8.2. Code-behind

Code-behind chỉ được sử dụng cho:

* Focus control.
* Window drag.
* Animation.
* Điều khiển giao diện đặc thù.
* Tương tác với API chỉ có ở tầng View.

Không được đặt trong code-behind:

* Truy vấn database.
* Tính toán tổng thu, tổng chi.
* Kiểm tra ngân sách.
* Xử lý đăng nhập.
* Thêm, sửa hoặc xóa giao dịch.

## 8.3. ViewModel

ViewModel phải:

* Kế thừa `ObservableObject` khi cần.
* Sử dụng `RelayCommand` hoặc `AsyncRelayCommand`.
* Không chứa tham chiếu trực tiếp đến control WPF.
* Không hiển thị `MessageBox` trực tiếp nếu có thể thay bằng Dialog Service.
* Không tạo trực tiếp Repository.
* Nhận dependency thông qua constructor.

## 8.4. Model và dữ liệu hiển thị

Không sử dụng Entity database làm toàn bộ trạng thái của một form phức tạp khi việc đó khiến View có thể sửa các field không được phép.

Trong cấu trúc hiện tại, dự án không tạo thư mục `DTOs`. ViewModel phải:

* Chỉ công khai những property mà View thực sự cần.
* Nhận kết quả từ Service và chuyển thành trạng thái hiển thị phù hợp.
* Không cho View chỉnh sửa trực tiếp khóa chính, `UserId`, `PasswordHash` hoặc các field hệ thống.
* Sử dụng Model trực tiếp khi dữ liệu đơn giản và an toàn.
* Chỉ bổ sung một class dữ liệu chuyên biệt vào `Models` khi thực sự cần tổng hợp dữ liệu cho Dashboard hoặc báo cáo và đã được cả nhóm thống nhất.

---

# ĐIỀU 9 – GIAO DIỆN VÀ TRẢI NGHIỆM NGƯỜI DÙNG

## 9.1. Tính nhất quán

Toàn bộ ứng dụng phải thống nhất:

* Font chữ.
* Màu sắc.
* Kích thước nút.
* Khoảng cách.
* Cách hiển thị validation.
* Cách hiển thị dialog.
* Cách hiển thị trạng thái loading.

## 9.2. Trạng thái màn hình

Mỗi màn hình tải dữ liệu phải xử lý ít nhất bốn trạng thái:

1. Loading.
2. Có dữ liệu.
3. Không có dữ liệu.
4. Có lỗi.

Không được để ứng dụng đứng yên mà không thông báo trong khi đang tải dữ liệu.

## 9.3. Định dạng tiền

Tiền Việt Nam phải được hiển thị theo định dạng phù hợp:

```text
1.500.000 ₫
```

Phần định dạng chỉ phục vụ hiển thị, không được dùng chuỗi đã định dạng để tính toán.

## 9.4. Validation giao diện

* Hiển thị lỗi gần trường dữ liệu bị sai.
* Không chỉ hiển thị một thông báo lỗi chung.
* Nút lưu có thể bị vô hiệu hóa khi dữ liệu chưa hợp lệ.
* Không xóa dữ liệu người dùng đã nhập khi validation thất bại.

## 9.5. Thao tác nguy hiểm

Các thao tác sau phải có hộp thoại xác nhận:

* Xóa giao dịch.
* Ngừng sử dụng ví.
* Ngừng sử dụng danh mục.
* Đăng xuất khi có dữ liệu chưa lưu.
* Xóa hoặc thay đổi ngân sách quan trọng.

---

# ĐIỀU 10 – HIỆU NĂNG

* Không tải toàn bộ lịch sử giao dịch nếu số lượng dữ liệu lớn.
* Danh sách giao dịch phải hỗ trợ phân trang hoặc giới hạn số bản ghi.
* Không thực hiện truy vấn database trong vòng lặp.
* Sử dụng projection thay vì tải toàn bộ Entity khi chỉ cần một số field.
* Truy vấn chỉ đọc phải cân nhắc `AsNoTracking()`.
* Dashboard không được thực hiện nhiều truy vấn trùng lặp.
* Biểu đồ chỉ tải dữ liệu cần thiết trong khoảng thời gian được chọn.
* UI không được bị treo trong quá trình tải báo cáo.

Mục tiêu hiệu năng trong môi trường local:

* Màn hình thông thường: tải dưới 2 giây.
* Dashboard: tải dưới 3 giây.
* Tìm kiếm và lọc: phản hồi dưới 1 giây với tập dữ liệu thông thường.

---

# ĐIỀU 11 – QUY TRÌNH LÀM VIỆC VỚI GIT

## 11.1. Các nhánh chính

* `main`: phiên bản ổn định.
* `develop`: tích hợp tính năng trước khi đưa lên main.
* Nhánh chức năng được tạo từ `develop`.

Không push trực tiếp vào `main`.

## 11.2. Đặt tên nhánh

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
feat/cash-flow-analysis
fix/wallet-balance-calculation
spec/transaction-management
```

## 11.3. Commit message

Sử dụng Conventional Commits:

```text
feat(auth): add email login
fix(wallet): recalculate balance after deleting transaction
test(budget): add budget warning tests
docs(spec): update cash flow rules
refactor(transaction): split transaction service
```

Commit message:

* Viết rõ thay đổi.
* Không dùng nội dung như `update`, `fix code`, `done`.
* Không dài quá 72 ký tự ở dòng đầu.
* Một commit chỉ tập trung vào một mục đích chính.

## 11.4. Pull Request

Mỗi Pull Request phải:

* Có tiêu đề rõ ràng.
* Có mô tả chức năng.
* Có danh sách thay đổi.
* Có hướng dẫn kiểm thử.
* Có ảnh giao diện nếu thay đổi UI.
* Có ít nhất một thành viên còn lại review.
* Build thành công.
* Test thành công.
* Không còn conflict.

PR nên dưới 400 dòng code thay đổi, không tính:

* File EF Core Migration tự sinh nếu dự án sử dụng Migration.
* File designer tự sinh.
* File lock package.

## 11.5. File không được commit

```text
bin/
obj/
.vs/
.idea/
*.user
*.suo
appsettings.Development.json
.env
secrets.json
TestResults/
coverage/
```

---

# ĐIỀU 12 – YÊU CẦU KIỂM THỬ

## 12.1. Framework

* Unit Test: `xUnit`
* Mock: `Moq`
* Assertion mở rộng: có thể sử dụng `FluentAssertions`

## 12.2. Unit Test bắt buộc

Phải có Unit Test cho:

* Đăng nhập bằng email và mật khẩu.
* Kiểm tra email trùng khi đăng ký.
* Tạo giao dịch Income.
* Tạo giao dịch Expense.
* Kiểm tra số dư ví.
* Sửa giao dịch.
* Xóa giao dịch.
* Kiểm tra loại danh mục.
* Tính tổng thu.
* Tính tổng chi.
* Tính dòng tiền ròng.
* Tính tỷ lệ tiết kiệm.
* Kiểm tra cảnh báo ngân sách 80%.
* Kiểm tra vượt ngân sách 100%.
* Phân loại trạng thái tài chính.
* Trường hợp tổng thu bằng 0.
* Trường hợp giao dịch có ngày trong tương lai.

## 12.3. Integration Test

Integration Test phải kiểm tra:

* Repository đọc và ghi dữ liệu đúng.
* Quan hệ User – Wallet – Transaction.
* Unique constraint của Budget.
* Transaction rollback khi cập nhật số dư thất bại.
* Cô lập dữ liệu giữa hai người dùng.
* Cấu trúc database có thể được khởi tạo đầy đủ trên một database test mới.

Không sử dụng EF Core InMemory để thay thế hoàn toàn kiểm thử database quan hệ.

Có thể sử dụng:

* SQL Server LocalDB.
* SQL Server Test Container.
* Database test riêng.

## 12.4. Coverage

* Service và business logic mới: tối thiểu 80%.
* Toàn bộ phần Core của hệ thống: tối thiểu 70%.
* View và XAML không bắt buộc đạt cùng mức coverage.

Không chạy theo coverage bằng cách viết test không có ý nghĩa.

## 12.5. Quy tắc merge

Không được merge nếu:

* Build thất bại.
* Test hiện tại bị hỏng.
* Có lỗi tính toán tài chính.
* Có lỗi làm sai số dư ví.
* Có lỗi cho phép xem dữ liệu người dùng khác.
* Có secret trong source code.

---

# ĐIỀU 13 – QUY TẮC DÀNH CHO AI AGENT

## 13.1. Tài liệu phải đọc

Trước khi bắt đầu tác vụ, AI Agent phải đọc:

1. `CONSTITUTION.md`
2. `AGENTS.md`
3. `CONTEXT.md`
4. `spec.md` của chức năng
5. `plan.md` hoặc `tasks.md` nếu có
6. Các Model, Service và Repository liên quan

## 13.2. Không được tự ý thay đổi

AI Agent không được tự ý:

* Thay đổi kiến trúc MVVM.
* Thay đổi framework hoặc database.
* Thêm package mới không được yêu cầu.
* Thay đổi business rule.
* Xóa validation.
* Thay đổi schema database.
* Tạo hoặc xóa migration quan trọng.
* Thay đổi cách tính số dư ví.
* Thay đổi công thức tài chính.
* Hardcode connection string hoặc secret.
* Refactor toàn bộ project ngoài phạm vi tác vụ.

## 13.3. Kế hoạch thực hiện

Trước tác vụ lớn, Agent phải trình bày:

* Những file sẽ tạo.
* Những file sẽ sửa.
* Luồng nghiệp vụ liên quan.
* Business rule được áp dụng.
* Nguy cơ ảnh hưởng đến dữ liệu.
* Kế hoạch kiểm thử.

Con người phải xem xét kế hoạch trước khi cho phép Agent thực hiện thay đổi lớn.

## 13.4. Chất lượng code AI

Mọi code do Agent tạo phải:

* Build thành công.
* Tuân thủ MVVM.
* Không đặt nghiệp vụ trong View.
* Không truy cập database trong ViewModel.
* Có validation.
* Xử lý exception phù hợp.
* Có test cho nghiệp vụ mới.
* Không chứa secret.
* Không chứa code giả chưa hoàn thiện.
* Có thể được thành viên trong nhóm giải thích lại.

Không chấp nhận code chỉ vì code chạy được.

## 13.5. Human-led review

Sau mỗi 3–5 tác vụ AI:

* Con người phải review cấu trúc code.
* Kiểm tra duplicate code.
* Kiểm tra ViewModel quá lớn.
* Kiểm tra dependency không cần thiết.
* Kiểm tra business rule.
* Thực hiện refactor khi cần.

---

# ĐIỀU 14 – QUY TRÌNH ĐẶC TẢ

Mỗi chức năng chính phải có tối thiểu:

```text
CONTEXT.md
spec.md
plan.md
tasks.md
```

## 14.1. CONTEXT.md

Mô tả:

* Bối cảnh nghiệp vụ.
* Người sử dụng.
* Mục tiêu.
* Vấn đề cần giải quyết.
* Phạm vi.
* Ngoài phạm vi.

## 14.2. spec.md

Mô tả:

* Functional requirements.
* Business rules.
* Main flow.
* Alternative flow.
* Exception flow.
* Validation.
* Acceptance criteria.

## 14.3. plan.md

Mô tả:

* Kiến trúc thực hiện.
* Các class cần tạo.
* Database thay đổi.
* Dependency.
* Luồng xử lý.
* Kế hoạch test.

## 14.4. tasks.md

Chia chức năng thành các task nhỏ có thể kiểm tra độc lập.

Không bắt đầu code chức năng khi `spec.md` chưa được cả nhóm thống nhất.

---

# ĐIỀU 15 – QUY TRÌNH REVIEW

## 15.1. Spec Review

Spec Review được thực hiện trước khi code mỗi chức năng.

Nội dung kiểm tra:

* Chức năng có nằm trong phạm vi không.
* Business rule có rõ ràng không.
* Main flow có đầy đủ không.
* Exception flow có được xử lý không.
* Dữ liệu có thay đổi không.
* Có ảnh hưởng đến số dư ví không.

## 15.2. Code Review

Code Review phải kiểm tra:

* MVVM có được tuân thủ không.
* ViewModel có gọi trực tiếp database không.
* Service có đầy đủ validation không.
* Dữ liệu có được cô lập theo UserId không.
* Các thao tác tài chính có transaction không.
* Công thức tính toán có chính xác không.
* Có test hay chưa.
* Có duplicate code không.
* Có secret không.
* UI có xử lý loading và error không.

## 15.3. Thay đổi kiến trúc

Các thay đổi sau phải được cả nhóm đồng ý:

* Thay WPF bằng framework khác.
* Thay SQL Server.
* Thay EF Core.
* Thay MVVM.
* Thêm API backend.
* Thêm cloud service.
* Thay đổi cách lưu hoặc tính số dư ví.
* Thêm vai trò Admin.
* Thay đổi phạm vi phiên bản 1.0.

## 15.4. Hotfix

Hotfix khẩn cấp được phép khi:

* Ứng dụng không đăng nhập được.
* Dữ liệu giao dịch bị sai.
* Số dư ví bị sai.
* Người dùng xem được dữ liệu của người khác.
* Ứng dụng bị crash ở chức năng chính.

Sau hotfix phải:

1. Viết lại nguyên nhân.
2. Bổ sung test tái hiện lỗi.
3. Sửa code.
4. Xác nhận lỗi không tái diễn.
5. Merge thông qua Pull Request.

---

# ĐIỀU 16 – DEFINITION OF DONE

Một chức năng chỉ được xem là hoàn thành khi:

* Spec đã được phê duyệt.
* Code đúng kiến trúc MVVM.
* Build không có lỗi.
* Không có compiler warning.
* Validation đầy đủ.
* Xử lý được main flow.
* Xử lý được exception flow.
* Có Unit Test cho nghiệp vụ.
* Test hiện tại không bị hỏng.
* Dữ liệu được cô lập theo UserId.
* Các thay đổi tài chính sử dụng database transaction.
* Không làm sai số dư ví.
* Không chứa secret.
* Không chứa TODO hoặc code debug.
* UI có loading, empty và error state.
* Đã được thành viên còn lại review.
* Đã merge thông qua Pull Request.
* Tài liệu liên quan đã được cập nhật.

---

# ĐIỀU 17 – DANH SÁCH KIỂM TRA TRƯỚC KHI COMMIT

Trước mỗi commit, thành viên phải kiểm tra:

```text
[ ] Code build thành công
[ ] Không có compiler warning
[ ] Không có secret hoặc connection string
[ ] Không có bin, obj hoặc .vs
[ ] Không còn TODO hoặc debug code
[ ] View không chứa business logic
[ ] ViewModel không gọi trực tiếp database
[ ] Service đã kiểm tra business rule
[ ] Tiền sử dụng decimal
[ ] Giao dịch cập nhật số dư chính xác
[ ] Có xử lý rollback khi lỗi
[ ] Truy vấn có lọc theo UserId
[ ] Unit Test thành công
[ ] Giao diện không bị treo
[ ] Commit message đúng định dạng
```

---

# ĐIỀU 18 – QUẢN LÝ PHẠM VI NHÓM 2 NGƯỜI

## 18.1. Thành viên 1 – Dữ liệu và giao dịch

Phụ trách chính:

* Đăng ký.
* Đăng nhập.
* Database.
* Quản lý ví.
* Quản lý danh mục.
* Quản lý giao dịch.
* Validation.
* Repository và DataAccess.

## 18.2. Thành viên 2 – Phân tích và báo cáo

Phụ trách chính:

* Dashboard.
* Quản lý ngân sách.
* Cảnh báo ngân sách.
* Phân tích dòng tiền.
* Báo cáo.
* Biểu đồ.
* Test case.
* Báo cáo kiểm thử.

## 18.3. Trách nhiệm chung

Cả hai thành viên đều phải:

* Review code của nhau.
* Hiểu các nghiệp vụ tài chính cốt lõi.
* Không chỉ làm phần giao diện hoặc chỉ làm database.
* Hỗ trợ xử lý conflict.
* Cập nhật tài liệu.
* Kiểm tra tích hợp trước khi merge main.

---

# ĐIỀU 19 – SỬA ĐỔI HIẾN PHÁP

Mọi đề xuất sửa đổi phải ghi rõ:

1. Nội dung hiện tại.
2. Nội dung đề xuất.
3. Lý do thay đổi.
4. Ảnh hưởng đến source code.
5. Ảnh hưởng đến database.
6. Ảnh hưởng đến tài liệu và kiểm thử.

Thay đổi chỉ có hiệu lực khi toàn bộ thành viên đồng ý.

Sau khi thay đổi phải:

* Tăng phiên bản tài liệu.
* Ghi ngày sửa đổi.
* Commit bằng nhánh `spec/`.
* Review thông qua Pull Request.

Quy tắc tăng phiên bản:

* `PATCH`: sửa câu chữ, không thay đổi nguyên tắc.
* `MINOR`: thêm nguyên tắc mới nhưng không phá vỡ quy tắc cũ.
* `MAJOR`: thay đổi công nghệ, kiến trúc hoặc business rule cốt lõi.

---

# ĐIỀU 20 – TUYÊN BỐ CUỐI CÙNG

Mục tiêu của dự án không chỉ là tạo ra một ứng dụng có thể chạy, mà phải tạo ra một hệ thống:

* Tính toán tài chính chính xác.
* Bảo vệ dữ liệu của từng người dùng.
* Có cấu trúc rõ ràng.
* Dễ bảo trì.
* Dễ kiểm thử.
* Có thể mở rộng.
* Có tài liệu đầy đủ.
* Có thể được toàn bộ thành viên giải thích và bảo vệ.

Mọi quyết định kỹ thuật phải ưu tiên:

1. Tính chính xác của dữ liệu tài chính.
2. Tính bảo mật.
3. Tính dễ hiểu.
4. Tính dễ kiểm thử.
5. Tính dễ bảo trì.
6. Tốc độ phát triển.

Không hy sinh tính chính xác của dữ liệu để hoàn thành chức năng nhanh hơn.