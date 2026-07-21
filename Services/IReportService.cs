using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IReportService
    {
        Task<(decimal TotalIncome, decimal TotalExpense, decimal Balance, int Month, int Year)> GetMonthlyReportAsync(int userId, int month, int year);
        Task<Dictionary<string, decimal>> GetExpenseByCategoryAsync(int userId, int month, int year);
        Task<Dictionary<string, decimal>> GetCashFlowTrendAsync(int userId, int year);

        /// <summary>
        /// Nhóm chi tiêu theo 4 nhóm lớn: Nhu cầu thiết yếu, Sở thích cá nhân, Tích lũy, Tương lai.
        /// </summary>
        Task<Dictionary<string, decimal>> GetExpenseBySpendingGroupAsync(int userId, int month, int year);
    }
}
