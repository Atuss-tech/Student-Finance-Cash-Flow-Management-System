using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Repositories;
using Services;

class Program
{
    static async Task Main(string[] args)
    {
        var budgetService = new BudgetService(new BudgetRepository(), new TransactionRepository());
        var progresses = await budgetService.GetBudgetProgressesAsync(1, DateTime.Now.Month, DateTime.Now.Year);
        Console.WriteLine($"Found {progresses.Count} budgets.");
        foreach (var p in progresses)
        {
            Console.WriteLine($"Category: {p.CategoryName}, Limit: {p.AmountLimit}, Spent: {p.SpentAmount}");
        }

        using var db = new StudentFinanceDbContext();
        var txs = await db.FinanceTransactions
            .Where(t => t.UserId == 1 && t.TransactionDate.Month == DateTime.Now.Month && t.TransactionDate.Year == DateTime.Now.Year)
            .ToListAsync();
        
        Console.WriteLine($"Found {txs.Count} transactions this month.");
        foreach (var t in txs)
        {
            Console.WriteLine($"Tx: {t.Description}, Type: {t.TransactionType}, Amount: {t.Amount}, CategoryId: {t.CategoryId}");
        }
    }
}
