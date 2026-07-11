using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        void UpdateTransaction(
            int userId,
            int transactionId,
            int walletId,
            int categoryId,
            string transactionType,
            decimal amount,
            DateTime transactionDate,
            string? description);

        void DeleteTransaction(
            int userId,
            int transactionId);

        Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year);
        Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year);
        Task<FinanceTransaction?> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(FinanceTransaction transaction);
        Task UpdateTransactionAsync(FinanceTransaction transaction);
        Task DeleteTransactionAsync(int id);
    }
}
