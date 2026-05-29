using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.Defaults;

namespace FinanceTracker.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly TransactionListViewModel _transactionListViewModel;

        [ObservableProperty]
        private ObservableObject _currentView;

        [ObservableProperty]
        private string _currentPageTitle = "Dashboard";

        [ObservableProperty]
        private bool _isNavigatioOpen = true;

        public MainViewModel(DashboardViewModel dashboardViewModel, TransactionListViewModel transactionListViewModel)
        {
            _dashboardViewModel = dashboardViewModel;
            _transactionListViewModel = transactionListViewModel;
            _currentView = dashboardViewModel;
        }

        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentView = _dashboardViewModel;
            CurrentPageTitle = "Dashbaord";
            _ = _dashboardViewModel.LoadAsync();
        }

        [RelayCommand]
        private async Task NavigateToTransactions()
        {
            CurrentView = _transactionListViewModel;
            CurrentPageTitle = "Transactions";
            _ = _transactionListViewModel.LoadAsync();
        }

        public async Task InitializeAsync()
        {
            await _dashboardViewModel.LoadAsync();
        }

        [RelayCommand]
        private void ToggleNavigation() => IsNavigatioOpen = !IsNavigatioOpen;
    }
}