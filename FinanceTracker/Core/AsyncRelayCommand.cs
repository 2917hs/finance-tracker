using System.Windows.Input;

namespace FinanceTracker.Core;

public class AsyncRelayCommand : ICommand
{
    private readonly Func<object?, Task> _execute;
    private readonly Func<object?, bool>? _canExecute;
    private bool _isExecuting;

    public bool IsExecuting
    {
        get => _isExecuting;
        private set
        {
            _isExecuting = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? CanExecuteChanged;

    public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        : this(_ => execute(), canExecute is null ? null : _ => canExecute()) { }

    public AsyncRelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) =>
        !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);

    public async void Execute(object? parameter)
    {
        IsExecuting = true;
        try
        {
            await _execute(parameter);
        }
        finally
        {
            IsExecuting = false;
        }
    }
}