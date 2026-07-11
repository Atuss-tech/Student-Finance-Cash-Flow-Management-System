using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    //Quy định các chức năng Repository của Wallet
    public interface IWalletRepository
    {
        List<Wallet> GetWalletsByUserId(int userId);

        Wallet? GetWalletById(
            int walletId,
            int userId);

        bool IsWalletNameExists(
            int userId,
            string walletName,
            int? ignoredWalletId = null);

        void AddWallet(Wallet wallet);

        void UpdateWallet(
            int walletId,
            int userId,
            string walletName,
            string walletType,
            string? note);

        bool HasTransactions(
            int walletId,
            int userId);

        void DeleteWallet(
            int walletId,
            int userId);

        void DeactivateWallet(
            int walletId,
            int userId);

    }
}
