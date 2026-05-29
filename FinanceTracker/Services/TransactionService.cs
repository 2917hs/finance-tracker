using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Services
{
    class TransactionService(IDbContextFactory<FinanceDbContext> dbContextFactory) : ITransactionService
    {

        public async Task DeleteAsync(int id)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            var transaction = await db.Transactions.FindAsync(id);
            if(transaction is not null)
            {
                db.Transactions.Remove(transaction);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Transaction>> GetAllAsync()
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            return await db.Transactions
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

        }

        public async Task<decimal> GetBalanceAsync()
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            var income = await db.Transactions
                .Where(t => t.Type == TransactionType.Income)
                .SumAsync(t => t.Amount);
            var expense = await db.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .SumAsync(t => t.Amount);

            return income - expense;
        }

        public async Task<IReadOnlyList<Transaction>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            return await db.Transactions
                .Include(t => t.Category)
                .Where(t => t.Date >= from && t.Date <= to)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            return await db.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();

            return await db.Transactions
                .Include(t => t.Category)
                .FirstAsync(t => t.Id == transaction.Id);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            db.Transactions.Update(transaction);
            await db.SaveChangesAsync();
        }
    }
}
