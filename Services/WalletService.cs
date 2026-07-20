using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    //Xử lý các quy tắc nghệp vụ của ví 
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository walletRepository;

        // Các loại ví được database cho phép.
        private readonly string[] allowedWalletTypes =
        {
            "Cash",
            "Bank",
            "EWallet"
        };

        public WalletService()
        {
            walletRepository = new WalletRepository();
        }
        // Thêm ví mới.
        public void CreateNewWallet(
            int userId,
            string walletName,
            string walletType,
            decimal initialBalance,
            string? note)
        {
            CheckUserId(userId);

            CheckWalletInput(
                walletName,
                walletType);

            // Nghiệp vụ: số dư ban đầu không được âm.
            if (initialBalance < 0)
            {
                throw new ArgumentException(
                    "Số dư ban đầu không được nhỏ hơn 0.");
            }

            string normalizedName = walletName.Trim();

            // Nghiệp vụ: một người dùng không có hai ví cùng tên.
            bool nameExists =
                walletRepository.IsWalletNameExists(
                    userId,
                    normalizedName);

            if (nameExists)
            {
                throw new InvalidOperationException(
                    "Tên ví đã tồn tại trong tài khoản.");
            }

            Wallet wallet = new Wallet
            {
                UserId = userId,
                WalletName = normalizedName,
                WalletType = walletType,
                InitialBalance = initialBalance,

                // Khi mới tạo, số dư hiện tại bằng số dư ban đầu.
                Balance = initialBalance,

                Note = GetNote(note),
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            walletRepository.AddWallet(wallet);
        }

        // Xóa hoặc ngừng sử dụng ví.
        public bool RemoveOrDeactivateWallet(
            int userId,
            int walletId)
        {
            CheckUserId(userId);

            Wallet? wallet =
                walletRepository.GetWalletById(
                    walletId,
                    userId);

            if (wallet == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy ví cần xóa.");
            }

            bool hasTransactions =
                walletRepository.HasTransactions(
                    walletId,
                    userId);

            // Ví có giao dịch phải được giữ lại để bảo toàn lịch sử.
            if (hasTransactions)
            {
                walletRepository.DeactivateWallet(
                    walletId,
                    userId);

                return true;
            }

            // Ví chưa có giao dịch có thể xóa hẳn.
            walletRepository.DeleteWallet(
                walletId,
                userId);

            return false;
        }

        // Lấy các ví thuộc người dùng hiện tại.
        public List<Wallet> GetAllWalletsByUser(int userId)
        {
            CheckUserId(userId);

            return walletRepository
                .GetWalletsByUserId(userId);
        }


        // Sửa thông tin ví.
        public void UpdateWalletInfo(
            int userId,
            int walletId,
            string walletName,
            string walletType,
            string? note)
        {
            CheckUserId(userId);

            CheckWalletInput(
                walletName,
                walletType);

            Wallet? wallet =
                walletRepository.GetWalletById(
                    walletId,
                    userId);

            if (wallet == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy ví cần sửa.");
            }

            string normalizedName = walletName.Trim();

            // Bỏ qua chính ví đang sửa khi kiểm tra trùng tên.
            bool nameExists =
                walletRepository.IsWalletNameExists(
                    userId,
                    normalizedName,
                    walletId);

            if (nameExists)
            {
                throw new InvalidOperationException(
                    "Tên ví đã tồn tại trong tài khoản.");
            }

            walletRepository.UpdateWallet(
                walletId,
                userId,
                normalizedName,
                walletType,
                GetNote(note));
        }
        // Kiểm tra UserId.
        private static void CheckUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException(
                    "Người dùng không hợp lệ.");
            }
        }

        // Kiểm tra tên ví và loại ví.
        private void CheckWalletInput(
            string walletName,
            string walletType)
        {
            if (string.IsNullOrWhiteSpace(walletName))
            {
                throw new ArgumentException(
                    "Tên ví không được để trống.");
            }

            if (walletName.Trim().Length > 100)
            {
                throw new ArgumentException(
                    "Tên ví không được vượt quá 100 ký tự.");
            }

            if (string.IsNullOrWhiteSpace(walletType) ||
                !allowedWalletTypes.Contains(walletType))
            {
                throw new ArgumentException(
                    "Loại ví không hợp lệ.");
            }
        }
        // Chuẩn hóa ghi chú trước khi lưu.
        private static string? GetNote(string? note)
        {
            if (string.IsNullOrWhiteSpace(note))
            {
                return null;
            }

            return note.Trim();
        }


    }
}
