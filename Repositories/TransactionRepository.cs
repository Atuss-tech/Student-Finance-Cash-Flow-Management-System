using BusinessObjects.Models;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    /// <summary>
    /// Repository trung gian cho Entity FinanceTransaction.
    /// Chuyển tiếp các yêu cầu dữ liệu từ Service xuống TransactionDAO.
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        /// <summary>
        /// Lấy giao dịch trong tháng thông qua DAO.
        /// </summary>
        public async Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year)
        {
            return await TransactionDAO.Instance.GetTransactionsByMonthAsync(userId, month, year);
        }

        /// <summary>
        /// Lấy giao dịch trong năm thông qua DAO.
        /// </summary>
        public async Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year)
        {
            return await TransactionDAO.Instance.GetTransactionsByYearAsync(userId, year);
        }

        /// <summary>
        /// Lấy chi tiết một giao dịch thông qua DAO.
        /// </summary>
        public async Task<FinanceTransaction?> GetTransactionByIdAsync(int id)
        {
            return await TransactionDAO.Instance.GetTransactionByIdAsync(id);
        }

        /// <summary>
        /// Gọi DAO để lưu giao dịch mới.
        /// </summary>
        public async Task AddTransactionAsync(FinanceTransaction transaction)
        {
            await TransactionDAO.Instance.AddTransactionAsync(transaction);
        }

        /// <summary>
        /// Gọi DAO để cập nhật giao dịch.
        /// </summary>
        public async Task UpdateTransactionAsync(FinanceTransaction transaction)
        {
            await TransactionDAO.Instance.UpdateTransactionAsync(transaction);
        }

        /// <summary>
        /// Gọi DAO để xóa giao dịch.
        /// </summary>
        public async Task DeleteTransactionAsync(int id)
        {
            await TransactionDAO.Instance.DeleteTransactionAsync(id);
        }
    }
}
