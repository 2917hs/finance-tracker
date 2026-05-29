using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanceTracker.Models;
using FinanceTracker.Services;
using Transaction = FinanceTracker.Models.Transaction;

namespace FinanceTracker.ViewModels;

public partial class TransactionListViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly ITransactionService _transactionService;

    [ObservableProperty] private decimal _balance;

    [ObservableProperty] private bool _isLoading;


    [ObservableProperty] private string _searchText;

    [ObservableProperty] private Transaction? _selectedTransaction;

    [ObservableProperty] private decimal _totalExpenses;

    [ObservableProperty] private decimal _totalIncome;

    public TransactionListViewModel(ITransactionService transactionService, IDialogService dialogService)
    {
        _transactionService = transactionService;
        _dialogService = dialogService;
    }

    public ObservableCollection<Transaction> Transactions { get; } = [];

    public async Task LoadAsync()
    {
        IsLoading = true;
        try
        {
            var transactions = await _transactionService.GetAllAsync();
            Transactions.Clear();

            foreach (var tx in transactions)
                Transactions.Add(tx);

            RecalculateSummary();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void RecalculateSummary()
    {
        TotalIncome = Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        TotalExpenses = Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
        Balance = TotalIncome - TotalExpenses;
    }

    [RelayCommand]
    private async Task AddTransactionAsync()
    {
        var created = await _dialogService.ShowAddTransactionDialogAsync();
        if (created is not null)
        {
            Transactions.Insert(0, created);
            RecalculateSummary();
        }
    }

    [RelayCommand]
    private async Task DeleteTransactionAsync(Transaction? transaction)
    {
        if (transaction is null) return;

        var confirmed = await _dialogService.ShowConfirmAsync(
            "Delete Transaction",
            $"Delete '{transaction.Description}'? This cannot be undone.");

        if (!confirmed) return;

        await _transactionService.DeleteAsync(transaction.Id);
        Transactions.Remove(transaction);
        RecalculateSummary();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadAsync();
    }
}