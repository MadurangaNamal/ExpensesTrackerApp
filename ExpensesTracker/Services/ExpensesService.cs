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

    public async Task<IEnumerable<Expense>> GetAll(int year, int month)
    {
        var expenses = await _dbContext.Expenses
            .Where(e => e.Date.Year == year && e.Date.Month == month)
            .ToListAsync();
        return expenses;
    }

    public async Task Add(Expense expense)
    {
        _dbContext.Add(expense);
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