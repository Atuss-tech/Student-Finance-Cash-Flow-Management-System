TÀI LIỆU NGHIỆP VỤ
Ứng dụng Quản lý Chi tiêu và Phân tích Dòng tiền cho Sinh viên

## 1. Tên đề tài
Ứng dụng Quản lý Chi tiêu và Phân tích Dòng tiền cho Sinh viên
Tên tiếng Anh đề xuất:
Student Finance & Cash Flow Management System

## 2. Tổng quan hệ thống
Ứng dụng Quản lý Chi tiêu và Phân tích Dòng tiền cho Sinh viên là một phần mềm desktop giúp sinh viên theo dõi tình hình tài chính cá nhân hằng ngày. Hệ thống cho phép người dùng ghi nhận các khoản thu nhập, chi tiêu, quản lý ví tiền, phân loại giao dịch, đặt ngân sách theo tháng và xem các báo cáo phân tích dòng tiền.
Thông qua dữ liệu thu chi được nhập vào, hệ thống có thể tính toán tổng thu, tổng chi, số dư hiện tại, dòng tiền ròng, tỷ lệ tiết kiệm, mức độ chi tiêu theo từng danh mục và đưa ra cảnh báo khi người dùng có nguy cơ vượt ngân sách hoặc dòng tiền bị âm.

## 3. Lý do chọn đề tài
Sinh viên thường có nguồn thu nhập không ổn định, chủ yếu đến từ gia đình, học bổng, làm thêm hoặc các khoản hỗ trợ khác. Trong khi đó, các khoản chi tiêu hằng tháng như ăn uống, đi lại, học phí, nhà trọ, giải trí và mua sắm thường phát sinh liên tục.
Nhiều sinh viên không ghi chép chi tiêu nên khó kiểm soát tiền bạc, dễ rơi vào tình trạng hết tiền trước cuối tháng hoặc không biết khoản tiền của mình đã được sử dụng vào đâu. Vì vậy, một ứng dụng desktop hỗ trợ quản lý chi tiêu và phân tích dòng tiền sẽ giúp sinh viên có cái nhìn rõ ràng hơn về tài chính cá nhân.

## 4. Mục tiêu hệ thống
Hệ thống hướng tới các mục tiêu chính sau:
| Mục tiêu | Mô tả |
|---|---|
| Quản lý thu nhập | Ghi nhận các khoản tiền vào như lương làm thêm, tiền gia đình gửi, học bổng |
| Quản lý chi tiêu | Ghi nhận các khoản tiền ra như ăn uống, đi lại, học tập, giải trí |
| Quản lý ví tiền | Theo dõi số dư của từng ví như tiền mặt, tài khoản ngân hàng, ví điện tử |
| Phân loại giao dịch | Sắp xếp giao dịch theo danh mục để dễ thống kê |
| Theo dõi ngân sách | Đặt giới hạn chi tiêu theo tháng hoặc theo danh mục |
| Phân tích dòng tiền | Tính tổng thu, tổng chi, dòng tiền ròng, tỷ lệ tiết kiệm |
| Báo cáo trực quan | Hiển thị biểu đồ, bảng thống kê theo tháng, danh mục, ví tiền |
| Cảnh báo tài chính | Cảnh báo khi chi tiêu vượt ngân sách hoặc dòng tiền âm |


## 5. Đối tượng sử dụng
### 5.1 Người dùng chính
Sinh viên
Sinh viên là người sử dụng chính của hệ thống. Sinh viên có thể tạo tài khoản, quản lý ví, nhập giao dịch thu chi, xem báo cáo và theo dõi tình hình tài chính cá nhân.
### 5.2 Quản trị viên hệ thống
Nếu project cần phân quyền, có thể thêm vai trò Admin. Admin dùng để quản lý tài khoản người dùng, danh mục mặc định hoặc kiểm tra dữ liệu hệ thống.
Tuy nhiên, với project 2 người, nên tập trung vào vai trò chính là Student/User để phạm vi vừa sức.

## 6. Phạm vi hệ thống
### 6.1 Phạm vi nên làm
Hệ thống tập trung vào các chức năng chính:
| Nhóm chức năng | Nội dung |
|---|---|
| Tài khoản | Đăng ký, đăng nhập, đăng xuất |
| Ví tiền | Thêm, sửa, xóa, xem ví |
| Danh mục | Quản lý danh mục thu nhập và chi tiêu |
| Giao dịch | Thêm, sửa, xóa, xem thu nhập và chi tiêu |
| Ngân sách | Đặt ngân sách chi tiêu theo tháng |
| Phân tích dòng tiền | Tính toán tổng thu, tổng chi, dòng tiền ròng |
| Báo cáo | Xem biểu đồ và bảng thống kê |
| Cảnh báo | Cảnh báo vượt ngân sách, chi tiêu bất thường |

### 6.2 Ngoài phạm vi
Hệ thống không xử lý các nghiệp vụ tài chính phức tạp như:
| Ngoài phạm vi | Lý do |
|---|---|
| Kết nối ngân hàng thật | Cần API ngân hàng, bảo mật cao |
| Chuyển tiền thật | Liên quan pháp lý và thanh toán |
| Ví điện tử thật | Cần tích hợp bên thứ ba |
| Đầu tư chứng khoán thật | Nghiệp vụ phức tạp |
| Tư vấn tài chính chuyên sâu | Không phù hợp phạm vi project sinh viên |


## 7. Yêu cầu chức năng
### 7.1 Đăng ký tài khoản
Người dùng có thể tạo tài khoản để sử dụng hệ thống.
**Thông tin cần có:**
| Trường dữ liệu | Mô tả |
|---|---|
| Họ tên | Tên người dùng |
| Email | Dùng để đăng nhập |
| Mật khẩu | Dùng để xác thực |
| Xác nhận mật khẩu | Kiểm tra nhập đúng mật khẩu |

**Quy tắc:**
Email không được để trống.
Email không được trùng với tài khoản đã tồn tại.
Mật khẩu phải có độ dài tối thiểu 6 ký tự.
Mật khẩu xác nhận phải trùng với mật khẩu đã nhập.

### 7.2 Đăng nhập
Người dùng nhập email và mật khẩu để truy cập hệ thống.
**Quy tắc:**
Email và mật khẩu không được để trống.
Nếu thông tin đúng, hệ thống chuyển đến màn hình Dashboard.
Nếu thông tin sai, hệ thống hiển thị thông báo lỗi.
Sau khi đăng nhập, hệ thống chỉ hiển thị dữ liệu của chính người dùng đó.

### 7.3 Quản lý ví tiền
Người dùng có thể tạo nhiều ví để quản lý tiền theo từng nguồn.
**Ví dụ:**
Tiền mặt
Tài khoản ngân hàng
Ví điện tử
Thẻ sinh viên
Tiền tiết kiệm
**Thông tin ví:**
| Trường dữ liệu | Mô tả |
|---|---|
| Tên ví | Ví dụ: Tiền mặt, MB Bank, MoMo |
| Số dư ban đầu | Số tiền hiện có trong ví |
| Loại ví | Cash, Bank, E-wallet |
| Ghi chú | Thông tin thêm |

**Chức năng:**
| Chức năng | Mô tả |
|---|---|
| Thêm ví | Tạo ví mới |
| Sửa ví | Cập nhật tên ví, ghi chú |
| Xóa ví | Xóa ví nếu không còn dùng |
| Xem số dư ví | Hiển thị số tiền còn lại trong từng ví |

**Quy tắc:**
Tên ví không được để trống.
Số dư ban đầu không được nhỏ hơn 0.
Không nên cho xóa ví nếu ví đã có giao dịch, chỉ nên cho đổi trạng thái ngừng sử dụng.

### 7.4 Quản lý danh mục thu chi
Danh mục giúp phân loại giao dịch.
**Ví dụ danh mục thu nhập:**
Tiền gia đình gửi
Lương làm thêm
Học bổng
Thưởng
Khác
**Ví dụ danh mục chi tiêu:**
Ăn uống
Đi lại
Nhà trọ
Học tập
Giải trí
Mua sắm
Y tế
Khác
**Thông tin danh mục:**
| Trường dữ liệu | Mô tả |
|---|---|
| Tên danh mục | Tên loại thu/chi |
| Loại danh mục | Income hoặc Expense |
| Mô tả | Ghi chú thêm |

**Chức năng:**
| Chức năng | Mô tả |
|---|---|
| Thêm danh mục | Người dùng tự tạo danh mục |
| Sửa danh mục | Cập nhật tên hoặc mô tả |
| Xóa danh mục | Xóa danh mục không còn dùng |
| Lọc danh mục | Lọc theo Income hoặc Expense |

**Quy tắc:**
Tên danh mục không được để trống.
Một danh mục chỉ thuộc một loại: thu nhập hoặc chi tiêu.
Không nên xóa danh mục nếu đã có giao dịch sử dụng danh mục đó.

### 7.5 Quản lý giao dịch thu nhập
Người dùng có thể thêm các khoản tiền vào.
**Ví dụ:**
Gia đình gửi 3.000.000đ
Lương part-time 1.500.000đ
Học bổng 2.000.000đ
**Thông tin giao dịch thu nhập:**
| Trường dữ liệu | Mô tả |
|---|---|
| Ví nhận tiền | Ví được cộng tiền |
| Danh mục | Loại thu nhập |
| Số tiền | Số tiền thu vào |
| Ngày giao dịch | Ngày nhận tiền |
| Mô tả | Ghi chú |

**Luồng xử lý:**
Người dùng chọn chức năng thêm thu nhập.
Người dùng chọn ví nhận tiền.
Người dùng chọn danh mục thu nhập.
Người dùng nhập số tiền, ngày giao dịch và mô tả.
Hệ thống kiểm tra dữ liệu hợp lệ.
Hệ thống lưu giao dịch.
Hệ thống cộng số tiền vào ví tương ứng.
Hệ thống cập nhật Dashboard và báo cáo.
**Quy tắc:**
Số tiền phải lớn hơn 0.
Ngày giao dịch không được lớn hơn ngày hiện tại.
Ví nhận tiền phải tồn tại.
Danh mục phải thuộc loại Income.

### 7.6 Quản lý giao dịch chi tiêu
Người dùng có thể thêm các khoản tiền ra.
**Ví dụ:**
Ăn trưa 50.000đ
Đổ xăng 70.000đ
Đóng tiền nhà 1.500.000đ
Mua tài liệu học tập 120.000đ
**Thông tin giao dịch chi tiêu:**
| Trường dữ liệu | Mô tả |
|---|---|
| Ví thanh toán | Ví bị trừ tiền |
| Danh mục | Loại chi tiêu |
| Số tiền | Số tiền chi |
| Ngày giao dịch | Ngày chi tiêu |
| Mô tả | Ghi chú |

**Luồng xử lý:**
Người dùng chọn chức năng thêm chi tiêu.
Người dùng chọn ví thanh toán.
Người dùng chọn danh mục chi tiêu.
Người dùng nhập số tiền, ngày giao dịch và mô tả.
Hệ thống kiểm tra dữ liệu hợp lệ.
Nếu số dư ví đủ, hệ thống lưu giao dịch.
Hệ thống trừ số tiền khỏi ví.
Hệ thống kiểm tra ngân sách của danh mục đó.
Nếu chi tiêu vượt ngân sách, hệ thống hiển thị cảnh báo.
Hệ thống cập nhật Dashboard và báo cáo.
**Quy tắc:**
Số tiền phải lớn hơn 0.
Ngày giao dịch không được lớn hơn ngày hiện tại.
Ví thanh toán phải tồn tại.
Danh mục phải thuộc loại Expense.
Nếu không cho phép âm ví, số tiền chi không được lớn hơn số dư ví.
Sau khi thêm chi tiêu, số dư ví bị trừ tương ứng.

### 7.7 Quản lý ngân sách
Người dùng có thể đặt giới hạn chi tiêu theo tháng cho từng danh mục.
**Ví dụ:**
| Danh mục | Ngân sách tháng |
|---|---|
| Ăn uống | 2.000.000đ |
| Đi lại | 500.000đ |
| Giải trí | 700.000đ |
| Mua sắm | 1.000.000đ |

**Thông tin ngân sách:**
| Trường dữ liệu | Mô tả |
|---|---|
| Tháng | Tháng áp dụng ngân sách |
| Năm | Năm áp dụng ngân sách |
| Danh mục | Danh mục chi tiêu |
| Số tiền ngân sách | Giới hạn được phép chi |
| Ghi chú | Thông tin thêm |

**Luồng xử lý:**
Người dùng chọn chức năng đặt ngân sách.
Người dùng chọn tháng, năm.
Người dùng chọn danh mục chi tiêu.
Người dùng nhập số tiền ngân sách.
Hệ thống kiểm tra dữ liệu.
Hệ thống lưu ngân sách.
Khi có giao dịch chi tiêu mới, hệ thống kiểm tra tổng chi của danh mục đó trong tháng.
Nếu tổng chi vượt ngưỡng, hệ thống hiển thị cảnh báo.
**Quy tắc:**
Ngân sách phải lớn hơn 0.
Một danh mục chỉ nên có một ngân sách trong cùng một tháng.
Chỉ danh mục Expense mới được đặt ngân sách.
Nếu chi tiêu đạt từ 80% ngân sách, hệ thống cảnh báo sắp vượt.
Nếu chi tiêu lớn hơn 100% ngân sách, hệ thống cảnh báo đã vượt ngân sách.

### 7.8 Phân tích dòng tiền cá nhân
Đây là phần nghiệp vụ quan trọng nhất của hệ thống.
Hệ thống sẽ dựa trên các giao dịch thu nhập và chi tiêu để tính toán tình hình tài chính.
**Các chỉ số chính:**
| Chỉ số | Công thức |
|---|---|
| Tổng thu | Tổng tất cả giao dịch Income trong kỳ |
| Tổng chi | Tổng tất cả giao dịch Expense trong kỳ |
| Dòng tiền ròng | Tổng thu - Tổng chi |
| Số dư hiện tại | Tổng số dư của tất cả ví |
| Tỷ lệ tiết kiệm | Dòng tiền ròng / Tổng thu x 100 |
| Chi tiêu trung bình/ngày | Tổng chi trong tháng / số ngày đã qua |
| Dự báo chi cuối tháng | Chi tiêu trung bình/ngày x số ngày trong tháng |
| Dự báo số dư cuối tháng | Tổng thu dự kiến - Tổng chi dự báo |

**Ví dụ:**
Tổng thu tháng 6: 5.000.000đ
Tổng chi tháng 6: 3.800.000đ
Dòng tiền ròng: 1.200.000đ
Tỷ lệ tiết kiệm: 24%
Trạng thái: Tốt

**Phân loại trạng thái tài chính:**
| Điều kiện | Trạng thái |
|---|---|
| Dòng tiền ròng > 0 và tỷ lệ tiết kiệm >= 20% | Tốt |
| Dòng tiền ròng > 0 nhưng tỷ lệ tiết kiệm < 20% | Trung bình |
| Dòng tiền ròng = 0 | Cân bằng |
| Dòng tiền ròng < 0 | Cảnh báo |


### 7.9 Báo cáo và biểu đồ
Hệ thống cung cấp báo cáo giúp người dùng hiểu rõ tình hình tài chính.
**Các loại báo cáo:**
| Báo cáo | Mô tả |
|---|---|
| Báo cáo tổng quan tháng | Tổng thu, tổng chi, dòng tiền ròng |
| Báo cáo chi tiêu theo danh mục | Danh mục nào chi nhiều nhất |
| Báo cáo thu nhập theo nguồn | Nguồn tiền nào chiếm nhiều nhất |
| Báo cáo theo ví | Ví nào còn nhiều tiền nhất |
| Báo cáo so sánh tháng | So sánh tháng này với tháng trước |
| Báo cáo ngân sách | Danh mục nào sắp vượt hoặc đã vượt ngân sách |

**Biểu đồ đề xuất:**
| Biểu đồ | Mục đích |
|---|---|
| Pie chart | Tỷ lệ chi tiêu theo danh mục |
| Bar chart | So sánh thu chi theo tháng |
| Line chart | Xu hướng dòng tiền theo thời gian |
| Progress bar | Mức sử dụng ngân sách |


## 8. Danh sách màn hình
### 8.1 Màn hình đăng nhập
Mục đích: Cho phép người dùng đăng nhập vào hệ thống.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Ô nhập email | Nhập email đăng nhập |
| Ô nhập mật khẩu | Nhập mật khẩu |
| Nút đăng nhập | Xác thực tài khoản |
| Nút đăng ký | Chuyển sang màn hình đăng ký |
| Thông báo lỗi | Hiển thị khi đăng nhập sai |


### 8.2 Màn hình đăng ký
Mục đích: Cho phép người dùng tạo tài khoản mới.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Họ tên | Nhập tên người dùng |
| Email | Nhập email |
| Mật khẩu | Nhập mật khẩu |
| Xác nhận mật khẩu | Nhập lại mật khẩu |
| Nút đăng ký | Tạo tài khoản |
| Nút quay lại | Quay về đăng nhập |


### 8.3 Màn hình Dashboard
Mục đích: Hiển thị tổng quan tài chính cá nhân.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Tổng số dư | Tổng tiền hiện có trong tất cả ví |
| Tổng thu tháng này | Tổng thu nhập trong tháng |
| Tổng chi tháng này | Tổng chi tiêu trong tháng |
| Dòng tiền ròng | Thu - chi |
| Cảnh báo ngân sách | Hiển thị danh mục sắp vượt/vượt ngân sách |
| Biểu đồ thu chi | Thống kê trực quan |
| Giao dịch gần đây | Danh sách giao dịch mới nhất |


### 8.4 Màn hình quản lý ví
Mục đích: Quản lý các ví tiền của người dùng.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Danh sách ví | Hiển thị các ví hiện có |
| Tên ví | Nhập tên ví |
| Loại ví | Chọn Cash, Bank, E-wallet |
| Số dư | Hiển thị số dư |
| Nút thêm | Thêm ví mới |
| Nút sửa | Cập nhật ví |
| Nút xóa | Xóa hoặc ngừng sử dụng ví |


### 8.5 Màn hình quản lý danh mục
Mục đích: Quản lý danh mục thu nhập và chi tiêu.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Danh sách danh mục | Hiển thị category |
| Tên danh mục | Nhập tên |
| Loại danh mục | Income hoặc Expense |
| Mô tả | Ghi chú |
| Nút thêm/sửa/xóa | CRUD danh mục |


### 8.6 Màn hình thêm giao dịch
Mục đích: Thêm khoản thu hoặc khoản chi.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Loại giao dịch | Income hoặc Expense |
| Ví | Chọn ví áp dụng |
| Danh mục | Chọn danh mục |
| Số tiền | Nhập số tiền |
| Ngày giao dịch | Chọn ngày |
| Mô tả | Ghi chú |
| Nút lưu | Lưu giao dịch |
| Nút hủy | Hủy thao tác |


### 8.7 Màn hình lịch sử giao dịch
Mục đích: Xem, tìm kiếm, lọc, sửa, xóa giao dịch.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Bảng giao dịch | Hiển thị danh sách thu chi |
| Bộ lọc ngày | Lọc theo khoảng thời gian |
| Bộ lọc loại | Income hoặc Expense |
| Bộ lọc danh mục | Lọc theo category |
| Ô tìm kiếm | Tìm theo mô tả |
| Nút sửa | Sửa giao dịch |
| Nút xóa | Xóa giao dịch |


### 8.8 Màn hình ngân sách
Mục đích: Đặt và theo dõi ngân sách chi tiêu.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Chọn tháng/năm | Thời gian áp dụng |
| Chọn danh mục | Danh mục chi tiêu |
| Số tiền ngân sách | Giới hạn chi tiêu |
| Số tiền đã chi | Hệ thống tự tính |
| Tỷ lệ sử dụng | Đã dùng bao nhiêu % |
| Trạng thái | Bình thường, sắp vượt, đã vượt |


### 8.9 Màn hình phân tích dòng tiền
Mục đích: Phân tích tài chính cá nhân theo tháng.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Tổng thu | Tổng thu nhập |
| Tổng chi | Tổng chi tiêu |
| Dòng tiền ròng | Thu - chi |
| Tỷ lệ tiết kiệm | Phần trăm tiền còn lại |
| Chi tiêu trung bình/ngày | Tổng chi / số ngày |
| Dự báo cuối tháng | Ước lượng chi tiêu cuối tháng |
| Trạng thái tài chính | Tốt, trung bình, cảnh báo |


### 8.10 Màn hình báo cáo
Mục đích: Hiển thị thống kê bằng bảng và biểu đồ.
**Thành phần chính:**
| Thành phần | Mô tả |
|---|---|
| Biểu đồ chi tiêu theo danh mục | Pie chart |
| Biểu đồ thu chi theo tháng | Bar chart |
| Biểu đồ dòng tiền | Line chart |
| Bảng thống kê | Hiển thị số liệu chi tiết |
| Nút xuất báo cáo | Xuất Excel/PDF nếu có |


## 9. Luồng nghiệp vụ chính
### 9.1 Luồng đăng ký và đăng nhập
Người dùng mở app
- Chọn đăng ký nếu chưa có tài khoản
- Nhập thông tin cá nhân
- Hệ thống kiểm tra email, mật khẩu
- Tạo tài khoản
- Người dùng đăng nhập
- Hệ thống xác thực
- Chuyển vào Dashboard


### 9.2 Luồng thêm thu nhập
Người dùng chọn "Thêm giao dịch"
- Chọn loại giao dịch là Thu nhập
- Chọn ví nhận tiền
- Chọn danh mục thu nhập
- Nhập số tiền
- Chọn ngày giao dịch
- Nhập mô tả
- Bấm lưu
- Hệ thống kiểm tra dữ liệu
- Hệ thống lưu giao dịch
- Hệ thống cộng tiền vào ví
- Cập nhật Dashboard


### 9.3 Luồng thêm chi tiêu
Người dùng chọn "Thêm giao dịch"
- Chọn loại giao dịch là Chi tiêu
- Chọn ví thanh toán
- Chọn danh mục chi tiêu
- Nhập số tiền
- Chọn ngày giao dịch
- Nhập mô tả
- Bấm lưu
- Hệ thống kiểm tra dữ liệu
- Hệ thống kiểm tra số dư ví
- Hệ thống lưu giao dịch
- Hệ thống trừ tiền khỏi ví
- Hệ thống kiểm tra ngân sách
- Nếu vượt ngân sách thì hiển thị cảnh báo
- Cập nhật Dashboard


### 9.4 Luồng phân tích dòng tiền
Người dùng vào màn hình Phân tích dòng tiền
- Chọn tháng/năm cần xem
- Hệ thống lấy tất cả giao dịch trong tháng
- Tính tổng thu
- Tính tổng chi
- Tính dòng tiền ròng
- Tính tỷ lệ tiết kiệm
- Tính chi tiêu trung bình/ngày
- Dự báo chi tiêu cuối tháng
- Xác định trạng thái tài chính
- Hiển thị kết quả và biểu đồ


### 9.5 Luồng cảnh báo ngân sách
Người dùng thêm giao dịch chi tiêu
- Hệ thống kiểm tra danh mục chi tiêu
- Hệ thống lấy ngân sách tháng của danh mục đó
- Hệ thống tính tổng số tiền đã chi trong tháng
- So sánh tổng chi với ngân sách
- Nếu dưới 80%: trạng thái bình thường
- Nếu từ 80% đến dưới 100%: cảnh báo sắp vượt
- Nếu từ 100% trở lên: cảnh báo đã vượt ngân sách


## 10. Quy tắc nghiệp vụ
| Mã rule | Nội dung |
|---|---|
| BR01 | Người dùng phải đăng nhập trước khi sử dụng hệ thống |
| BR02 | Mỗi người dùng chỉ được xem dữ liệu của chính mình |
| BR03 | Số tiền giao dịch phải lớn hơn 0 |
| BR04 | Ngày giao dịch không được lớn hơn ngày hiện tại |
| BR05 | Giao dịch thu nhập làm tăng số dư ví |
| BR06 | Giao dịch chi tiêu làm giảm số dư ví |
| BR07 | Nếu không cho phép âm ví, số tiền chi không được lớn hơn số dư ví |
| BR08 | Danh mục Income chỉ dùng cho giao dịch thu nhập |
| BR09 | Danh mục Expense chỉ dùng cho giao dịch chi tiêu |
| BR10 | Ngân sách chỉ áp dụng cho danh mục chi tiêu |
| BR11 | Một danh mục chỉ có một ngân sách trong cùng một tháng |
| BR12 | Khi chi tiêu đạt 80% ngân sách, hệ thống cảnh báo sắp vượt |
| BR13 | Khi chi tiêu vượt 100% ngân sách, hệ thống cảnh báo đã vượt |
| BR14 | Dòng tiền ròng = Tổng thu - Tổng chi |
| BR15 | Tỷ lệ tiết kiệm = Dòng tiền ròng / Tổng thu x 100 |
| BR16 | Nếu dòng tiền ròng âm, hệ thống đánh giá trạng thái tài chính là cảnh báo |
| BR17 | Khi sửa giao dịch, hệ thống phải cập nhật lại số dư ví |
| BR18 | Khi xóa giao dịch, hệ thống phải hoàn tác ảnh hưởng của giao dịch đó lên ví |


## 11. Yêu cầu phi chức năng
| Nhóm yêu cầu | Nội dung |
|---|---|
| Hiệu năng | Các màn hình danh sách cần tải dữ liệu nhanh, không bị treo app |
| Bảo mật | Mật khẩu không nên lưu dạng plain text |
| Dễ sử dụng | Giao diện đơn giản, dễ hiểu với sinh viên |
| Chính xác | Tính toán thu chi, số dư, ngân sách phải đúng |
| Khả năng bảo trì | Code nên chia theo tầng: UI, Service, Repository/DAO, Model |
| Khả năng mở rộng | Có thể thêm tính năng saving goal, export report, backup data sau này |


## 12. Dữ liệu chính của hệ thống
### 12.1 Users
Lưu thông tin người dùng.
| Field | Mô tả |
|---|---|
| UserId | Mã người dùng |
| FullName | Họ tên |
| Email | Email đăng nhập |
| PasswordHash | Mật khẩu đã mã hóa |
| CreatedAt | Ngày tạo tài khoản |


### 12.2 Wallets
Lưu thông tin ví tiền.
| Field | Mô tả |
|---|---|
| WalletId | Mã ví |
| UserId | Người sở hữu ví |
| WalletName | Tên ví |
| WalletType | Loại ví |
| Balance | Số dư hiện tại |
| Note | Ghi chú |
| IsActive | Trạng thái hoạt động |


### 12.3 Categories
Lưu danh mục thu chi.
| Field | Mô tả |
|---|---|
| CategoryId | Mã danh mục |
| UserId | Người sở hữu danh mục |
| CategoryName | Tên danh mục |
| CategoryType | Income hoặc Expense |
| Description | Mô tả |


### 12.4 Transactions
Lưu các giao dịch thu nhập và chi tiêu.
| Field | Mô tả |
|---|---|
| TransactionId | Mã giao dịch |
| UserId | Người tạo giao dịch |
| WalletId | Ví áp dụng |
| CategoryId | Danh mục |
| TransactionType | Income hoặc Expense |
| Amount | Số tiền |
| TransactionDate | Ngày giao dịch |
| Description | Mô tả |
| CreatedAt | Ngày tạo |


### 12.5 Budgets
Lưu ngân sách chi tiêu.
| Field | Mô tả |
|---|---|
| BudgetId | Mã ngân sách |
| UserId | Người tạo |
| CategoryId | Danh mục chi tiêu |
| Month | Tháng |
| Year | Năm |
| AmountLimit | Số tiền giới hạn |
| Note | Ghi chú |


## 13. Các chức năng ưu tiên khi làm project 2 người
Giai đoạn 1: Bắt buộc phải có
| Chức năng | Mức độ |
|---|---|
| Đăng nhập/Đăng ký | Bắt buộc |
| Quản lý ví | Bắt buộc |
| Quản lý danh mục | Bắt buộc |
| Thêm/sửa/xóa giao dịch | Bắt buộc |
| Xem lịch sử giao dịch | Bắt buộc |
| Dashboard tổng quan | Bắt buộc |

Giai đoạn 2: Nên có
| Chức năng | Mức độ |
|---|---|
| Quản lý ngân sách | Nên có |
| Cảnh báo vượt ngân sách | Nên có |
| Phân tích dòng tiền | Nên có |
| Báo cáo theo tháng | Nên có |
| Biểu đồ chi tiêu | Nên có |

Giai đoạn 3: Có thời gian thì làm thêm
| Chức năng | Mức độ |
|---|---|
| Xuất Excel/PDF | Nâng cao |
| Mục tiêu tiết kiệm | Nâng cao |
| So sánh nhiều tháng | Nâng cao |
| Dự báo cuối tháng | Nâng cao |
| Backup/Restore dữ liệu | Nâng cao |


## 14. Phân chia công việc cho 2 thành viên
Thành viên 1: Quản lý dữ liệu chính
Phụ trách:
Đăng nhập, đăng ký
Database
Quản lý ví
Quản lý danh mục
Quản lý giao dịch thu/chi
CRUD dữ liệu
Validation form nhập liệu
Thành viên 2: Phân tích và báo cáo
Phụ trách:
Dashboard
Quản lý ngân sách
Cảnh báo vượt ngân sách
Phân tích dòng tiền
Biểu đồ báo cáo
Test case và báo cáo kiểm thử

## 15. Kết luận nghiệp vụ
Ứng dụng Quản lý Chi tiêu và Phân tích Dòng tiền cho Sinh viên là một hệ thống desktop phù hợp với nhóm 2 người vì phạm vi vừa phải, nghiệp vụ rõ ràng và có nhiều chức năng để thể hiện kỹ năng lập trình.
Hệ thống không chỉ dừng lại ở việc nhập thu chi mà còn phân tích dữ liệu tài chính để giúp sinh viên hiểu rõ tiền của mình đến từ đâu, được chi vào đâu, còn lại bao nhiêu và có đang chi tiêu vượt kiểm soát hay không.
Điểm mạnh của đề tài là có đầy đủ CRUD, validation, database, báo cáo, biểu đồ, business rule và luồng nghiệp vụ rõ ràng. Đây là một đề tài phù hợp để phát triển bằng các công nghệ desktop như C# WinForms, WPF hoặc JavaFX.
