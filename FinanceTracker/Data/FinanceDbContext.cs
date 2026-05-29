using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Data;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>()
            .Property(x => x.Amount)
            .HasColumnType("Text");

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Salary", ColorHex = "#4CAF50", AppliesTo = TransactionType.Income },
            new Category { Id = 2, Name = "Freelance", ColorHex = "#8BC34A", AppliesTo = TransactionType.Income },
            new Category { Id = 3, Name = "Food", ColorHex = "#FF5722", AppliesTo = TransactionType.Expense },
            new Category { Id = 4, Name = "Transport", ColorHex = "#2196F3", AppliesTo = TransactionType.Expense },
            new Category { Id = 5, Name = "Utilities", ColorHex = "#9C27B0", AppliesTo = TransactionType.Expense },
            new Category { Id = 6, Name = "Healthcare", ColorHex = "#F44336", AppliesTo = TransactionType.Expense },
            new Category { Id = 7, Name = "Shopping", ColorHex = "#FF9800", AppliesTo = TransactionType.Expense },
            new Category { Id = 8, Name = "Other", ColorHex = "#607D8B", AppliesTo = TransactionType.Expense }
        );
    }
}