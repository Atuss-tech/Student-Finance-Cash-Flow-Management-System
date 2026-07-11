using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TransactionDAO
    {
        private static readonly Lazy<TransactionDAO> instance = new Lazy<TransactionDAO>(() => new TransactionDAO());

        private TransactionDAO() { }

        public static TransactionDAO Instance => instance.Value;

        public async Task<List<FinanceTransaction>> GetTransactionsByMonthAsync(int userId, int month, int year)
        {
            using (var db = new StudentFinanceDbContext())
            {
                return await db.FinanceTransactions
                    .Include(t => t.Category)
                    .Where(t => t.UserId == userId && t.TransactionDate.Month == month && t.TransactionDate.Year == year)
                    .ToListAsync();
            }
        }

        public async Task<List<FinanceTransaction>> GetTransactionsByYearAsync(int userId, int year)
        {
            using (var db = new StudentFinanceDbContext())
            {
                return await db.FinanceTransactions
                    .Include(t => t.Category)
                    .Where(t => t.UserId == userId && t.TransactionDate.Year == year)
                    .ToListAsync();
            }
        }

        public async Task<FinanceTransaction?> GetTransactionByIdAsync(int id)
        {
            using (var db = new StudentFinanceDbContext())
            {
                return await db.FinanceTransactions.FindAsync(id);
            }
        }

        public async Task AddTransactionAsync(FinanceTransaction transaction)
        {
            using (var db = new StudentFinanceDbContext())
            {
                db.FinanceTransactions.Add(transaction);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateTransactionAsync(FinanceTransaction transaction)
        {
            using (var db = new StudentFinanceDbContext())
            {
                db.Entry(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteTransactionAsync(int id)
        {
            using (var db = new StudentFinanceDbContext())
            {
                var transaction = await db.FinanceTransactions.FindAsync(id);
                if (transaction != null)
                {
                    db.FinanceTransactions.Remove(transaction);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
