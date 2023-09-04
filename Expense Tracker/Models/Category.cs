using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Category
    {
        [Key]// make it a primary Key
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")] // For string properties, it is necessary to specify the necessary SQL server datatype
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expense";
    }
}
