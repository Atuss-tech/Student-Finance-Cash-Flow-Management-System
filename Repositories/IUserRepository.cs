using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories
{
    // Interface quy định các phương thức chuẩn cho UserRepository
    public interface IUserRepository
    {
        // Lấy thông tin User theo địa chỉ email
        User? GetUserByEmail(string email);
        
        // Thêm một User mới vào cơ sở dữ liệu
        void AddUser(User nguoiDung);
    }
}
