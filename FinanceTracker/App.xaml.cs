using System.Windows;
using FinanceTracker.Data;
using FinanceTracker.Services;
using FinanceTracker.ViewModels;
using FinanceTracker.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinanceTracker;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .Build();

        System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level =
            System.Diagnostics.SourceLevels.Warning;

        DispatcherUnhandledException += (_, e) =>
        {
            MessageBox.Show(
                $"An unexpected error occurred:\n\n{e.Exception.Message}",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (_, e) => e.SetObserved();

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            MessageBox.Show($"Fatal error: {e.ExceptionObject}", "Fatal Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContextFactory<FinanceDbContext>(options =>
            options.UseSqlite("Data Source=finance.db"));

        services.AddTransient<IDialogService, DialogService>();
        services.AddSingleton<ITransactionService, TransactionService>();

        services.AddTransient<DashboardViewModel>();
        services.AddTransient<TransactionListViewModel>();
        services.AddTransient<AddTransactionViewModel>();

        services.AddSingleton<MainViewModel>(sp => new MainViewModel(
            sp.GetRequiredService<DashboardViewModel>(),
            sp.GetRequiredService<TransactionListViewModel>()
        ));

        services.AddSingleton<MainWindow>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var factory = _host.Services.GetRequiredService<IDbContextFactory<FinanceDbContext>>();
        await using var db = await factory.CreateDbContextAsync();
        await db.Database.EnsureCreatedAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}