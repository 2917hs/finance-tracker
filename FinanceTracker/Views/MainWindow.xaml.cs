using FinanceTracker.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace FinanceTracker.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        Loaded += async (_, _) => await viewModel.InitializeAsync();
    }
}