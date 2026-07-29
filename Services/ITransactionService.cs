using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ITransactionService
    {
        List<FinanceTransaction> GetTransactionsByUserId(
            int userId);

        void AddTransaction(
            int userId,
            int walletId,
            int categoryId,
            string transactionType,
            decimal amount,
            DateTime transactionDate,
            string? description);

        Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year);
        Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year);
        Task<FinanceTransaction?> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(FinanceTransaction transaction);
    }
}
