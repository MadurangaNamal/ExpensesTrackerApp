using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers;

[Authorize]
public class ExpensesController : Controller
{
    private readonly IExpensesService _expensesService;
    private readonly IUserService _userService;

    public ExpensesController(IExpensesService expensesService, IUserService userService)
    {
        _expensesService = expensesService ?? throw new ArgumentNullException(nameof(expensesService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public async Task<IActionResult> Index(int year = 0, int month = 0)
    {
        if (year == 0 || month == 0)
        {
            var now = DateTime.UtcNow;
            year = now.Year;
            month = now.Month;
        }

        var expenses = await _expensesService.GetAllExpensesAsync(year, month);
        return View(expenses);
    }

    [HttpGet]
    public async Task<IActionResult> GetExpensesTable(int year, int month)
    {
        var expenses = await _expensesService.GetAllExpensesAsync(year, month);
        return PartialView("_ExpensesTablePartial", expenses);
    }

    public IActionResult Create()
    {
        var model = new Expense
        {
            Date = DateTime.UtcNow
        };

        return View(model);
    }

    public async Task<IActionResult> Update(int itemId)
    {
        var expense = await _expensesService.GetExpenseItemAsync(itemId);
        return View(expense);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Expense expense)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user != null)
            {
                expense.UserId = user.UserId;
                await _expensesService.AddExpenseItemAsync(expense);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in.");
                return View(expense);
            }

            return RedirectToAction("Index");
        }

        return View(expense);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int itemId, Expense expense)
    {
        if (ModelState.IsValid)
        {
            await _expensesService.UpdateExpenseItemAsync(itemId, expense);
            return RedirectToAction("Index");
        }

        return View(expense);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int itemId)
    {
        await _expensesService.DeleteExpenseItemAsync(itemId);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult GetChart(int year, int month)
    {
        var data = _expensesService.GetChartData(year, month);
        return Json(data);
    }
}
