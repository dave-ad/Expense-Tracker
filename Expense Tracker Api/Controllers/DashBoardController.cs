using Expense_Tracker_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashBoardController : ControllerBase
{
    private readonly AppDbContext _context;
    public DashBoardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("TotalIncomeExpenseBalance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DashBoardTotalData>> GetTotalIncomeExpenseBalance()
    {
        try
        {
            // Last 7 Days
            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;

            List<Transaction> selectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= startDate && y.Date <= endDate)
                .ToListAsync();

            // Total Income
            int totalIncome = selectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);

            // Total Expense
            int totalExpense = selectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);

            // Balance
            int balance = totalIncome - totalExpense;

            var totalData = new DashBoardTotalData
            {
                TotalIncome = totalIncome.ToString("C0"),
                TotalExpense = totalExpense.ToString("C0"),
                Balance = balance.ToString("C0"),
            };

            return Ok(totalData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("DoughnutChartData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<DoughnutChartData>>> GetDoughnutChartData()
    {
        try
        {
            //Doughnut Chart -Expense By Category

            List<Transaction> selectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(i => i.Category.Type == "Expense")
                .ToListAsync();


            var doughnutChartData = selectedTransactions
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new DoughnutChartData()
                {
                    CategoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    Amount = k.Sum(j => j.Amount),
                    FormattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.Amount)
                .ToList();

            return Ok(doughnutChartData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }


    [HttpGet("SplineChartData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<object>>> GetSplineChartData()
    {
        try
        {
            // Spline Chart - Income vs Expense
            var selectedTransactions = await _context.Transactions
                .Where(i => i.Category.Type == "Income" || i.Category.Type == "Expense")
                .ToListAsync();

            // Income
            List<SplineChartData> incomeSummary = selectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    Day = k.First().Date.ToString("dd-MMM"),
                    Income = k.Sum(l => l.Amount)
                })
                .ToList();

            // Expense
            List<SplineChartData> expenseSummary = selectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    Day = k.First().Date.ToString("dd-MMM"),
                    Expense = k.Sum(l => l.Amount)
                })
                .ToList();

            // Combine Income & Expense
            string[] last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-6).AddDays(i).ToString("dd-MMM"))
                .ToArray();

            var splineChartData = from day in last7Days
                                  join income in incomeSummary on day equals income?.Day into dayIncomeJoined
                                  from income in dayIncomeJoined.DefaultIfEmpty()
                                  join expense in expenseSummary on day equals expense.Day into expenseJoined
                                  from expense in expenseJoined.DefaultIfEmpty()
                                  select new
                                  {
                                      Day = day,
                                      Income = income == null ? 0 : income.Income,
                                      Expense = expense == null ? 0 : expense.Expense,
                                  };
            return Ok(splineChartData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("RecentTransactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Transaction>>> GetRecentTransactions()
    {
        try
        {
            // Sample logic to retrieve recent transactions
            var recentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            return Ok(recentTransactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}