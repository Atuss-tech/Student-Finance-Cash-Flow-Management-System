using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBudgetRepository
    {
        Task<List<Budget>> GetBudgetsAsync(int userId, int month, int year);
        Task<Budget?> GetBudgetAsync(int userId, int categoryId, int month, int year);
        Task AddBudgetAsync(Budget budget);
        Task UpdateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int budgetId);
    }
}
