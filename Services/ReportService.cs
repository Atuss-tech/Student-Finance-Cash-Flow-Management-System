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
    }
}
