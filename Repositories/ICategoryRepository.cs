using BusinessObjects.Models;
using System.Collections.Generic;

namespace Repositories
{
    // Khai báo các chức năng Repository của Category.
    public interface ICategoryRepository
    {
        // Lấy danh sách danh mục thuộc 1 người dùng.
        List<Category> GetCategoriesByUserId(int userId);

        // Lấy toàn bộ danh mục trong hệ thống (không lọc userId).
        List<Category> GetAllCategories();

        List<Category> GetActiveCategoriesByType(
            int userId,
            string categoryType);

        Category? GetCategoryById(
            int categoryId,
            int userId);

        bool IsCategoryNameExists(
            int userId,
            string categoryName,
            string categoryType,
            int? ignoredCategoryId = null);

        void AddCategory(Category category);

        void UpdateCategory(
            int categoryId,
            int userId,
            string categoryName,
            string categoryType,
            string? description);

        bool HasRelatedData(
            int categoryId,
            int userId);

        void DeleteCategory(
            int categoryId,
            int userId);

        void DeactivateCategory(
            int categoryId,
            int userId);
    }
}
