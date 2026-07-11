using BusinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class WalletRepository : IWalletRepository
    {
        //Lấy danh sách ví của người dùng theo userId
        public List<Wallet> GetWalletsByUserId(int userId)
        {
            return WalletDAO.Instance
                .GetWalletsByUserId(userId);
        }
        //Lấy ví theo walletId và userId
        public Wallet? GetWalletById(
            int walletId,
            int userId)
        {
            return WalletDAO.Instance
                .GetWalletById(walletId, userId);
        }
        //Kiểm tra ví có tồn tại theo tên ví, userId và walletId bị bỏ qua
        public bool IsWalletNameExists(
            int userId,
            string walletName,
            int? ignoredWalletId = null)
        {
            return WalletDAO.Instance
                .IsWalletNameExists(
                    userId,
                    walletName,
                    ignoredWalletId);
        }

        public void AddWallet(Wallet wallet)
        {
            WalletDAO.Instance.AddWallet(wallet);
        }

        public void UpdateWallet(
            int walletId,
            int userId,
            string walletName,
            string walletType,
            string? note)
        {
            WalletDAO.Instance.UpdateWallet(
                walletId,
                userId,
                walletName,
                walletType,
                note);
        }

        public void DeleteWallet(
           int walletId,
           int userId)
        {
            WalletDAO.Instance
                .DeleteWallet(walletId, userId);
        }

        public void DeactivateWallet(
            int walletId,
            int userId)
        {
            WalletDAO.Instance
                .DeactivateWallet(walletId, userId);
        }

        public bool HasTransactions(int walletId, int userId)
        {
            return WalletDAO.Instance.HasTransactions(
                walletId,
                userId);
        }


    }
}
