using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IBudgetService
    {
        Task AddBudgetAsync(Budget budget);
        Task UpdateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int budgetId);
        Task<List<Budget>> GetBudgetsAsync(int userId, int month, int year);
        Task<List<(int BudgetId, int CategoryId, string CategoryName, decimal AmountLimit, decimal SpentAmount, double UsagePercentage, string AlertStatus)>> GetBudgetProgressesAsync(int userId, int month, int year);
    }
}
