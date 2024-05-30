using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTracApp.Model
{
    [Table("Expenses")]
    public class Expenses
    {
        [PrimaryKey , AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
