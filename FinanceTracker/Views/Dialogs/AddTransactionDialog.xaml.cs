using System.Windows;
using FinanceTracker.ViewModels;

namespace FinanceTracker.Views.Dialogs;

public partial class AddTransactionDialog : Window
{
    public AddTransactionDialog(AddTransactionViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.RequestClose = result => DialogResult = result;
        Loaded += async (_, _) => await viewModel.InitializeAsync();
    }
}