using FinanceTracker.Models;

namespace FinanceTracker.Services;

public interface IDialogService
{
    Task<Transaction?> ShowAddTransactionDialogAsync();

    Task ShowErrorAsync(string title, string message);

    Task<bool> ShowConfirmAsync(string title, string message);
}