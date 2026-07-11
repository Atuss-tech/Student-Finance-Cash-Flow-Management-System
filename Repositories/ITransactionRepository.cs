using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITransactionRepository
    {
        Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year);
        Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year);
        Task<FinanceTransaction?> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(FinanceTransaction transaction);
        Task UpdateTransactionAsync(FinanceTransaction transaction);
        Task DeleteTransactionAsync(int id);
    }
}
