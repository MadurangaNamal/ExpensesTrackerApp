using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers;

public class ExpensesController : Controller
{
    private readonly IExpensesService _expensesService;

    public ExpensesController(IExpensesService expensesService)
    {
        _expensesService = expensesService ?? throw new ArgumentNullException(nameof(expensesService));
    }

    public async Task<IActionResult> Index()
    {
        var expenses = await _expensesService.GetAll();
        return View(expenses);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Expense expense)
    {
        if (ModelState.IsValid)
        {
            await _expensesService.Add(expense);

            return RedirectToAction("Index");
        }

        return View(expense);
    }
}
