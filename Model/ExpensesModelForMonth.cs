

namespace ExpTracApp.Model
{
    public class ExpensesModelForMonth
    {
        public int Id { get; set; }
        public string Month { get; set; } = string.Empty;
        public int MonthNumber { get; set; }
        public decimal TotalNumber { get; set; }
    }
}
