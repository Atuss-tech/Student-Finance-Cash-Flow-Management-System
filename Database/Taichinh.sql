/* ============================================================
   PROJECT: STUDENT FINANCE MANAGEMENT SYSTEM
   DATABASE: StudentFinanceDb
   SQL SERVER
   ============================================================ */

USE master;
GO


/* ============================================================
   2. TẠO DATABASE
   ============================================================ */

CREATE DATABASE StudentFinanceDb;
GO

USE StudentFinanceDb;
GO


/* ============================================================
   3. TẠO BẢNG USERS
   Lưu tài khoản người dùng
   ============================================================ */

CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,

    FullName NVARCHAR(100) NOT NULL,

    Email VARCHAR(150) NOT NULL,

    PasswordHash NVARCHAR(20) NOT NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt
        DEFAULT SYSDATETIME(),

    IsActive BIT NOT NULL
        CONSTRAINT DF_Users_IsActive
        DEFAULT 1,

    CONSTRAINT UQ_Users_Email
        UNIQUE (Email)

    -- thiếu tên đăng nhập 
);
GO


/* ============================================================
   4. TẠO BẢNG WALLETS
   Lưu các ví tiền của người dùng
   ============================================================ */

CREATE TABLE Wallets
(
    WalletId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    WalletName NVARCHAR(100) NOT NULL,

    WalletType VARCHAR(20) NOT NULL,
    --Số dư ban đầu khi bạn vừa tạo ví.
    InitialBalance DECIMAL(18,2) NOT NULL 
        CONSTRAINT DF_Wallets_InitialBalance
        DEFAULT 0,
    --Số dư hiện tại còn lại trong ví sau khi thu hoặc chi.
    Balance DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Wallets_Balance
        DEFAULT 0,

    Note NVARCHAR(500) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Wallets_IsActive
        DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Wallets_CreatedAt
        DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Wallets_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT CK_Wallets_Type
        CHECK
        (
            WalletType IN
            (
                'Cash',
                'Bank',
                'EWallet'
            )
        ),

    CONSTRAINT CK_Wallets_InitialBalance
        CHECK (InitialBalance >= 0),

    CONSTRAINT CK_Wallets_Balance
        CHECK (Balance >= 0),

    CONSTRAINT UQ_Wallets_User_WalletName
        UNIQUE
        (
            UserId,
            WalletName
        )
);
GO


/* ============================================================
   5. TẠO BẢNG CATEGORIES
   Lưu danh mục thu nhập và chi tiêu
   ============================================================ */

CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    CategoryName NVARCHAR(100) NOT NULL,

    CategoryType VARCHAR(20) NOT NULL,

    Description NVARCHAR(200) NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Categories_IsActive
        DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Categories_CreatedAt
        DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Categories_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT CK_Categories_Type
        CHECK
        (
            CategoryType IN
            (
                'Income',
                'Expense'
            )
        ),

    CONSTRAINT UQ_Categories_User_Name_Type
        UNIQUE
        (
            UserId,
            CategoryName,
            CategoryType
        )
);
GO


/* ============================================================
   6. TẠO BẢNG FINANCE TRANSACTIONS
   Lưu giao dịch thu nhập và chi tiêu
   ============================================================ */

CREATE TABLE FinanceTransactions
(
    TransactionId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    WalletId INT NOT NULL,

    CategoryId INT NOT NULL,

    TransactionType VARCHAR(20) NOT NULL,
    --Số tiền của giao dịch.
    Amount DECIMAL(18,2) NOT NULL,
    -- chỗ này chỉ lưu ngày, không có giờ phút
    TransactionDate DATE NOT NULL,

    Description NVARCHAR(200) NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_FinanceTransactions_CreatedAt
        DEFAULT SYSDATETIME(),

    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_FinanceTransactions_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_FinanceTransactions_Wallets
        FOREIGN KEY (WalletId)
        REFERENCES Wallets(WalletId),

    CONSTRAINT FK_FinanceTransactions_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId),

    CONSTRAINT CK_FinanceTransactions_Type
        CHECK
        (
            TransactionType IN
            (
                'Income',
                'Expense'
            )
        ),

    CONSTRAINT CK_FinanceTransactions_Amount
        CHECK (Amount > 0),

    CONSTRAINT CK_FinanceTransactions_Date
        CHECK
        (
            TransactionDate <= CAST(GETDATE() AS DATE)
        )
);
GO


/* ============================================================
   7. TẠO BẢNG BUDGETS
   Lưu ngân sách theo tháng và danh mục chi tiêu
   ============================================================ */

CREATE TABLE Budgets
(
    BudgetId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    CategoryId INT NOT NULL,

    [Month] INT NOT NULL,

    [Year] INT NOT NULL,

    AmountLimit DECIMAL(18,2) NOT NULL,

    Note NVARCHAR(200) NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Budgets_CreatedAt
        DEFAULT SYSDATETIME(),

    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Budgets_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Budgets_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId),

    CONSTRAINT CK_Budgets_Month
        CHECK
        (
            [Month] BETWEEN 1 AND 12
        ),

    CONSTRAINT CK_Budgets_Year
        CHECK
        (
            [Year] BETWEEN 2000 AND 2100
        ),

    CONSTRAINT CK_Budgets_AmountLimit
        CHECK
        (
            AmountLimit > 0
        ),

    CONSTRAINT UQ_Budgets_User_Category_Month_Year
        UNIQUE
        (
            UserId,
            CategoryId,
            [Month],
            [Year]
        )
);
GO


