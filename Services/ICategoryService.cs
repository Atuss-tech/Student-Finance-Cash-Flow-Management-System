using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICategoryService
    {
        List<Category> GetCategoriesByUserId(int userId);

        List<Category> GetActiveCategoriesByType(
            int userId,
            string categoryType);

        void AddCategory(
            int userId,
            string categoryName,
            string categoryType,
            string? description);

        void UpdateCategory(
            int userId,
            int categoryId,
            string categoryName,
            string categoryType,
            string? description);

        // true: danh mục được chuyển thành ngừng sử dụng.
        // false: danh mục được xóa hoàn toàn.
        bool DeleteCategory(
            int userId,
            int categoryId);
    }
}
