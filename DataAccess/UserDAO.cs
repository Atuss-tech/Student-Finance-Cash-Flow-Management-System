using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace DataAccess
{
    // Lớp thao tác cơ sở dữ liệu (Data Access Object) cho bảng User.
    // Dùng mô hình Singleton để chỉ khởi tạo 1 lần duy nhất trong toàn hệ thống.
    public class UserDAO
    {
        // Khởi tạo trễ (Lazy) cho Singleton, tự động thread-safe (an toàn đa luồng)
        private static readonly Lazy<UserDAO> instance = new Lazy<UserDAO>(() => new UserDAO());

        // Constructor private để ngăn không cho tạo instance mới từ bên ngoài bằng lệnh 'new'
        private UserDAO() { }

        // Gọi UserDAO.Instance để lấy đối tượng duy nhất
        public static UserDAO Instance => instance.Value;

        // Tìm kiếm người dùng dựa trên địa chỉ email.
        // Trả về đối tượng User nếu tìm thấy, hoặc null nếu không tồn tại.
        public User? GetUserByEmail(string email)
        {
            // Mở kết nối tới cơ sở dữ liệu
            using (var db = new StudentFinanceDbContext())
            {
                // Lấy User đầu tiên khớp email, dùng Equals cho chuỗi
                return db.Users.FirstOrDefault(user => user.Email.Equals(email));
            }
        }

        // Thêm một người dùng mới (đối tượng user) vào cơ sở dữ liệu.
        public void CreateUser(User user)
        {
            // Mở kết nối tới cơ sở dữ liệu
            using (var db = new StudentFinanceDbContext())
            {
                // Thêm vào DbSet
                db.Users.Add(user);
                // Lưu thay đổi xuống Database thực tế
                db.SaveChanges();
            }
        }
    }
}
