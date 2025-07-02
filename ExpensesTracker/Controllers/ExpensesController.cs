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
        var model = new Expense
        {
            Date = DateTime.Now
        };

        return View(model);
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

    public IActionResult GetChart()
    {
        var data = _expensesService.GetChartData();
        return Json(data);
    }
}
