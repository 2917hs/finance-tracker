using System.Windows;
using FinanceTracker.Models;
using FinanceTracker.ViewModels;
using FinanceTracker.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Services;

internal class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<Transaction?> ShowAddTransactionDialogAsync()
    {
        var vm = _serviceProvider.GetRequiredService<AddTransactionViewModel>();
        var dialog = new AddTransactionDialog(vm)
        {
            Owner = Application.Current.MainWindow
        };

        var result = dialog.ShowDialog();
        return Task.FromResult(result == true ? vm.CreatedTransaction : null);
    }

    public Task<bool> ShowConfirmAsync(string title, string message)
    {
        var result = MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        return Task.FromResult(result == MessageBoxResult.Yes);
    }

    public Task ShowErrorAsync(string title, string message)
    {
        MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Error);

        return Task.CompletedTask;
    }
}