using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IReportService
    {
        Task<(decimal TotalIncome, decimal TotalExpense, decimal Balance, int Month, int Year)> GetMonthlyReportAsync(int userId, int month, int year);
        Task<Dictionary<string, decimal>> GetExpenseByCategoryAsync(int userId, int month, int year);
        Task<Dictionary<string, decimal>> GetCashFlowTrendAsync(int userId, int year);
    }
}
