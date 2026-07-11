using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    // Truy cập trực tiếp dữ liệu bảng Categories.
    public class CategoryDAO
    {
        private static readonly CategoryDAO instance =
            new CategoryDAO();

        private CategoryDAO()
        {
        }

        public static CategoryDAO Instance = instance;

        //Lấy danh sách danh mục thuộc 1 người dùng

        public List<Category> GetCategoriesByUserId(int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.Categories
                .AsNoTracking()
                .Where(category => category.UserId == userId)
                .OrderByDescending(category => category.IsActive)
                .ThenBy(category => category.CategoryType)
                .ThenBy(category => category.CategoryName)
                .ToList();
        }
        //Lấy các danh mục đang hoạt động theo loại Income hoặc Expense
        //Hàm này sử dụng khi làm chức năng giao dịch
        public List<Category> GetActiveCategoriesByType(
            int userId,
            string categoryType)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.Categories
                .AsNoTracking()
                .Where(category =>
                    category.UserId == userId &&
                    category.CategoryType == categoryType &&
                    category.IsActive)
                .OrderBy(category => category.CategoryName)
                .ToList();
        }
        // Lấy một danh mục và kiểm tra đúng người sở hữu.
        public Category? GetCategoryById(
            int categoryId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.Categories
                .AsNoTracking()
                .FirstOrDefault(category =>
                    category.CategoryId == categoryId &&
                    category.UserId == userId);
        }
        // Kiểm tra tên danh mục đã tồn tại hay chưa.
        // Một người dùng có thể có cùng tên nếu khác loại.
        
        public bool IsCategoryNameExists(
            int userId,
            string categoryName,
            string categoryType,
            int? ignoredCategoryId = null)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            string normalizedName =
                categoryName.Trim().ToLower();

            return db.Categories.Any(category =>
                category.UserId == userId &&
                category.CategoryType == categoryType &&
                category.CategoryName.ToLower() == normalizedName &&
                (
                    ignoredCategoryId == null ||
                    category.CategoryId != ignoredCategoryId.Value
                ));
        }
        // Thêm danh mục mới.
        public void AddCategory(Category category)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            db.Categories.Add(category);
            db.SaveChanges();
        }
        // Cập nhật danh mục.
        public void UpdateCategory(
            int categoryId,
            int userId,
            string categoryName,
            string categoryType,
            string? description)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            Category? category = db.Categories
                .FirstOrDefault(item =>
                    item.CategoryId == categoryId &&
                    item.UserId == userId);

            if (category == null)
            {
                return;
            }

            category.CategoryName = categoryName;
            category.CategoryType = categoryType;
            category.Description = description;

            db.SaveChanges();
        }
        // Kiểm tra danh mục đã được dùng trong giao dịch
        // hoặc ngân sách hay chưa.
        public bool HasRelatedData(
            int categoryId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            bool hasTransactions =
                db.FinanceTransactions.Any(transaction =>
                    transaction.CategoryId == categoryId &&
                    transaction.UserId == userId);

            bool hasBudgets =
                db.Budgets.Any(budget =>
                    budget.CategoryId == categoryId &&
                    budget.UserId == userId);

            return hasTransactions || hasBudgets;
        }
        // Xóa hẳn danh mục chưa được sử dụng.
        public void DeleteCategory(
            int categoryId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            Category? category = db.Categories
                .FirstOrDefault(item =>
                    item.CategoryId == categoryId &&
                    item.UserId == userId);

            if (category == null)
            {
                return;
            }

            db.Categories.Remove(category);
            db.SaveChanges();
        }
        // Ngừng sử dụng danh mục đã có giao dịch hoặc ngân sách.
        public void DeactivateCategory(
            int categoryId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            Category? category = db.Categories
                .FirstOrDefault(item =>
                    item.CategoryId == categoryId &&
                    item.UserId == userId);

            if (category == null)
            {
                return;
            }

            category.IsActive = false;
            db.SaveChanges();
        }








    }
}
