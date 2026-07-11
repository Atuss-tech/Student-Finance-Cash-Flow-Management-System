using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Service xử lý các nghiệp vụ liên quan đến giao dịch tài chính (Thu/Chi).
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Lấy danh sách giao dịch trong một tháng cụ thể của người dùng.
        /// </summary>
        public async Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year)
        {
            return await _transactionRepository.GetTransactionsByMonthAsync(userId, month, year);
        }

        /// <summary>
        /// Lấy danh sách toàn bộ giao dịch trong một năm của người dùng.
        /// </summary>
        public async Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year)
        {
            return await _transactionRepository.GetTransactionsByYearAsync(userId, year);
        }

        /// <summary>
        /// Lấy thông tin chi tiết một giao dịch theo ID.
        /// </summary>
        public async Task<FinanceTransaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }

        /// <summary>
        /// Thêm mới một giao dịch. Sẽ kiểm tra tính hợp lệ trước khi lưu.
        /// </summary>
        public async Task AddTransactionAsync(FinanceTransaction transaction)
        {
            ValidateTransaction(transaction);
            await _transactionRepository.AddTransactionAsync(transaction);
        }

        /// <summary>
        /// Cập nhật một giao dịch đã có. Sẽ kiểm tra tính hợp lệ trước khi lưu.
        /// </summary>
        public async Task UpdateTransactionAsync(FinanceTransaction transaction)
        {
            ValidateTransaction(transaction);
            await _transactionRepository.UpdateTransactionAsync(transaction);
        }

        /// <summary>
        /// Xóa một giao dịch khỏi hệ thống.
        /// </summary>
        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteTransactionAsync(id);
        }

        /// <summary>
        /// Kiểm tra các quy tắc nghiệp vụ cho giao dịch (Số tiền dương, ngày không được ở tương lai, loại giao dịch hợp lệ).
        /// </summary>
        private void ValidateTransaction(FinanceTransaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new Exception("Số tiền giao dịch phải lớn hơn 0.");

            if (transaction.TransactionDate > DateOnly.FromDateTime(DateTime.Now))
                throw new Exception("Ngày giao dịch không được lớn hơn ngày hiện tại.");

            if (transaction.TransactionType != "Income" && transaction.TransactionType != "Expense")
                throw new Exception("Loại giao dịch phải là Income hoặc Expense.");
        }
    }
}
