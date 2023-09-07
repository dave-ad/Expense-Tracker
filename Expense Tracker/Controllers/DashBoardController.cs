using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
