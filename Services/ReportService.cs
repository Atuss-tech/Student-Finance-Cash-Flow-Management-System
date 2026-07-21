using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Service xử lý các nghiệp vụ thống kê, báo cáo tài chính.
    /// Trả về các kiểu dữ liệu nguyên thủy (ValueTuple, Dictionary) thay vì tạo DTO.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly ITransactionRepository _transactionRepository;

        public ReportService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Lấy báo cáo tổng quan (Tổng thu, Tổng chi, Số dư) cho một tháng cụ thể.
        /// </summary>
        public async Task<(decimal TotalIncome, decimal TotalExpense, decimal Balance, int Month, int Year)> GetMonthlyReportAsync(int userId, int month, int year)
        {
            var transactions = await _transactionRepository.GetTransactionsByMonthAsync(userId, month, year);
            
            decimal totalIncome = transactions.Where(t => t.TransactionType == "Income").Sum(t => t.Amount);
            decimal totalExpense = transactions.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount);
            decimal netCashFlow = totalIncome - totalExpense;

            return (
                totalIncome,
                totalExpense,
                netCashFlow, // Using NetCashFlow as Balance for simplicity
                month,
                year
            );
        }

        /// <summary>
        /// Thống kê chi phí theo từng danh mục (Category) trong tháng.
        /// Sử dụng Dictionary để map Tên danh mục -> Tổng tiền.
        /// </summary>
        public async Task<Dictionary<string, decimal>> GetExpenseByCategoryAsync(int userId, int month, int year)
        {
            var transactions = await _transactionRepository.GetTransactionsByMonthAsync(userId, month, year);
            
            return transactions
                .Where(t => t.TransactionType == "Expense")
                .GroupBy(t => t.Category?.CategoryName ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }

        /// <summary>
        /// Phân tích xu hướng dòng tiền (Thu - Chi) theo từng tháng trong năm.
        /// Trả về Dictionary map "Tháng X" -> Dòng tiền.
        /// </summary>
        public async Task<Dictionary<string, decimal>> GetCashFlowTrendAsync(int userId, int year)
        {
            var transactions = await _transactionRepository.GetTransactionsByYearAsync(userId, year);
            
            return transactions
                .GroupBy(t => t.TransactionDate.Month)
                .ToDictionary(
                    g => $"Tháng {g.Key}",
                    g => g.Where(t => t.TransactionType == "Income").Sum(t => t.Amount) 
                       - g.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount)
                );
        }

        /// <summary>
        /// Nhóm chi tiêu theo 4 nhóm lớn dựa trên keyword trong tên danh mục:
        ///   - Nhu cầu thiết yếu: ăn, uống, nhà, ở, điện, nước, y tế, thuốc, đi lại, xăng, sửa xe, internet, điện thoại
        ///   - Sở thích cá nhân: giải trí, cafe, cà phê, mua sắm, du lịch, thể thao, mỹ phẩm, quà tặng, thời trang
        ///   - Tích lũy: tiết kiệm, đầu tư, bảo hiểm, tích lũy
        ///   - Tương lai: học, khóa học, sách, phát triển, ngoại ngữ, kỹ năng
        /// </summary>
        public async Task<Dictionary<string, decimal>> GetExpenseBySpendingGroupAsync(int userId, int month, int year)
        {
            var transactions = await _transactionRepository.GetTransactionsByMonthAsync(userId, month, year);

            var expenses = transactions.Where(t => t.TransactionType == "Expense").ToList();

            // Keyword maps: lowercase Vietnamese keywords -> group name
            var essentialKeywords = new[]
            {
                "ăn", "uống", "nhà", "ở", "thuê", "điện", "nước", "y tế", "thuốc",
                "khám", "bệnh", "đi lại", "xăng", "sửa xe", "xe", "internet",
                "điện thoại", "sinh hoạt", "chợ", "tạp hóa", "thực phẩm"
            };
            var personalKeywords = new[]
            {
                "giải trí", "cafe", "cà phê", "mua sắm", "du lịch", "thể thao",
                "mỹ phẩm", "làm đẹp", "quà tặng", "quà", "thời trang", "quần áo",
                "phim", "game", "nhà hàng", "karaoke", "vui chơi", "đồ dùng cá nhân"
            };
            var savingsKeywords = new[]
            {
                "tiết kiệm", "đầu tư", "bảo hiểm", "tích lũy", "quỹ", "dự phòng"
            };
            var futureKeywords = new[]
            {
                "học", "khóa học", "sách", "phát triển", "ngoại ngữ", "kỹ năng",
                "đào tạo", "chứng chỉ", "giáo dục", "học phí", "trường", "online"
            };

            var result = new Dictionary<string, decimal>
            {
                ["🏠 Nhu cầu thiết yếu"] = 0,
                ["🎮 Sở thích cá nhân"] = 0,
                ["💰 Tích lũy"] = 0,
                ["🎓 Tương lai"] = 0,
            };

            foreach (var t in expenses)
            {
                var catName = (t.Category?.CategoryName ?? "").ToLowerInvariant();
                var desc = (t.Description ?? "").ToLowerInvariant();
                var combined = catName + " " + desc;

                if (futureKeywords.Any(k => combined.Contains(k)))
                    result["🎓 Tương lai"] += t.Amount;
                else if (savingsKeywords.Any(k => combined.Contains(k)))
                    result["💰 Tích lũy"] += t.Amount;
                else if (personalKeywords.Any(k => combined.Contains(k)))
                    result["🎮 Sở thích cá nhân"] += t.Amount;
                else if (essentialKeywords.Any(k => combined.Contains(k)))
                    result["🏠 Nhu cầu thiết yếu"] += t.Amount;
                else
                    result["🏠 Nhu cầu thiết yếu"] += t.Amount; // fallback
            }

            // Chỉ trả về những nhóm có chi tiêu > 0
            return result.Where(kvp => kvp.Value > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
