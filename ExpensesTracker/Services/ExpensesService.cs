using ExpensesTracker.Data;
using ExpensesTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Services;

public class ExpensesService : IExpensesService
{
    private readonly ExpensesTrackerDBContext _dbContext;

    public ExpensesService(ExpensesTrackerDBContext dBContext)
    {
        _dbContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync(int year, int month)
    {
        var expenses = await _dbContext.Expenses
            .Where(e => e.Date.Year == year && e.Date.Month == month)
            .AsNoTracking()
            .ToListAsync();
        return expenses;
    }

    public async Task<Expense> GetExpenseItemAsync(int itemId)
    {
        var expenseItem = await _dbContext.Expenses.FindAsync(itemId);

        if (expenseItem == null)
            throw new KeyNotFoundException($"Expense with ID {itemId} not found.");

        return expenseItem;
    }

    public async Task AddExpenseItemAsync(Expense expense)
    {
        _dbContext.Add(expense);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateExpenseItemAsync(int itemId, Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense, nameof(expense));
        var expenseItem = await _dbContext.Expenses.FindAsync(itemId);

        if (expenseItem == null)
            throw new KeyNotFoundException($"Expense with ID {itemId} not found.");

        expenseItem.Description = expense.Description;
        expenseItem.Amount = expense.Amount;
        expenseItem.Category = expense.Category;
        expenseItem.Date = expense.Date;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteExpenseItemAsync(int itemId)
    {
        var expenseItem = await _dbContext.Expenses.FindAsync(itemId);
        if (expenseItem == null)
            throw new KeyNotFoundException($"Expense with ID {itemId} not found.");

        _dbContext.Expenses.Remove(expenseItem);
        await _dbContext.SaveChangesAsync();
    }

    public IQueryable GetChartData(int year, int month)
    {
        var data = _dbContext.Expenses
            .Where(e => e.Date.Year == year && e.Date.Month == month)
            .GroupBy(e => e.Category)
            .Select(g => new ChartData
            {
                Category = g.Key,
                Total = g.Sum(e => e.Amount)
            });

        return data;
    }
}