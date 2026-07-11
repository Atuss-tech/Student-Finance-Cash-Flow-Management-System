using BusinessObjects.Models;
using BusinessObjects.Models;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    /// <summary>
    /// Repository trung gian thực hiện pattern Repository cho Entity Budget.
    /// Nhận request từ Service và chuyển tiếp xuống Data Access Object.
    /// </summary>
    public class BudgetRepository : IBudgetRepository
    {
        /// <summary>
        /// Chuyển tiếp yêu cầu lấy danh sách ngân sách.
        /// </summary>
        public async Task<List<Budget>> GetBudgetsAsync(int userId, int month, int year)
        {
            return await BudgetDAO.Instance.GetBudgetsAsync(userId, month, year);
        }

        /// <summary>
        /// Chuyển tiếp yêu cầu lấy một ngân sách cụ thể.
        /// </summary>
        public async Task<Budget?> GetBudgetAsync(int userId, int categoryId, int month, int year)
        {
            return await BudgetDAO.Instance.GetBudgetAsync(userId, categoryId, month, year);
        }

        /// <summary>
        /// Chuyển tiếp yêu cầu thêm mới ngân sách.
        /// </summary>
        public async Task AddBudgetAsync(Budget budget)
        {
            await BudgetDAO.Instance.AddBudgetAsync(budget);
        }

        /// <summary>
        /// Chuyển tiếp yêu cầu cập nhật ngân sách.
        /// </summary>
        public async Task UpdateBudgetAsync(Budget budget)
        {
            await BudgetDAO.Instance.UpdateBudgetAsync(budget);
        }

        /// <summary>
        /// Chuyển tiếp yêu cầu xóa ngân sách.
        /// </summary>
        public async Task DeleteBudgetAsync(int budgetId)
        {
            await BudgetDAO.Instance.DeleteBudgetAsync(budgetId);
        }
    }
}
