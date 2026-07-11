using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Data Access Object xử lý các thao tác CRUD với bảng Budget trong Database.
    /// Sử dụng Entity Framework Core theo mô hình Singleton.
    /// </summary>
    public class BudgetDAO
    {
        private static readonly Lazy<BudgetDAO> instance = new Lazy<BudgetDAO>(() => new BudgetDAO());
        private BudgetDAO() { }
        
        /// <summary>
        /// Thể hiện duy nhất (Singleton Instance) của BudgetDAO.
        /// </summary>
        public static BudgetDAO Instance => instance.Value;

        /// <summary>
        /// Lấy tất cả ngân sách của một user trong một tháng/năm.
        /// </summary>
        public async Task<List<Budget>> GetBudgetsAsync(int userId, int month, int year)
        {
            using (var db = new StudentFinanceDbContext())
            {
                return await db.Budgets
                    .Include(b => b.Category)
                    .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// Tìm kiếm một ngân sách cụ thể của một danh mục trong tháng.
        /// Dùng để kiểm tra trùng lặp.
        /// </summary>
        public async Task<Budget?> GetBudgetAsync(int userId, int categoryId, int month, int year)
        {
            using (var db = new StudentFinanceDbContext())
            {
                return await db.Budgets
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.CategoryId == categoryId && b.Month == month && b.Year == year);
            }
        }

        /// <summary>
        /// Thêm mới một bản ghi ngân sách vào cơ sở dữ liệu.
        /// </summary>
        public async Task AddBudgetAsync(Budget budget)
        {
            using (var db = new StudentFinanceDbContext())
            {
                db.Budgets.Add(budget);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Cập nhật thông tin ngân sách (ví dụ: thay đổi giới hạn tiền).
        /// </summary>
        public async Task UpdateBudgetAsync(Budget budget)
        {
            using (var db = new StudentFinanceDbContext())
            {
                db.Entry(budget).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Xóa ngân sách theo khóa chính (ID).
        /// </summary>
        public async Task DeleteBudgetAsync(int id)
        {
            using (var db = new StudentFinanceDbContext())
            {
                var budget = await db.Budgets.FindAsync(id);
                if (budget != null)
                {
                    db.Budgets.Remove(budget);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
