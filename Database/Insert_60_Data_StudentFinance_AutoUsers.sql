USE StudentFinanceDb;
GO

SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    /* ============================================================
       1. BẢO ĐẢM CÓ 5 USER ĐỂ LÀM KHÓA NGOẠI

       Wallets, Categories, Budgets và FinanceTransactions đều có
       UserId là khóa ngoại, vì vậy không thể thêm dữ liệu vào các
       bảng này khi Users đang rỗng.

       Các user mẫu chỉ được thêm khi email tương ứng chưa tồn tại.
       ============================================================ */

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'huy@gmail.com')
    BEGIN
        INSERT INTO Users (FullName, Email, PasswordHash)
        VALUES (N'Nguyễn Văn Huy', 'huy@gmail.com', N'HASH_HUY_123456');
    END;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'anh@gmail.com')
    BEGIN
        INSERT INTO Users (FullName, Email, PasswordHash)
        VALUES (N'Trần Minh Anh', 'anh@gmail.com', N'HASH_ANH_123456');
    END;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'trang@gmail.com')
    BEGIN
        INSERT INTO Users (FullName, Email, PasswordHash)
        VALUES (N'Lê Thu Trang', 'trang@gmail.com', N'HASH_TRANG_123');
    END;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'duy@gmail.com')
    BEGIN
        INSERT INTO Users (FullName, Email, PasswordHash)
        VALUES (N'Phạm Quang Duy', 'duy@gmail.com', N'HASH_DUY_123456');
    END;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'chi@gmail.com')
    BEGIN
        INSERT INTO Users (FullName, Email, PasswordHash)
        VALUES (N'Hoàng Lan Chi', 'chi@gmail.com', N'HASH_CHI_123456');
    END;


    /* ============================================================
       2. LẤY USER ID THEO EMAIL
       Không phụ thuộc UserId cụ thể là 1, 2, 3, 4 hay 5
       ============================================================ */

    DECLARE @HuyUserId INT;
    DECLARE @AnhUserId INT;
    DECLARE @TrangUserId INT;
    DECLARE @DuyUserId INT;
    DECLARE @ChiUserId INT;

    SELECT @HuyUserId = UserId
    FROM Users
    WHERE Email = 'huy@gmail.com';

    SELECT @AnhUserId = UserId
    FROM Users
    WHERE Email = 'anh@gmail.com';

    SELECT @TrangUserId = UserId
    FROM Users
    WHERE Email = 'trang@gmail.com';

    SELECT @DuyUserId = UserId
    FROM Users
    WHERE Email = 'duy@gmail.com';

    SELECT @ChiUserId = UserId
    FROM Users
    WHERE Email = 'chi@gmail.com';

    IF @HuyUserId IS NULL
       OR @AnhUserId IS NULL
       OR @TrangUserId IS NULL
       OR @DuyUserId IS NULL
       OR @ChiUserId IS NULL
    BEGIN
        RAISERROR
        (
            N'Không thể tạo hoặc tìm đủ 5 user mẫu.',
            16,
            1
        );

        ROLLBACK TRANSACTION;
        RETURN;
    END;


    /* ============================================================
       3. XÓA BỘ DỮ LIỆU BATCH60 CŨ NẾU ĐÃ TỪNG CHẠY
       Xóa theo đúng thứ tự khóa ngoại để có thể chạy lại an toàn
       ============================================================ */

    DELETE FROM FinanceTransactions
    WHERE Description LIKE N'Batch60:%';

    DELETE FROM Budgets
    WHERE Note LIKE N'Batch60:%';

    DELETE FROM Categories
    WHERE Description LIKE N'Batch60:%';

    DELETE FROM Wallets
    WHERE Note LIKE N'Batch60:%';


    /* ============================================================
       4. THÊM 10 WALLETS
       Mỗi user có thêm 2 ví
       ============================================================ */

    INSERT INTO Wallets
    (
        UserId,
        WalletName,
        WalletType,
        InitialBalance,
        Balance,
        Note
    )
    VALUES
    /* Nguyễn Văn Huy */
    (
        @HuyUserId,
        N'ACB 2026',
        'Bank',
        2000000,
        2000000,
        N'Batch60: Tài khoản ngân hàng ACB'
    ),
    (
        @HuyUserId,
        N'ShopeePay 2026',
        'EWallet',
        300000,
        300000,
        N'Batch60: Ví điện tử ShopeePay'
    ),

    /* Trần Minh Anh */
    (
        @AnhUserId,
        N'VPBank 2026',
        'Bank',
        1800000,
        1800000,
        N'Batch60: Tài khoản ngân hàng VPBank'
    ),
    (
        @AnhUserId,
        N'MoMo phụ 2026',
        'EWallet',
        400000,
        400000,
        N'Batch60: Ví MoMo dùng cho thanh toán'
    ),

    /* Lê Thu Trang */
    (
        @TrangUserId,
        N'BIDV 2026',
        'Bank',
        1500000,
        1500000,
        N'Batch60: Tài khoản ngân hàng BIDV'
    ),
    (
        @TrangUserId,
        N'Tiền dự phòng 2026',
        'Cash',
        1000000,
        1000000,
        N'Batch60: Tiền mặt dùng khi khẩn cấp'
    ),

    /* Phạm Quang Duy */
    (
        @DuyUserId,
        N'TPBank 2026',
        'Bank',
        2200000,
        2200000,
        N'Batch60: Tài khoản ngân hàng TPBank'
    ),
    (
        @DuyUserId,
        N'VNPay 2026',
        'EWallet',
        600000,
        600000,
        N'Batch60: Ví điện tử VNPay'
    ),

    /* Hoàng Lan Chi */
    (
        @ChiUserId,
        N'Sacombank 2026',
        'Bank',
        1700000,
        1700000,
        N'Batch60: Tài khoản ngân hàng Sacombank'
    ),
    (
        @ChiUserId,
        N'Tiền tiết kiệm 2026',
        'Cash',
        1200000,
        1200000,
        N'Batch60: Tiền mặt tiết kiệm'
    );


    /* ============================================================
       5. THÊM 20 CATEGORIES
       Mỗi user có:
       - 2 danh mục Income
       - 2 danh mục Expense
       ============================================================ */

    INSERT INTO Categories
    (
        UserId,
        CategoryName,
        CategoryType,
        Description
    )
    VALUES
    /* Nguyễn Văn Huy */
    (
        @HuyUserId,
        N'Thưởng dự án 2026',
        'Income',
        N'Batch60: Tiền thưởng từ dự án'
    ),
    (
        @HuyUserId,
        N'Bán đồ cũ 2026',
        'Income',
        N'Batch60: Tiền bán các đồ dùng không còn sử dụng'
    ),
    (
        @HuyUserId,
        N'Khóa học online 2026',
        'Expense',
        N'Batch60: Chi phí mua khóa học trực tuyến'
    ),
    (
        @HuyUserId,
        N'Đồ dùng cá nhân 2026',
        'Expense',
        N'Batch60: Chi phí mua đồ dùng cá nhân'
    ),

    /* Trần Minh Anh */
    (
        @AnhUserId,
        N'Tiền trợ cấp 2026',
        'Income',
        N'Batch60: Khoản tiền trợ cấp'
    ),
    (
        @AnhUserId,
        N'Tiền thưởng 2026',
        'Income',
        N'Batch60: Tiền thưởng công việc'
    ),
    (
        @AnhUserId,
        N'Điện nước 2026',
        'Expense',
        N'Batch60: Tiền điện và tiền nước'
    ),
    (
        @AnhUserId,
        N'Internet 2026',
        'Expense',
        N'Batch60: Chi phí Internet hàng tháng'
    ),

    /* Lê Thu Trang */
    (
        @TrangUserId,
        N'Freelance 2026',
        'Income',
        N'Batch60: Thu nhập từ công việc tự do'
    ),
    (
        @TrangUserId,
        N'Quà tặng 2026',
        'Income',
        N'Batch60: Tiền được tặng'
    ),
    (
        @TrangUserId,
        N'Mỹ phẩm 2026',
        'Expense',
        N'Batch60: Chi phí mua mỹ phẩm'
    ),
    (
        @TrangUserId,
        N'Du lịch 2026',
        'Expense',
        N'Batch60: Chi phí đi du lịch'
    ),

    /* Phạm Quang Duy */
    (
        @DuyUserId,
        N'Lương thực tập 2026',
        'Income',
        N'Batch60: Thu nhập từ kỳ thực tập'
    ),
    (
        @DuyUserId,
        N'Tiền hoàn lại 2026',
        'Income',
        N'Batch60: Các khoản tiền được hoàn lại'
    ),
    (
        @DuyUserId,
        N'Thể thao 2026',
        'Expense',
        N'Batch60: Chi phí tập luyện thể thao'
    ),
    (
        @DuyUserId,
        N'Sửa xe 2026',
        'Expense',
        N'Batch60: Chi phí bảo dưỡng và sửa xe'
    ),

    /* Hoàng Lan Chi */
    (
        @ChiUserId,
        N'Bán hàng online 2026',
        'Income',
        N'Batch60: Thu nhập từ bán hàng trực tuyến'
    ),
    (
        @ChiUserId,
        N'Thưởng học tập 2026',
        'Income',
        N'Batch60: Tiền thưởng kết quả học tập'
    ),
    (
        @ChiUserId,
        N'Điện thoại 2026',
        'Expense',
        N'Batch60: Chi phí điện thoại và dữ liệu di động'
    ),
    (
        @ChiUserId,
        N'Quà tặng bạn bè 2026',
        'Expense',
        N'Batch60: Chi phí mua quà tặng bạn bè'
    );


    /* ============================================================
       6. LẤY WALLET ID
       ============================================================ */

    DECLARE @HuyACBId INT;
    DECLARE @HuyShopeePayId INT;

    DECLARE @AnhVPBankId INT;
    DECLARE @AnhMomoId INT;

    DECLARE @TrangBIDVId INT;
    DECLARE @TrangCashId INT;

    DECLARE @DuyTPBankId INT;
    DECLARE @DuyVNPayId INT;

    DECLARE @ChiSacombankId INT;
    DECLARE @ChiSavingId INT;


    SELECT @HuyACBId = WalletId
    FROM Wallets
    WHERE UserId = @HuyUserId
      AND WalletName = N'ACB 2026';

    SELECT @HuyShopeePayId = WalletId
    FROM Wallets
    WHERE UserId = @HuyUserId
      AND WalletName = N'ShopeePay 2026';


    SELECT @AnhVPBankId = WalletId
    FROM Wallets
    WHERE UserId = @AnhUserId
      AND WalletName = N'VPBank 2026';

    SELECT @AnhMomoId = WalletId
    FROM Wallets
    WHERE UserId = @AnhUserId
      AND WalletName = N'MoMo phụ 2026';


    SELECT @TrangBIDVId = WalletId
    FROM Wallets
    WHERE UserId = @TrangUserId
      AND WalletName = N'BIDV 2026';

    SELECT @TrangCashId = WalletId
    FROM Wallets
    WHERE UserId = @TrangUserId
      AND WalletName = N'Tiền dự phòng 2026';


    SELECT @DuyTPBankId = WalletId
    FROM Wallets
    WHERE UserId = @DuyUserId
      AND WalletName = N'TPBank 2026';

    SELECT @DuyVNPayId = WalletId
    FROM Wallets
    WHERE UserId = @DuyUserId
      AND WalletName = N'VNPay 2026';


    SELECT @ChiSacombankId = WalletId
    FROM Wallets
    WHERE UserId = @ChiUserId
      AND WalletName = N'Sacombank 2026';

    SELECT @ChiSavingId = WalletId
    FROM Wallets
    WHERE UserId = @ChiUserId
      AND WalletName = N'Tiền tiết kiệm 2026';


    /* ============================================================
       7. LẤY CATEGORY ID
       ============================================================ */

    DECLARE @HuyProjectIncomeId INT;
    DECLARE @HuyOldItemIncomeId INT;
    DECLARE @HuyCourseExpenseId INT;
    DECLARE @HuyPersonalExpenseId INT;

    DECLARE @AnhSupportIncomeId INT;
    DECLARE @AnhBonusIncomeId INT;
    DECLARE @AnhUtilityExpenseId INT;
    DECLARE @AnhInternetExpenseId INT;

    DECLARE @TrangFreelanceIncomeId INT;
    DECLARE @TrangGiftIncomeId INT;
    DECLARE @TrangCosmeticExpenseId INT;
    DECLARE @TrangTravelExpenseId INT;

    DECLARE @DuyInternIncomeId INT;
    DECLARE @DuyRefundIncomeId INT;
    DECLARE @DuySportExpenseId INT;
    DECLARE @DuyRepairExpenseId INT;

    DECLARE @ChiOnlineIncomeId INT;
    DECLARE @ChiStudyIncomeId INT;
    DECLARE @ChiPhoneExpenseId INT;
    DECLARE @ChiGiftExpenseId INT;


    /* Huy */

    SELECT @HuyProjectIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @HuyUserId
      AND CategoryName = N'Thưởng dự án 2026'
      AND CategoryType = 'Income';

    SELECT @HuyOldItemIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @HuyUserId
      AND CategoryName = N'Bán đồ cũ 2026'
      AND CategoryType = 'Income';

    SELECT @HuyCourseExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @HuyUserId
      AND CategoryName = N'Khóa học online 2026'
      AND CategoryType = 'Expense';

    SELECT @HuyPersonalExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @HuyUserId
      AND CategoryName = N'Đồ dùng cá nhân 2026'
      AND CategoryType = 'Expense';


    /* Minh Anh */

    SELECT @AnhSupportIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @AnhUserId
      AND CategoryName = N'Tiền trợ cấp 2026'
      AND CategoryType = 'Income';

    SELECT @AnhBonusIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @AnhUserId
      AND CategoryName = N'Tiền thưởng 2026'
      AND CategoryType = 'Income';

    SELECT @AnhUtilityExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @AnhUserId
      AND CategoryName = N'Điện nước 2026'
      AND CategoryType = 'Expense';

    SELECT @AnhInternetExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @AnhUserId
      AND CategoryName = N'Internet 2026'
      AND CategoryType = 'Expense';


    /* Thu Trang */

    SELECT @TrangFreelanceIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @TrangUserId
      AND CategoryName = N'Freelance 2026'
      AND CategoryType = 'Income';

    SELECT @TrangGiftIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @TrangUserId
      AND CategoryName = N'Quà tặng 2026'
      AND CategoryType = 'Income';

    SELECT @TrangCosmeticExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @TrangUserId
      AND CategoryName = N'Mỹ phẩm 2026'
      AND CategoryType = 'Expense';

    SELECT @TrangTravelExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @TrangUserId
      AND CategoryName = N'Du lịch 2026'
      AND CategoryType = 'Expense';


    /* Quang Duy */

    SELECT @DuyInternIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @DuyUserId
      AND CategoryName = N'Lương thực tập 2026'
      AND CategoryType = 'Income';

    SELECT @DuyRefundIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @DuyUserId
      AND CategoryName = N'Tiền hoàn lại 2026'
      AND CategoryType = 'Income';

    SELECT @DuySportExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @DuyUserId
      AND CategoryName = N'Thể thao 2026'
      AND CategoryType = 'Expense';

    SELECT @DuyRepairExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @DuyUserId
      AND CategoryName = N'Sửa xe 2026'
      AND CategoryType = 'Expense';


    /* Lan Chi */

    SELECT @ChiOnlineIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @ChiUserId
      AND CategoryName = N'Bán hàng online 2026'
      AND CategoryType = 'Income';

    SELECT @ChiStudyIncomeId = CategoryId
    FROM Categories
    WHERE UserId = @ChiUserId
      AND CategoryName = N'Thưởng học tập 2026'
      AND CategoryType = 'Income';

    SELECT @ChiPhoneExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @ChiUserId
      AND CategoryName = N'Điện thoại 2026'
      AND CategoryType = 'Expense';

    SELECT @ChiGiftExpenseId = CategoryId
    FROM Categories
    WHERE UserId = @ChiUserId
      AND CategoryName = N'Quà tặng bạn bè 2026'
      AND CategoryType = 'Expense';


    /* ============================================================
       8. THÊM 10 BUDGETS
       Mỗi user có 2 ngân sách Expense
       ============================================================ */

    DECLARE @CurrentMonth INT = MONTH(GETDATE());
    DECLARE @CurrentYear INT = YEAR(GETDATE());

    INSERT INTO Budgets
    (
        UserId,
        CategoryId,
        [Month],
        [Year],
        AmountLimit,
        Note
    )
    VALUES
    /* Huy */
    (
        @HuyUserId,
        @HuyCourseExpenseId,
        @CurrentMonth,
        @CurrentYear,
        500000,
        N'Batch60: Ngân sách khóa học online'
    ),
    (
        @HuyUserId,
        @HuyPersonalExpenseId,
        @CurrentMonth,
        @CurrentYear,
        200000,
        N'Batch60: Ngân sách đồ dùng cá nhân'
    ),

    /* Minh Anh */
    (
        @AnhUserId,
        @AnhUtilityExpenseId,
        @CurrentMonth,
        @CurrentYear,
        500000,
        N'Batch60: Ngân sách điện nước'
    ),
    (
        @AnhUserId,
        @AnhInternetExpenseId,
        @CurrentMonth,
        @CurrentYear,
        200000,
        N'Batch60: Ngân sách Internet'
    ),

    /* Thu Trang */
    (
        @TrangUserId,
        @TrangCosmeticExpenseId,
        @CurrentMonth,
        @CurrentYear,
        500000,
        N'Batch60: Ngân sách mỹ phẩm'
    ),
    (
        @TrangUserId,
        @TrangTravelExpenseId,
        @CurrentMonth,
        @CurrentYear,
        500000,
        N'Batch60: Ngân sách du lịch'
    ),

    /* Quang Duy */
    (
        @DuyUserId,
        @DuySportExpenseId,
        @CurrentMonth,
        @CurrentYear,
        800000,
        N'Batch60: Ngân sách thể thao'
    ),
    (
        @DuyUserId,
        @DuyRepairExpenseId,
        @CurrentMonth,
        @CurrentYear,
        250000,
        N'Batch60: Ngân sách sửa xe'
    ),

    /* Lan Chi */
    (
        @ChiUserId,
        @ChiPhoneExpenseId,
        @CurrentMonth,
        @CurrentYear,
        300000,
        N'Batch60: Ngân sách điện thoại'
    ),
    (
        @ChiUserId,
        @ChiGiftExpenseId,
        @CurrentMonth,
        @CurrentYear,
        600000,
        N'Batch60: Ngân sách quà tặng'
    );


    /* ============================================================
       9. THÊM 20 FINANCE TRANSACTIONS
       Mỗi user có:
       - 2 Income
       - 2 Expense
       ============================================================ */

    DECLARE @Today DATE = CAST(GETDATE() AS DATE);
    DECLARE @Yesterday DATE = DATEADD(DAY, -1, CAST(GETDATE() AS DATE));

    INSERT INTO FinanceTransactions
    (
        UserId,
        WalletId,
        CategoryId,
        TransactionType,
        Amount,
        TransactionDate,
        Description
    )
    VALUES
    /* ============================================================
       HUY: 4 GIAO DỊCH
       ============================================================ */
    (
        @HuyUserId,
        @HuyACBId,
        @HuyProjectIncomeId,
        'Income',
        2500000,
        @Yesterday,
        N'Batch60: Nhận tiền thưởng dự án'
    ),
    (
        @HuyUserId,
        @HuyShopeePayId,
        @HuyOldItemIncomeId,
        'Income',
        600000,
        @Today,
        N'Batch60: Bán bàn phím cũ'
    ),
    (
        @HuyUserId,
        @HuyACBId,
        @HuyCourseExpenseId,
        'Expense',
        400000,
        @Today,
        N'Batch60: Mua khóa học lập trình WPF'
    ),
    (
        @HuyUserId,
        @HuyShopeePayId,
        @HuyPersonalExpenseId,
        'Expense',
        250000,
        @Today,
        N'Batch60: Mua đồ dùng cá nhân'
    ),

    /* ============================================================
       MINH ANH: 4 GIAO DỊCH
       ============================================================ */
    (
        @AnhUserId,
        @AnhVPBankId,
        @AnhSupportIncomeId,
        'Income',
        1200000,
        @Yesterday,
        N'Batch60: Nhận tiền trợ cấp'
    ),
    (
        @AnhUserId,
        @AnhMomoId,
        @AnhBonusIncomeId,
        'Income',
        500000,
        @Today,
        N'Batch60: Nhận tiền thưởng công việc'
    ),
    (
        @AnhUserId,
        @AnhVPBankId,
        @AnhUtilityExpenseId,
        'Expense',
        350000,
        @Today,
        N'Batch60: Thanh toán tiền điện nước'
    ),
    (
        @AnhUserId,
        @AnhMomoId,
        @AnhInternetExpenseId,
        'Expense',
        180000,
        @Today,
        N'Batch60: Thanh toán tiền Internet'
    ),

    /* ============================================================
       THU TRANG: 4 GIAO DỊCH
       ============================================================ */
    (
        @TrangUserId,
        @TrangBIDVId,
        @TrangFreelanceIncomeId,
        'Income',
        1800000,
        @Yesterday,
        N'Batch60: Nhận tiền làm freelance'
    ),
    (
        @TrangUserId,
        @TrangCashId,
        @TrangGiftIncomeId,
        'Income',
        700000,
        @Today,
        N'Batch60: Nhận tiền quà tặng'
    ),
    (
        @TrangUserId,
        @TrangBIDVId,
        @TrangCosmeticExpenseId,
        'Expense',
        450000,
        @Today,
        N'Batch60: Mua mỹ phẩm'
    ),
    (
        @TrangUserId,
        @TrangCashId,
        @TrangTravelExpenseId,
        'Expense',
        600000,
        @Today,
        N'Batch60: Chi phí chuyến du lịch'
    ),

    /* ============================================================
       QUANG DUY: 4 GIAO DỊCH
       ============================================================ */
    (
        @DuyUserId,
        @DuyTPBankId,
        @DuyInternIncomeId,
        'Income',
        3000000,
        @Yesterday,
        N'Batch60: Nhận lương thực tập'
    ),
    (
        @DuyUserId,
        @DuyVNPayId,
        @DuyRefundIncomeId,
        'Income',
        300000,
        @Today,
        N'Batch60: Nhận khoản tiền hoàn lại'
    ),
    (
        @DuyUserId,
        @DuyTPBankId,
        @DuySportExpenseId,
        'Expense',
        500000,
        @Today,
        N'Batch60: Đăng ký phòng tập thể thao'
    ),
    (
        @DuyUserId,
        @DuyVNPayId,
        @DuyRepairExpenseId,
        'Expense',
        280000,
        @Today,
        N'Batch60: Sửa xe máy'
    ),

    /* ============================================================
       LAN CHI: 4 GIAO DỊCH
       ============================================================ */
    (
        @ChiUserId,
        @ChiSacombankId,
        @ChiOnlineIncomeId,
        'Income',
        2000000,
        @Yesterday,
        N'Batch60: Thu nhập bán hàng online'
    ),
    (
        @ChiUserId,
        @ChiSavingId,
        @ChiStudyIncomeId,
        'Income',
        800000,
        @Today,
        N'Batch60: Nhận thưởng học tập'
    ),
    (
        @ChiUserId,
        @ChiSacombankId,
        @ChiPhoneExpenseId,
        'Expense',
        220000,
        @Today,
        N'Batch60: Thanh toán cước điện thoại'
    ),
    (
        @ChiUserId,
        @ChiSavingId,
        @ChiGiftExpenseId,
        'Expense',
        500000,
        @Today,
        N'Batch60: Mua quà sinh nhật cho bạn'
    );


    /* ============================================================
       10. CẬP NHẬT SỐ DƯ CHO 10 VÍ MỚI

       Balance =
           InitialBalance
           + Tổng Income
           - Tổng Expense
       ============================================================ */

    UPDATE W
    SET W.Balance =
        W.InitialBalance
        + ISNULL(T.TotalChange, 0)
    FROM Wallets AS W
    LEFT JOIN
    (
        SELECT
            WalletId,
            SUM
            (
                CASE
                    WHEN TransactionType = 'Income'
                        THEN Amount

                    WHEN TransactionType = 'Expense'
                        THEN -Amount

                    ELSE 0
                END
            ) AS TotalChange
        FROM FinanceTransactions
        WHERE Description LIKE N'Batch60:%'
        GROUP BY WalletId
    ) AS T
        ON W.WalletId = T.WalletId
    WHERE W.Note LIKE N'Batch60:%';


    /* ============================================================
       11. KIỂM TRA ĐÚNG 60 BẢN GHI TRƯỚC KHI COMMIT
       ============================================================ */

    DECLARE @WalletCount INT;
    DECLARE @CategoryCount INT;
    DECLARE @BudgetCount INT;
    DECLARE @TransactionCount INT;
    DECLARE @TotalInserted INT;

    SELECT @WalletCount = COUNT(*)
    FROM Wallets
    WHERE Note LIKE N'Batch60:%';

    SELECT @CategoryCount = COUNT(*)
    FROM Categories
    WHERE Description LIKE N'Batch60:%';

    SELECT @BudgetCount = COUNT(*)
    FROM Budgets
    WHERE Note LIKE N'Batch60:%';

    SELECT @TransactionCount = COUNT(*)
    FROM FinanceTransactions
    WHERE Description LIKE N'Batch60:%';

    SET @TotalInserted =
        @WalletCount
        + @CategoryCount
        + @BudgetCount
        + @TransactionCount;

    IF @TotalInserted <> 60
    BEGIN
        RAISERROR
        (
            N'Số bản ghi Batch60 không đúng 60. Giao dịch đã bị hủy.',
            16,
            1
        );
    END;

    COMMIT TRANSACTION;

    PRINT N'Đã chèn thành công 60 bản ghi vào Wallets, Categories, Budgets và FinanceTransactions.';
END TRY

BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    RAISERROR
    (
        @ErrorMessage,
        @ErrorSeverity,
        @ErrorState
    );
END CATCH;
GO


/* ============================================================
   11. KIỂM TRA SỐ LƯỢNG DỮ LIỆU VỪA THÊM
   ============================================================ */

SELECT
    N'Wallets' AS TableName,
    COUNT(*) AS AddedRecords
FROM Wallets
WHERE Note LIKE N'Batch60:%'

UNION ALL

SELECT
    N'Categories',
    COUNT(*)
FROM Categories
WHERE Description LIKE N'Batch60:%'

UNION ALL

SELECT
    N'Budgets',
    COUNT(*)
FROM Budgets
WHERE Note LIKE N'Batch60:%'

UNION ALL

SELECT
    N'FinanceTransactions',
    COUNT(*)
FROM FinanceTransactions
WHERE Description LIKE N'Batch60:%';
GO


/* ============================================================
   12. KIỂM TRA TỔNG PHẢI BẰNG 60
   ============================================================ */

SELECT
    (
        (SELECT COUNT(*)
         FROM Wallets
         WHERE Note LIKE N'Batch60:%')

        +

        (SELECT COUNT(*)
         FROM Categories
         WHERE Description LIKE N'Batch60:%')

        +

        (SELECT COUNT(*)
         FROM Budgets
         WHERE Note LIKE N'Batch60:%')

        +

        (SELECT COUNT(*)
         FROM FinanceTransactions
         WHERE Description LIKE N'Batch60:%')
    ) AS TotalAddedRecords;
GO


/* ============================================================
   13. XEM 10 VÍ VỪA THÊM
   ============================================================ */

SELECT
    W.WalletId,
    U.FullName,
    W.WalletName,
    W.WalletType,
    W.InitialBalance,
    W.Balance,
    W.Note
FROM Wallets AS W

INNER JOIN Users AS U
    ON W.UserId = U.UserId

WHERE W.Note LIKE N'Batch60:%'

ORDER BY
    U.UserId,
    W.WalletId;
GO


/* ============================================================
   14. XEM 20 DANH MỤC VỪA THÊM
   ============================================================ */

SELECT
    C.CategoryId,
    U.FullName,
    C.CategoryName,
    C.CategoryType,
    C.Description
FROM Categories AS C

INNER JOIN Users AS U
    ON C.UserId = U.UserId

WHERE C.Description LIKE N'Batch60:%'

ORDER BY
    U.UserId,
    C.CategoryType,
    C.CategoryId;
GO


/* ============================================================
   15. XEM 20 GIAO DỊCH VỪA THÊM
   ============================================================ */

SELECT
    FT.TransactionId,
    U.FullName,
    W.WalletName,
    C.CategoryName,
    FT.TransactionType,
    FT.Amount,
    FT.TransactionDate,
    FT.Description
FROM FinanceTransactions AS FT

INNER JOIN Users AS U
    ON FT.UserId = U.UserId

INNER JOIN Wallets AS W
    ON FT.WalletId = W.WalletId

INNER JOIN Categories AS C
    ON FT.CategoryId = C.CategoryId

WHERE FT.Description LIKE N'Batch60:%'

ORDER BY
    U.UserId,
    FT.TransactionId;
GO


/* ============================================================
   16. XEM 10 NGÂN SÁCH VÀ TRẠNG THÁI
   ============================================================ */

SELECT
    B.BudgetId,
    U.FullName,
    C.CategoryName,
    B.[Month],
    B.[Year],
    B.AmountLimit,

    ISNULL(SUM(FT.Amount), 0) AS AmountSpent,

    B.AmountLimit
        - ISNULL(SUM(FT.Amount), 0) AS RemainingAmount,

    CAST
    (
        ISNULL(SUM(FT.Amount), 0)
        / NULLIF(B.AmountLimit, 0)
        * 100
        AS DECIMAL(10,2)
    ) AS UsagePercentage,

    CASE
        WHEN
        (
            ISNULL(SUM(FT.Amount), 0)
            / NULLIF(B.AmountLimit, 0)
            * 100
        ) < 80
            THEN N'Bình thường'

        WHEN
        (
            ISNULL(SUM(FT.Amount), 0)
            / NULLIF(B.AmountLimit, 0)
            * 100
        ) < 100
            THEN N'Sắp vượt'

        ELSE N'Đã vượt ngân sách'
    END AS BudgetStatus

FROM Budgets AS B

INNER JOIN Users AS U
    ON B.UserId = U.UserId

INNER JOIN Categories AS C
    ON B.CategoryId = C.CategoryId

LEFT JOIN FinanceTransactions AS FT
    ON FT.UserId = B.UserId
    AND FT.CategoryId = B.CategoryId
    AND FT.TransactionType = 'Expense'
    AND MONTH(FT.TransactionDate) = B.[Month]
    AND YEAR(FT.TransactionDate) = B.[Year]

WHERE B.Note LIKE N'Batch60:%'

GROUP BY
    B.BudgetId,
    U.FullName,
    C.CategoryName,
    B.[Month],
    B.[Year],
    B.AmountLimit

ORDER BY
    B.BudgetId;
GO