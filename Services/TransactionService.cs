using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    // Xử lý nghiệp vụ giao dịch thu và chi.
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository
            transactionRepository;
        //Khởi tạo TransactionRepository để sử dụng các phương thức truy xuất dữ liệu.
        public TransactionService()
        {
            transactionRepository =
                new TransactionRepository();
        }

        public List<FinanceTransaction> GetTransactionsByUserId(int userId)
        {
            CheckUserId(userId);

            return transactionRepository
                .GetTransactionsByUserId(userId);
        }

        public void AddTransaction(int userId, int walletId, int categoryId, string transactionType, decimal amount, DateTime transactionDate, string? description)
        {
            string finalTransactionType =
                 GetTransactionType(transactionType);

            // Khi thêm mới giao dịch chi, cần kiểm tra số dư hiện tại của ví.
            CheckTransactionInput(
                userId,
                walletId,
                categoryId,
                finalTransactionType,
                amount,
                transactionDate,
                checkCurrentBalance: true);

            FinanceTransaction transaction =
               new FinanceTransaction
               {
                   UserId = userId,
                   WalletId = walletId,
                   CategoryId = categoryId,
                   TransactionType = finalTransactionType,
                   Amount = amount,
                   TransactionDate =
                       DateOnly.FromDateTime(transactionDate),
                   Description = GetDescription(description),
                   CreatedAt = DateTime.Now
               };

            // DAO sẽ lưu giao dịch và tự cộng/trừ số dư ví.
            transactionRepository.AddTransaction(transaction);
        }
        public void UpdateTransaction(int userId, int transactionId, int walletId, int categoryId, string transactionType, decimal amount, DateTime transactionDate, string? description)
        {
            CheckUserId(userId);

            CheckTransactionExists(
                transactionId,
                userId);

            string finalTransactionType =
                GetTransactionType(transactionType);

            // Khi sửa giao dịch, không kiểm tra số dư hiện tại ở Service.
            // Lý do: DAO sẽ hoàn tác giao dịch cũ trước,
            // rồi mới áp dụng giao dịch mới trong transaction database.
            CheckTransactionInput(
                userId,
                walletId,
                categoryId,
                finalTransactionType,
                amount,
                transactionDate,
                checkCurrentBalance: false);

            transactionRepository.UpdateTransaction(
                transactionId,
                userId,
                walletId,
                categoryId,
                finalTransactionType,
                amount,
                transactionDate,
                GetDescription(description));

        }

        public void DeleteTransaction(int userId, int transactionId)
        {
            CheckUserId(userId);

            CheckTransactionExists(
                transactionId,
                userId);

            // DAO sẽ tự hoàn lại số dư ví.
            transactionRepository.DeleteTransaction(
                transactionId,
                userId);


        }

        // Gom toàn bộ kiểm tra dữ liệu giao dịch vào một hàm.
        private void CheckTransactionInput(
            int userId,
            int walletId,
            int categoryId,
            string transactionType,
            decimal amount,
            DateTime transactionDate,
            bool checkCurrentBalance)
        {
            CheckUserId(userId);
            CheckAmount(amount);
            CheckTransactionDate(transactionDate);

            Wallet wallet =
                GetWallet(walletId, userId);

            Category category =
                GetCategory(categoryId, userId);

            CheckCategoryType(
                category,
                transactionType);

            if (checkCurrentBalance)
            {
                CheckWalletBalance(
                    wallet,
                    transactionType,
                    amount);
            }
        }

        // Kiểm tra UserId.
        private static void CheckUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException(
                    "Người dùng không hợp lệ.");
            }
        }
        // Kiểm tra số tiền.
        private static void CheckAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException(
                    "Số tiền phải lớn hơn 0.");
            }
        }

        // Kiểm tra ngày giao dịch.
        private static void CheckTransactionDate(
            DateTime transactionDate)
        {
            if (transactionDate.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "Ngày giao dịch không được lớn hơn ngày hiện tại.");
            }
        }

        // Chuẩn hóa loại giao dịch.
        private static string GetTransactionType(
            string transactionType)
        {
            if (string.IsNullOrWhiteSpace(transactionType))
            {
                throw new ArgumentException(
                    "Vui lòng chọn loại giao dịch.");
            }

            string finalType =
                transactionType.Trim();

            if (finalType.Equals(
                "Income",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Income";
            }

            if (finalType.Equals(
               "Expense",
               StringComparison.OrdinalIgnoreCase))
            {
                return "Expense";
            }

            throw new ArgumentException(
                "Loại giao dịch chỉ được là Income hoặc Expense.");
        }
        // Lấy ví còn hoạt động.
        private Wallet GetWallet(
            int walletId,
            int userId)
        {
            Wallet? wallet =
                transactionRepository.GetWalletById(
                    walletId,
                    userId);

            if (wallet == null)
            {
                throw new InvalidOperationException(
                    "Ví không tồn tại.");
            }

            if (!wallet.IsActive)
            {
                throw new InvalidOperationException(
                    "Ví đã ngừng sử dụng.");
            }

            return wallet;
        }

        // Lấy danh mục còn hoạt động.
        private Category GetCategory(
            int categoryId,
            int userId)
        {
            Category? category =
                transactionRepository.GetCategoryById(
                    categoryId,
                    userId);

            if (category == null)
            {
                throw new InvalidOperationException(
                    "Danh mục không tồn tại.");
            }

            if (!category.IsActive)
            {
                throw new InvalidOperationException(
                    "Danh mục đã ngừng sử dụng.");
            }

            return category;
        }

        // Kiểm tra loại danh mục có khớp loại giao dịch không.
        private static void CheckCategoryType(
            Category category,
            string transactionType)
        {
            if (category.CategoryType != transactionType)
            {
                throw new InvalidOperationException(
                    "Loại danh mục không khớp với loại giao dịch.");
            }
        }

        // Kiểm tra số dư khi thêm khoản chi.
        private static void CheckWalletBalance(
            Wallet wallet,
            string transactionType,
            decimal amount)
        {
            if (transactionType == "Expense" &&
                wallet.Balance < amount)
            {
                throw new InvalidOperationException(
                    "Số dư ví không đủ để chi tiêu.");
            }
        }

        // Kiểm tra giao dịch có tồn tại không.
        private void CheckTransactionExists(
            int transactionId,
            int userId)
        {
            if (transactionId <= 0)
            {
                throw new ArgumentException(
                    "Giao dịch không hợp lệ.");
            }

            FinanceTransaction? transaction =
                transactionRepository.GetTransactionById(
                    transactionId,
                    userId);

            if (transaction == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy giao dịch.");
            }
        }
        // Chuẩn hóa mô tả.
        private static string? GetDescription(
            string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return null;
            }

            string finalDescription =
                description.Trim();

            if (finalDescription.Length > 256)
            {
                throw new ArgumentException(
                    "Mô tả không được vượt quá 256 ký tự.");
            }

            return finalDescription;
        }




    }
}
