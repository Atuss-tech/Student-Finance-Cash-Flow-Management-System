using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ liên quan đến Ngân sách (Budgets) và tiến độ chi tiêu.
    /// </summary>
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BudgetService(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository)
        {
            _budgetRepository = budgetRepository;
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Tạo mới một ngân sách. Đảm bảo số tiền > 0 và không bị trùng danh mục trong tháng.
        /// </summary>
        public async Task AddBudgetAsync(Budget budget)
        {
            // Business Rule: BR11 - Ngân sách chỉ áp dụng cho danh mục Expense
            // Business Rule: BR11 - Số tiền ngân sách phải lớn hơn 0
            if (budget.AmountLimit <= 0)
            {
                throw new Exception("Ngân sách phải lớn hơn 0.");
            }

            // Validate: Không trùng ngân sách trong tháng
            var existingBudget = await _budgetRepository.GetBudgetAsync(budget.UserId, budget.CategoryId, budget.Month, budget.Year);
            if (existingBudget != null)
            {
                throw new Exception("Danh mục này đã được thiết lập ngân sách trong tháng.");
            }

            await _budgetRepository.AddBudgetAsync(budget);
        }

        /// <summary>
        /// Cập nhật một ngân sách. Đảm bảo số tiền > 0.
        /// </summary>
        public async Task UpdateBudgetAsync(Budget budget)
        {
            if (budget.AmountLimit <= 0)
            {
                throw new Exception("Ngân sách phải lớn hơn 0.");
            }

            await _budgetRepository.UpdateBudgetAsync(budget);
        }

        /// <summary>
        /// Xóa một ngân sách.
        /// </summary>
        public async Task DeleteBudgetAsync(int budgetId)
        {
            await _budgetRepository.DeleteBudgetAsync(budgetId);
        }

        /// <summary>
        /// Lấy danh sách ngân sách thô của người dùng trong một tháng cụ thể.
        /// </summary>
        public async Task<List<Budget>> GetBudgetsAsync(int userId, int month, int year)
        {
            return await _budgetRepository.GetBudgetsAsync(userId, month, year);
        }

        /// <summary>
        /// Tính toán tiến độ chi tiêu thực tế so với ngân sách.
        /// Phối hợp dữ liệu từ BudgetRepository và TransactionRepository để ra % tiêu thụ.
        /// Trả về danh sách dạng ValueTuple.
        /// </summary>
        public async Task<List<(int BudgetId, int CategoryId, string CategoryName, decimal AmountLimit, decimal SpentAmount, double UsagePercentage, string AlertStatus)>> GetBudgetProgressesAsync(int userId, int month, int year)
        {
            var budgets = await _budgetRepository.GetBudgetsAsync(userId, month, year);
            var transactions = await _transactionRepository.GetTransactionsByMonthAsync(userId, month, year);
            var expenses = transactions.Where(t => t.TransactionType == "Expense").ToList();

            var progressList = new List<(int, int, string, decimal, decimal, double, string)>();

            foreach (var budget in budgets)
            {
                var spent = expenses.Where(t => t.CategoryId == budget.CategoryId).Sum(t => t.Amount);
                var percentage = budget.AmountLimit > 0 ? (double)(spent / budget.AmountLimit) * 100 : 0;

                string status = "Normal";
                if (percentage >= 100) status = "Exceeded";
                else if (percentage >= 80) status = "Warning";

                progressList.Add((
                    budget.BudgetId,
                    budget.CategoryId,
                    budget.Category?.CategoryName ?? "Unknown",
                    budget.AmountLimit,
                    spent,
                    percentage,
                    status
                ));
            }

            return progressList;
        }
    }
}
