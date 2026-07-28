using BusinessObjects.Models;
using System.Collections.Generic;

namespace Services
{
    public interface ICategoryService
    {
        // Lấy danh sách danh mục thuộc 1 người dùng.
        List<Category> GetCategoriesByUserId(int userId);

        // Lấy toàn bộ danh mục trong hệ thống (không lọc userId).
        List<Category> GetAllCategories();

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

        // Trả về true: danh mục được chuyển thành ngừng sử dụng.
        // Trả về false: danh mục được xóa hoàn toàn.
        bool DeleteCategory(
            int userId,
            int categoryId);

        // Kiểm tra danh mục đã có giao dịch hoặc ngân sách liên quan chưa.
        bool HasRelatedData(
            int userId,
            int categoryId);
    }
}
