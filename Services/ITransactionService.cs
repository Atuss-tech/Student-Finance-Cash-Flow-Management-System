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
    }
}
