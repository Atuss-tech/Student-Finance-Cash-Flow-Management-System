using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TransactionDAO
    {
        private static readonly TransactionDAO instance = 
                new TransactionDAO();

        private TransactionDAO() { }

        public static TransactionDAO Instance => instance;

        // Lấy toàn bộ giao dịch của một người dùng.
        public List<FinanceTransaction> GetTransactionsByUserId(
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.FinanceTransactions
                .AsNoTracking()
                .Include(transaction => transaction.Wallet)
                .Include(transaction => transaction.Category)
                .Where(transaction => transaction.UserId == userId)
                .OrderByDescending(transaction =>
                    transaction.TransactionDate)
                .ThenByDescending(transaction =>
                    transaction.TransactionId)
                .ToList();
        }

        // Lấy một giao dịch theo id và user.
        public FinanceTransaction? GetTransactionById(
            int transactionId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.FinanceTransactions
                .AsNoTracking()
                .Include(transaction => transaction.Wallet)
                .Include(transaction => transaction.Category)
                .FirstOrDefault(transaction =>
                    transaction.TransactionId == transactionId &&
                    transaction.UserId == userId);
        }
        // Lấy ví để Service kiểm tra nghiệp vụ.
        public Wallet? GetWalletById(
            int walletId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.Wallets
                .AsNoTracking()
                .FirstOrDefault(wallet =>
                    wallet.WalletId == walletId &&
                    wallet.UserId == userId);
        }
        // Lấy danh mục để Service kiểm tra nghiệp vụ.
        public Category? GetCategoryById(
            int categoryId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.Categories
                .AsNoTracking()
                .FirstOrDefault(category =>
                    category.CategoryId == categoryId &&
                    category.UserId == userId);
        }
        // Thêm giao dịch và cập nhật số dư ví.
        public void AddTransaction(
            FinanceTransaction transaction)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            using var dbTransaction =
                db.Database.BeginTransaction();

            try
            {
                Wallet wallet = db.Wallets.First(item =>
                    item.WalletId == transaction.WalletId &&
                    item.UserId == transaction.UserId);

                ApplyTransactionToWallet(
                    wallet,
                    transaction.TransactionType,
                    transaction.Amount);

                db.FinanceTransactions.Add(transaction);
                db.SaveChanges();

                dbTransaction.Commit();
            }
            catch
            {
                dbTransaction.Rollback();
                throw;
            }
        }
        // Cập nhật giao dịch và tính lại số dư ví.
        public void UpdateTransaction(
            int transactionId,
            int userId,
            int newWalletId,
            int newCategoryId,
            string newTransactionType,
            decimal newAmount,
            DateTime newTransactionDate,
            string? newDescription)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            using var dbTransaction =
                db.Database.BeginTransaction();

            try
            {
                FinanceTransaction oldTransaction =
                    db.FinanceTransactions.First(transaction =>
                        transaction.TransactionId == transactionId &&
                        transaction.UserId == userId);

                Wallet oldWallet = db.Wallets.First(wallet =>
                    wallet.WalletId == oldTransaction.WalletId &&
                    wallet.UserId == userId);

                // Hoàn tác ảnh hưởng của giao dịch cũ.
                UndoTransactionFromWallet(
                    oldWallet,
                    oldTransaction.TransactionType,
                    oldTransaction.Amount);

                Wallet newWallet = db.Wallets.First(wallet =>
                    wallet.WalletId == newWalletId &&
                    wallet.UserId == userId);

                // Áp dụng giao dịch mới.
                ApplyTransactionToWallet(
                    newWallet,
                    newTransactionType,
                    newAmount);

                oldTransaction.WalletId = newWalletId;
                oldTransaction.CategoryId = newCategoryId;
                oldTransaction.TransactionType = newTransactionType;
                oldTransaction.Amount = newAmount;
                oldTransaction.TransactionDate =
                    DateOnly.FromDateTime(newTransactionDate);
                oldTransaction.Description = newDescription;
                oldTransaction.UpdatedAt = DateTime.Now;

                db.SaveChanges();
                dbTransaction.Commit();
            }
            catch
            {
                dbTransaction.Rollback();
                throw;
            }
        }
        // Xóa giao dịch và hoàn lại số dư ví.
        public void DeleteTransaction(
            int transactionId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            using var dbTransaction =
                db.Database.BeginTransaction();

            try
            {
                FinanceTransaction transaction =
                    db.FinanceTransactions.First(item =>
                        item.TransactionId == transactionId &&
                        item.UserId == userId);

                Wallet wallet = db.Wallets.First(item =>
                    item.WalletId == transaction.WalletId &&
                    item.UserId == userId);

                UndoTransactionFromWallet(
                    wallet,
                    transaction.TransactionType,
                    transaction.Amount);

                db.FinanceTransactions.Remove(transaction);
                db.SaveChanges();

                dbTransaction.Commit();
            }
            catch
            {
                dbTransaction.Rollback();
                throw;
            }
        }
        // Income làm tăng ví, Expense làm giảm ví.
        private static void ApplyTransactionToWallet(
            Wallet wallet,
            string transactionType,
            decimal amount)
        {
            if (transactionType == "Income")
            {
                wallet.Balance += amount;
                return;
            }

            if (wallet.Balance < amount)
            {
                throw new InvalidOperationException(
                    "Số dư ví không đủ để thực hiện giao dịch chi.");
            }

            wallet.Balance -= amount;
        }
        // Hoàn tác giao dịch cũ.
        private static void UndoTransactionFromWallet(
            Wallet wallet,
            string transactionType,
            decimal amount)
        {
            if (transactionType == "Income")
            {
                if (wallet.Balance < amount)
                {
                    throw new InvalidOperationException(
                        "Không thể xóa hoặc sửa giao dịch thu "
                        + "vì số dư ví sẽ bị âm.");
                }

                wallet.Balance -= amount;
                return;
            }

            wallet.Balance += amount;
        }

    }
}
