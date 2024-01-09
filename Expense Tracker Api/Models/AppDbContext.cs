using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Api.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
