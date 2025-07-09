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

    public async Task<IActionResult> Index(int year = 0, int month = 0)
    {
        if (year == 0 || month == 0)
        {
            var now = DateTime.Now;
            year = now.Year;
            month = now.Month;
        }

        var expenses = await _expensesService.GetAll(year, month);
        return View(expenses);
    }

    [HttpGet]
    public async Task<IActionResult> GetExpensesTable(int year, int month)
    {
        var expenses = await _expensesService.GetAll(year, month);
        return PartialView("_ExpensesTablePartial", expenses);
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

    [HttpGet]
    public IActionResult GetChart(int year, int month)
    {
        var data = _expensesService.GetChartData(year, month);
        return Json(data);
    }
}
