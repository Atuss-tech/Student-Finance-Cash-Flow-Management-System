using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using BusinessObjects.Models;
using BCrypt.Net;

namespace Services
{
    // Lớp dịch vụ xử lý logic nghiệp vụ (Business Logic) cho Người dùng
    public class UserService : IUserService
    {
        // Khai báo repository để giao tiếp với tầng dữ liệu
        private readonly IUserRepository userRepository;

        // Hàm khởi tạo, khởi tạo đối tượng UserRepository
        public UserService()
        {
            userRepository = new UserRepository();
        }

        // Xử lý logic đăng nhập: Kiểm tra email và mật khẩu
        public User? AuthenticateUser(string email, string password)
        {
            // Lấy thông tin user từ database theo email
            var user = userRepository.GetUserByEmail(email);
            
            // Nếu user tồn tại và mật khẩu (đã hash BCrypt) khớp thì trả về user
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            // Nếu sai hoặc không tồn tại thì trả về null (đăng nhập thất bại)
            return null;
        }

        // Xử lý logic đăng ký: Kiểm tra email trùng lặp và tạo tài khoản mới
        public bool RegisterUser(string fullName, string email, string password)
        {
            // Kiểm tra xem email đã được đăng ký chưa
            var user = userRepository.GetUserByEmail(email);
            if (user != null)
            {
                // Nếu email đã tồn tại, trả về false (đăng ký thất bại)
                return false;
            }

            // Tạo đối tượng User mới với mật khẩu được băm (hash) bằng BCrypt
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                IsActive = true
            };

            // Gọi repository để lưu vào database
            userRepository.AddUser(newUser);
            // Trả về true (đăng ký thành công)
            return true;
        }
    }
}
