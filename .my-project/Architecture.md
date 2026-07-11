# Cấu trúc Solution: ProductManagementDemo

- 📂 **BusinessObjects** (Tầng chứa các đối tượng dữ liệu - Class Library)
  - 📄 `Product.cs`
  - 📄 `Category.cs`

- 📂 **DataAccessLayer / DataAccessObjects** (Tầng truy cập dữ liệu trực tiếp - Class Library)
  - 📄 `AccountDAO.cs`
  - 📄 `CategoryDAO.cs`
  - 📄 `ProductDAO.cs`

- 📂 **Repositories** (Tầng trừu tượng hóa dữ liệu - Class Library)
  - 📄 `IAccountRepository.cs` (Interface)
  - 📄 `ICatergoryRepository.cs` (Interface - nguyên văn theo tài liệu)
  - 📄 `IProductRepository.cs` (Interface)
  - 📄 `AccountRepository.cs` (Class triển khai)
  - 📄 `CategoryRepository.cs` (Class triển khai)
  - 📄 `ProductRepository.cs` (Class triển khai)

- 📂 **Services** (Tầng xử lý logic nghiệp vụ - Class Library)
  - 📄 `IAccountService.cs` (Interface)
  - 📄 `ICatergoryService.cs` (Interface)
  - 📄 `IProductService.cs` (Interface)
  - 📄 `AccountService.cs` (Class triển khai)
  - 📄 `CategoryService.cs` (Class triển khai)
  - 📄 `ProductService.cs` (Class triển khai)

- 📂 **ProductManagement / WPFApp** (Tầng Giao diện người dùng - WPF Project)
  - 📄 `App.xaml` (File cấu hình luồng khởi chạy dự án)
  - 📄 `LoginWindow.xaml` (Giao diện màn hình đăng nhập)
    - 📄 `LoginWindow.xaml.cs` (Code xử lý backend cho đăng nhập)
  - 📄 `MainWindow.xaml` (Giao diện màn hình quản lý chính)
    - 📄 `MainWindow.xaml.cs` (Code xử lý backend cho các nút Thêm/Sửa/Xóa/Đóng)