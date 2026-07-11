using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BusinessObjects.Models;

namespace Services
{
    public interface IUserService
    {
        // Hàm xác thực người dùng khi đăng nhập
        User? AuthenticateUser(string email, string password);
        bool RegisterUser(string fullName, string email, string password);
    }
}
