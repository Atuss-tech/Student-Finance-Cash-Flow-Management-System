using BusinessObjects.Models;

namespace Services
{
    public interface IUserService
    {
        // Hàm xác thực người dùng khi đăng nhập.
        User? AuthenticateUser(string email, string password);

        // Hàm đăng ký tài khoản mới.
        bool RegisterUser(string fullName, string email, string password);
    }
}
