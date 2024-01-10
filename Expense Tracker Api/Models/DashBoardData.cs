using Expense_Tracker_Api.Controllers;

namespace Expense_Tracker_Api.Models
{
    public class DashBoardData
    {
        public string TotalIncome { get; set; }
        public string TotalExpense { get; set; }
        public string Balance { get; set; }
        public List<DoughnutChartData> DoughnutChartData { get; set; }
        public IEnumerable<object> SplineChartData { get; set; }
        public List<Transaction> RecentTransactions { get; set; }
    }
}
