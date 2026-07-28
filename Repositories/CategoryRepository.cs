using BusinessObjects.Models;
using DataAccess;
using System.Collections.Generic;

namespace Repositories
{
    // Cầu nối giữa CategoryService và CategoryDAO.
    public class CategoryRepository : ICategoryRepository
    {
        // Lấy danh sách danh mục thuộc 1 người dùng.
        public List<Category> GetCategoriesByUserId(int userId)
        {
            return CategoryDAO.Instance
                .GetCategoriesByUserId(userId);
        }

        // Lấy toàn bộ danh mục (không lọc userId).
        public List<Category> GetAllCategories()
        {
            return CategoryDAO.Instance.GetAllCategories();
        }

        // Lấy danh mục đang hoạt động theo loại (Income/Expense).
        public List<Category> GetActiveCategoriesByType(
           int userId,
           string categoryType)
        {
            return CategoryDAO.Instance
                .GetActiveCategoriesByType(
                    userId,
                    categoryType);
        }

        // Lấy một danh mục theo ID và userId.
        public Category? GetCategoryById(
           int categoryId,
           int userId)
        {
            return CategoryDAO.Instance
                .GetCategoryById(
                    categoryId,
                    userId);
        }

        // Kiểm tra xem tên danh mục đã tồn tại chưa.
        public bool IsCategoryNameExists(
            int userId,
            string categoryName,
            string categoryType,
            int? ignoredCategoryId = null)
        {
            return CategoryDAO.Instance
                .IsCategoryNameExists(
                    userId,
                    categoryName,
                    categoryType,
                    ignoredCategoryId);
        }

        public void AddCategory(Category category)
        {
            CategoryDAO.Instance.AddCategory(category);
        }

        public void UpdateCategory(
            int categoryId,
            int userId,
            string categoryName,
            string categoryType,
            string? description)
        {
            CategoryDAO.Instance.UpdateCategory(
                categoryId,
                userId,
                categoryName,
                categoryType,
                description);
        }

        public void DeactivateCategory(
            int categoryId,
            int userId)
        {
            CategoryDAO.Instance
                .DeactivateCategory(categoryId, userId);
        }

        public void DeleteCategory(
            int categoryId,
            int userId)
        {
            CategoryDAO.Instance
                .DeleteCategory(categoryId, userId);
        }

        // Kiểm tra xem danh mục có dữ liệu liên quan không.
        public bool HasRelatedData(
            int categoryId,
            int userId)
        {
            return CategoryDAO.Instance
                .HasRelatedData(categoryId, userId);
        }
    }
}
