using BudgetTracker.Models;
using SQLite;

namespace BudgetTracker.Services
{
    public class DatabaseService
    {
        private const string DatabaseFilename = "budget.db3";

        private const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |   // read and write
            SQLiteOpenFlags.Create |      // create the file if it doesn't exist
            SQLiteOpenFlags.SharedCache;  // allow multithreaded access

        // App-private storage on the phone; wiped if the app is uninstalled
        private static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        private SQLiteAsyncConnection? _database;

        // Lazy init: open the connection and create tables on first use, not at app startup
        private async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            if (_database is not null)
                return _database;

            _database = new SQLiteAsyncConnection(DatabasePath, Flags);
            await _database.CreateTableAsync<Transaction>();
            return _database;
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            var db = await GetConnectionAsync();
            return await db.Table<Transaction>()
                           .OrderByDescending(t => t.Date)
                           .ThenByDescending(t => t.Id)
                           .ToListAsync();
        }

        public async Task<int> AddTransactionAsync(Transaction transaction)
        {
            var db = await GetConnectionAsync();
            return await db.InsertAsync(transaction);
        }

        public async Task<int> DeleteTransactionAsync(Transaction transaction)
        {
            var db = await GetConnectionAsync();
            return await db.DeleteAsync(transaction);
        }
    }
}