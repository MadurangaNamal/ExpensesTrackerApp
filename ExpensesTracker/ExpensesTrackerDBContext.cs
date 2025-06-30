using ExpensesTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker;

public class ExpensesTrackerDBContext : DbContext
{
    public ExpensesTrackerDBContext(DbContextOptions<ExpensesTrackerDBContext> options)
        : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>()
            .ToTable("Expenses");
        base.OnModelCreating(modelBuilder);
    }
}
