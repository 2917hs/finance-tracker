using FinanceTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Services
{
    public interface ITransactionService
    {
        Task<IReadOnlyList<Transaction>> GetAllAsync();
        Task<IReadOnlyList<Transaction>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<Transaction> AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
        Task<IReadOnlyList<Category>> GetCategoriesAsync();
        Task<decimal> GetBalanceAsync();
    }
}
