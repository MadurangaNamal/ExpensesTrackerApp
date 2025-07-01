using ExpensesTracker.Models;

namespace ExpensesTracker.Services;

public interface IExpensesService
{
    Task<IEnumerable<Expense>> GetAll();
    Task Add(Expense expense);
    IQueryable GetChartData();
}
