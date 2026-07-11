using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    // Khai báo các chức năng Repository của Category.
    public interface ICategoryRepository
    {
        List<Category> GetCategoriesByUserId(int userId);

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
