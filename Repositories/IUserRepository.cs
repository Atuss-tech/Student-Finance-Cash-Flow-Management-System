using BusinessObjects.Models;

namespace Repositories
{
    // Interface quy định các phương thức chuẩn cho UserRepository.
    public interface IUserRepository
    {
        // Lấy thông tin User theo địa chỉ email.
        User? GetUserByEmail(string email);

        // Thêm một User mới vào cơ sở dữ liệu.
        void AddUser(User user);
    }
}
