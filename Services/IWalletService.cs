using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    //Các chức năng nghiệp vụ mà giao diện được phép sử dụng 
    public interface IWalletService
    {
        List<Wallet> GetAllWalletsByUser(int userId);

        void CreateNewWallet(
            int userId,
            string walletName,
            string walletType,
            decimal initialBalance,
            string? note);

        void UpdateWalletInfo(
            int userId,
            int walletId,
            string walletName,
            string walletType,
            string? note);

        // Trả về true nếu ví được chuyển sang ngừng sử dụng.
        // Trả về false nếu ví được xóa hẳn.
        bool RemoveOrDeactivateWallet(
            int userId,
            int walletId);
    }
}
