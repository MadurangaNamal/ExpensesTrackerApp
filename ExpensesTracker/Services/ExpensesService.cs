using ExpensesTracker.Data;
using ExpensesTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Services;

public class ExpensesService : IExpensesService
{
    private readonly ExpensesTrackerDBContext _dbContext;
    private readonly IUserService _userService;

    public ExpensesService(ExpensesTrackerDBContext dBContext, IUserService userService)
    {
        _dbContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync(int year, int month)
    {
        var user = await _userService.GetCurrentUserAsync();

        if (user == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var expenses = await _dbContext.Expenses
            .Where(e => e.Date.Year == year && e.Date.Month == month && e.UserId == user.UserId)
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
        var user = _userService.GetCurrentUserAsync();

        if (user == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var data = _dbContext.Expenses
            .Where(e => e.Date.Year == year && e.Date.Month == month && e.UserId == user.Result!.UserId)
            .GroupBy(e => e.Category)
            .Select(g => new ChartData
            {
                Category = g.Key,
                Total = g.Sum(e => e.Amount)
            });

        return data;
    }
}