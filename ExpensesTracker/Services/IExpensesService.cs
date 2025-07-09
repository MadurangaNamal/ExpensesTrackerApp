using ExpensesTracker.Models;

namespace ExpensesTracker.Services;

public interface IExpensesService
{
    Task<IEnumerable<Expense>> GetAll(int year, int month);
    Task Add(Expense expense);
    IQueryable GetChartData(int year, int month);
}
