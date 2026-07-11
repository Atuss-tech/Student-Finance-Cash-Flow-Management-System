using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITransactionRepository
    {
        List<FinanceTransaction> GetTransactionsByUserId(
            int userId);

        FinanceTransaction? GetTransactionById(
            int transactionId,
            int userId);

        Wallet? GetWalletById(
            int walletId,
            int userId);

        Category? GetCategoryById(
            int categoryId,
            int userId);
        void AddTransaction(
            FinanceTransaction transaction);

        void UpdateTransaction(
            int transactionId,
            int userId,
            int newWalletId,
            int newCategoryId,
            string newTransactionType,
            decimal newAmount,
            DateTime newTransactionDate,
            string? newDescription);

        void DeleteTransaction(
            int transactionId,
            int userId);

        Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year);
        Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year);
        Task<FinanceTransaction?> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(FinanceTransaction transaction);
        Task UpdateTransactionAsync(FinanceTransaction transaction);
        Task DeleteTransactionAsync(int id);
    }
}
