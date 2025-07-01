using ExpensesTracker.Services;

namespace ExpensesTracker.Controllers;

public class ExpensesController
{
    private readonly IExpensesService _expensesService;

    public ExpensesController(IExpensesService expensesService)
    {
        _expensesService = expensesService ?? throw new ArgumentNullException(nameof(expensesService));
    }


}
