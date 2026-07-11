using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using BusinessObjects.Models;

namespace Repositories
{
    // Lớp Repository làm cầu nối giữa tầng Dịch vụ (Services) và tầng Dữ liệu (DataAccess)
    public class UserRepository : IUserRepository
    {
        // Gọi hàm GetUserByEmail từ UserDAO (DataAccess) để lấy thông tin
        public User? GetUserByEmail(string email)
        {
            return UserDAO.Instance.GetUserByEmail(email);
        }

        // Gọi hàm CreateUser từ UserDAO (DataAccess) để thêm người dùng mới
        public void AddUser(User user)
        {
            UserDAO.Instance.CreateUser(user);
        }
    }
}
