using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public void AddTransaction(FinanceTransaction transaction)
        {
            TransactionDAO.Instance
                .AddTransaction(transaction);
        }

        public void DeleteTransaction(int transactionId, int userId)
        {
            TransactionDAO.Instance
                .DeleteTransaction(
                    transactionId,
                    userId);
        }

        public Category? GetCategoryById(int categoryId, int userId)
        {
            return TransactionDAO.Instance
                .GetCategoryById(
                    categoryId,
                    userId);
        }

        public FinanceTransaction? GetTransactionById(int transactionId, int userId)
        {
            return TransactionDAO.Instance
                .GetTransactionById(
                    transactionId,
                    userId);
        }

        public List<FinanceTransaction> GetTransactionsByUserId(int userId)
        {
            return TransactionDAO.Instance
                .GetTransactionsByUserId(userId);
        }

        public Wallet? GetWalletById(int walletId, int userId)
        {
            return TransactionDAO.Instance
               .GetWalletById(
                   walletId,
                   userId);
        }

        public void UpdateTransaction(int transactionId, int userId, int newWalletId, int newCategoryId, string newTransactionType, decimal newAmount, DateTime newTransactionDate, string? newDescription)
        {
            TransactionDAO.Instance.UpdateTransaction(
                transactionId,
                userId,
                newWalletId,
                newCategoryId,
                newTransactionType,
                newAmount,
                newTransactionDate,
                newDescription);
        }

        public async Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year)
        {
            return await TransactionDAO.Instance.GetTransactionsByMonthAsync(userId, month, year);
        }

        public async Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year)
        {
            return await TransactionDAO.Instance.GetTransactionsByYearAsync(userId, year);
        }

        public async Task<FinanceTransaction?> GetTransactionByIdAsync(int id)
        {
            return await TransactionDAO.Instance.GetTransactionByIdAsync(id);
        }

        public async Task AddTransactionAsync(FinanceTransaction transaction)
        {
            await TransactionDAO.Instance.AddTransactionAsync(transaction);
        }

        public async Task UpdateTransactionAsync(FinanceTransaction transaction)
        {
            await TransactionDAO.Instance.UpdateTransactionAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await TransactionDAO.Instance.DeleteTransactionAsync(id);
        }
    }
}
