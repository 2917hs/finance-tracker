using FinanceTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Services
{
    public interface IDialogService
    {
        Task<Transaction?> ShowAddTransactionDialogAsync();

        Task ShowErrorAsync(string title, string message);

        Task<bool> ShowConfirmAsync(string title, string message);
    }
}
