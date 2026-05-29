using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceTracker.Models;
using FinanceTracker.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace FinanceTracker.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;

    [ObservableProperty] private Axis[] _expenseAxis = [];

    [ObservableProperty] private ISeries[] _expenseSeries = [];

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private decimal _monthlyExpenses;

    [ObservableProperty] private decimal _monthlyIncome;

    [ObservableProperty] private decimal _totalBalance;

    public DashboardViewModel(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public ObservableCollection<Transaction> RecentTransactions { get; } = [];

    public async Task LoadAsync()
    {
        IsLoading = true;
        try
        {
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var allTransactions = await _transactionService.GetAllAsync();
            var monthlyTransaction = allTransactions.Where(t => t.Date >= monthStart).ToList();

            TotalBalance = await _transactionService.GetBalanceAsync();
            MonthlyIncome = monthlyTransaction.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            MonthlyExpenses = monthlyTransaction.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

            RecentTransactions.Clear();
            foreach (var tx in allTransactions.Take(5)) RecentTransactions.Add(tx);

            BuildExpenseChart(allTransactions.Where(t => t.Type == TransactionType.Expense).ToList());
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void BuildExpenseChart(List<Transaction> transactions)
    {
        var grouped = transactions
            .GroupBy(t => t.Category.Name)
            .Select(g => new
            {
                Category = g.Key,
                TotalBalance = g.Sum(t => t.Amount),
                Color = g.First().Category.ColorHex
            })
            .OrderByDescending(x => x.TotalBalance)
            .ToList();

        ExpenseSeries = grouped.Select(g => new PieSeries<decimal>
        {
            Name = g.Category,
            Values = new[] { g.TotalBalance },
            Fill = new SolidColorPaint(SKColor.Parse(g.Color))
        }).Cast<ISeries>().ToArray();
    }
}