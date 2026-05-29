namespace FinanceTracker.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ColorHex { get; set; } = "#607D8B";

    public TransactionType AppliesTo { get; set; }
}