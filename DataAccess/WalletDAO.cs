using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class WalletDAO
    {
        private static readonly WalletDAO instance = new WalletDAO();

        // Không cho lớp bên ngoài tự tạo WalletDAO.
        private WalletDAO()
        {
        }

        public static WalletDAO Instance => instance;

        // Lấy toàn bộ ví của một người dùng.
        public List<Wallet> GetWalletsByUserId(int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.Wallets
                .AsNoTracking()
                .Where(wallet => wallet.UserId == userId)
                .OrderByDescending(wallet => wallet.IsActive)
                .ThenBy(wallet => wallet.WalletName)
                .ToList();
        }

        // Lấy một ví theo WalletId.
        // Kiểm tra cả UserId để không lấy nhầm ví của tài khoản khác.
        public Wallet? GetWalletById(
            int walletId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.Wallets
                .AsNoTracking()
                .FirstOrDefault(wallet =>
                    wallet.WalletId == walletId &&
                    wallet.UserId == userId);
        }

        // Kiểm tra tên ví đã tồn tại trong tài khoản hay chưa.
        public bool IsWalletNameExists(
            int userId,
            string walletName,
            int? ignoredWalletId = null)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            string normalizedName =
                walletName.Trim().ToLower();

            return db.Wallets.Any(wallet =>
                wallet.UserId == userId &&
                wallet.WalletName.ToLower() == normalizedName &&
                (
                    ignoredWalletId == null ||
                    wallet.WalletId != ignoredWalletId.Value
                ));
        }

        // Thêm một ví mới.
        public void AddWallet(Wallet wallet)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            db.Wallets.Add(wallet);
            db.SaveChanges();
        }

        // Cập nhật tên ví, loại ví và ghi chú.
        // Không sửa số dư tại đây.
        public void UpdateWallet(
            int walletId,
            int userId,
            string walletName,
            string walletType,
            string? note)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            Wallet? wallet = db.Wallets
                .FirstOrDefault(item =>
                    item.WalletId == walletId &&
                    item.UserId == userId);

            if (wallet == null)
            {
                return;
            }

            wallet.WalletName = walletName;
            wallet.WalletType = walletType;
            wallet.Note = note;

            db.SaveChanges();
        }

        

        // Xóa hẳn ví chưa có giao dịch.
        public void DeleteWallet(
            int walletId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            Wallet? wallet = db.Wallets
                .FirstOrDefault(item =>
                    item.WalletId == walletId &&
                    item.UserId == userId);

            if (wallet == null)
            {
                return;
            }

            db.Wallets.Remove(wallet);
            db.SaveChanges();
        }

        // Chuyển ví sang trạng thái ngừng sử dụng.
        public void DeactivateWallet(
            int walletId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            Wallet? wallet = db.Wallets
                .FirstOrDefault(item =>
                    item.WalletId == walletId &&
                    item.UserId == userId);

            if (wallet == null)
            {
                return;
            }

            wallet.IsActive = false;
            db.SaveChanges();
        }
        // Kiểm tra ví đã được dùng trong giao dịch chưa.
        public bool HasTransactions(
            int walletId,
            int userId)
        {
            using StudentFinanceDbContext db =
                new StudentFinanceDbContext();

            return db.FinanceTransactions.Any(transaction =>
                transaction.WalletId == walletId &&
                transaction.UserId == userId);
        }

    }
}
