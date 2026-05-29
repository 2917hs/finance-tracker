using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanceTracker.Models;
using FinanceTracker.Services;

namespace FinanceTracker.ViewModels;

public partial class AddTransactionViewModel : ObservableValidator
{
    private readonly ITransactionService _transactionService;

    private IReadOnlyList<Category> _allCategories = [];

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0.01, 1_000_000, ErrorMessage = "Amount must be between 0.01 and 1,000,000")]
    private decimal _amount;

    [ObservableProperty] private DateTime _date = DateTime.Today;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(200, ErrorMessage = "Max 200 characters")]
    private string _description = string.Empty;

    [ObservableProperty] private bool _isSaving;

    [ObservableProperty] [NotifyDataErrorInfo] [Required(ErrorMessage = "Please select a category")]
    private Category? _selectedCategory;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(FilteredCategories))]
    private TransactionType _selectedType = TransactionType.Expense;

    public AddTransactionViewModel(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public Transaction? CreatedTransaction { get; private set; }
    public Action<bool>? RequestClose { get; set; }

    public IEnumerable<Category> FilteredCategories =>
        _allCategories.Where(c => c.AppliesTo == SelectedType);

    public IEnumerable<TransactionType> TransactionTypes =>
        Enum.GetValues<TransactionType>();

    public async Task InitializeAsync()
    {
        _allCategories = await _transactionService.GetCategoriesAsync();
        OnPropertyChanged(nameof(FilteredCategories));
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return;

        IsSaving = true;
        try
        {
            var transaction = new Transaction
            {
                Description = Description,
                Amount = Amount,
                Date = Date,
                Type = SelectedType,
                CategoryId = SelectedCategory!.Id
            };

            CreatedTransaction = await _transactionService.AddAsync(transaction);
            RequestClose?.Invoke(true);
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}