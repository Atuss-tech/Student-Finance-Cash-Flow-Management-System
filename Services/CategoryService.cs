using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService()
        {
            categoryRepository = new CategoryRepository();
        }

        //Lấy danh sách danh mục thuộc của người dùng
        public List<Category> GetCategoriesByUserId(int userId)
        {
            ValidateUserId(userId);

            return categoryRepository
                .GetCategoriesByUserId(userId);
        }
        // Lấy danh mục đang hoạt động theo loại.
        // Được sử dụng cho ComboBox khi thêm giao dịch.
        public List<Category> GetActiveCategoriesByType(
            int userId,
            string categoryType)
        {
            ValidateUserId(userId);

            string normalizedType =
                NormalizeCategoryType(categoryType);

            return categoryRepository
                .GetActiveCategoriesByType(
                    userId,
                    normalizedType);
        }
        // Thêm danh mục mới.
        public void AddCategory(
            int userId,
            string categoryName,
            string categoryType,
            string? description)
        {
            ValidateUserId(userId);

            string normalizedName =
                ValidateAndNormalizeName(categoryName);

            string normalizedType =
                NormalizeCategoryType(categoryType);

            string? normalizedDescription =
                ValidateAndNormalizeDescription(description);

            bool nameExists =
                categoryRepository.IsCategoryNameExists(
                    userId,
                    normalizedName,
                    normalizedType);

            if (nameExists)
            {
                throw new InvalidOperationException(
                    "Danh mục này đã tồn tại.");
            }

            Category category = new Category
            {
                UserId = userId,
                CategoryName = normalizedName,
                CategoryType = normalizedType,
                Description = normalizedDescription,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            categoryRepository.AddCategory(category);
        }

        // Xóa hoặc ngừng sử dụng danh mục.
        public bool DeleteCategory(
            int userId,
            int categoryId)
        {
            ValidateUserId(userId);

            Category? category =
                categoryRepository.GetCategoryById(
                    categoryId,
                    userId);

            if (category == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy danh mục cần xóa.");
            }

            bool hasRelatedData =
                categoryRepository.HasRelatedData(
                    categoryId,
                    userId);

            // Danh mục đã có giao dịch hoặc ngân sách
            // phải được giữ lại để bảo toàn lịch sử.
            if (hasRelatedData)
            {
                categoryRepository.DeactivateCategory(
                    categoryId,
                    userId);

                return true;
            }

            categoryRepository.DeleteCategory(
                categoryId,
                userId);

            return false;
        }
        



        // Cập nhật danh mục.
        public void UpdateCategory(
            int userId,
            int categoryId,
            string categoryName,
            string categoryType,
            string? description)
        {
            ValidateUserId(userId);

            if (categoryId <= 0)
            {
                throw new ArgumentException(
                    "Danh mục không hợp lệ.");
            }

            Category? currentCategory =
                categoryRepository.GetCategoryById(
                    categoryId,
                    userId);

            if (currentCategory == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy danh mục cần sửa.");
            }

            string normalizedName =
                ValidateAndNormalizeName(categoryName);

            string normalizedType =
                NormalizeCategoryType(categoryType);

            string? normalizedDescription =
                ValidateAndNormalizeDescription(description);

            bool hasRelatedData =
                categoryRepository.HasRelatedData(
                    categoryId,
                    userId);

            // Khi danh mục đã được dùng thì không được đổi Income thành Expense
            // hoặc ngược lại vì sẽ làm sai dữ liệu giao dịch cũ.
            bool typeChanged =
                currentCategory.CategoryType != normalizedType;

            if (hasRelatedData && typeChanged)
            {
                throw new InvalidOperationException(
                    "Không thể đổi loại của danh mục đã được sử dụng.");
            }

            bool nameExists =
                categoryRepository.IsCategoryNameExists(
                    userId,
                    normalizedName,
                    normalizedType,
                    categoryId);

            if (nameExists)
            {
                throw new InvalidOperationException(
                    "Danh mục này đã tồn tại.");
            }

            categoryRepository.UpdateCategory(
                categoryId,
                userId,
                normalizedName,
                normalizedType,
                normalizedDescription);
        }

        private static void ValidateUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException(
                    "Người dùng không hợp lệ.");
            }
        }
        // Kiểm tra và chuẩn hóa tên danh mục.
        private static string ValidateAndNormalizeName(
            string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException(
                    "Tên danh mục không được để trống.");
            }

            string normalizedName = categoryName.Trim();

            if (normalizedName.Length > 100)
            {
                throw new ArgumentException(
                    "Tên danh mục không được vượt quá 100 ký tự.");
            }

            return normalizedName;
        }

        // Chuẩn hóa loại danh mục thành đúng giá trị trong database.
        private static string NormalizeCategoryType(
            string categoryType)
        {
            if (string.IsNullOrWhiteSpace(categoryType))
            {
                throw new ArgumentException(
                    "Vui lòng chọn loại danh mục.");
            }

            if (categoryType.Equals(
                "Income",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Income";
            }

            if (categoryType.Equals(
                "Expense",
                StringComparison.OrdinalIgnoreCase))
            {
                return "Expense";
            }

            throw new ArgumentException(
                "Loại danh mục chỉ được là Income hoặc Expense.");
        }
        // Kiểm tra và chuẩn hóa mô tả.
        private static string? ValidateAndNormalizeDescription(
            string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return null;
            }

            string normalizedDescription = description.Trim();

            if (normalizedDescription.Length > 200)
            {
                throw new ArgumentException(
                    "Mô tả không được vượt quá 200 ký tự.");
            }

            return normalizedDescription;
        }
    }
}
