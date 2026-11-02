using SQLite;

namespace BudgetTracker.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Money stored as whole cents: $12.50 -> 1250
        public long AmountCents { get; set; }

        // Plain text for now; becomes a CategoryId in Phase 3
        [NotNull]
        public string Category { get; set; } = string.Empty;

        // Date only (time = 00:00); indexed because every screen filters by month
        [Indexed]
        public DateTime Date { get; set; } = DateTime.Today;

        public string? Note { get; set; }

        // Display helper, not a database column
        [Ignore]
        public decimal Amount => AmountCents / 100m;
    }
}