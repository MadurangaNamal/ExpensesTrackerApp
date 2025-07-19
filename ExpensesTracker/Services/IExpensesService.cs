using ExpensesTracker.Models;

namespace ExpensesTracker.Services;

public interface IExpensesService
{
    Task<IEnumerable<Expense>> GetAllExpensesAsync(int year, int month);
    Task AddExpenseItemAsync(Expense expense);
    Task UpdateExpenseItemAsync(int itemId, Expense expense);
    Task DeleteExpenseItemAsync(int itemId);
    IQueryable GetChartData(int year, int month);
}
