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

    public async Task<IEnumerable<Expense>> GetAll()
    {
        var expenses = await _dbContext.Expenses.ToListAsync();
        return expenses;
    }

    public async Task Add(Expense expense)
    {
        _dbContext.Add(expense);
        await _dbContext.SaveChangesAsync();
    }

    public IQueryable GetChartData()
    {
        var data = _dbContext.Expenses
            .GroupBy(e => e.Category)
            .Select(g => new ChartData
            {
                Category = g.Key,
                Total = g.Sum(e => e.Amount)
            });

        return data;
    }
}