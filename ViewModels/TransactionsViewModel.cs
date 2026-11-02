using BudgetTracker.Models;
using BudgetTracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace BudgetTracker.ViewModels
{
    public partial class TransactionsViewModel : ObservableObject
    {
        private readonly DatabaseService _database;

        // DI supplies the shared DatabaseService
        public TransactionsViewModel(DatabaseService database)
        {
            _database = database;
        }

        // The list on screen; ObservableCollection tells the UI when items are added/removed
        public ObservableCollection<Transaction> Transactions { get; } = new();

        // Form fields: the toolkit generates public properties AmountText, Category, Date, Note
        [ObservableProperty]
        private string amountText = string.Empty;

        [ObservableProperty]
        private string category = string.Empty;

        [ObservableProperty]
        private DateTime date = DateTime.Today;

        [ObservableProperty]
        private string? note;

        // Generates LoadCommand
        [RelayCommand]
        private async Task LoadAsync()
        {
            var items = await _database.GetTransactionsAsync();
            Transactions.Clear();
            foreach (var item in items)
                Transactions.Add(item);
        }

        // Generates AddCommand
        [RelayCommand]
        private async Task AddAsync()
        {
            if (!decimal.TryParse(AmountText, out var amount) || amount <= 0)
                return;
            if (string.IsNullOrWhiteSpace(Category))
                return;

            var transaction = new Transaction
            {
                AmountCents = (long)Math.Round(amount * 100),
                Category = Category.Trim(),
                Date = Date.Date,
                Note = string.IsNullOrWhiteSpace(Note) ? null : Note.Trim()
            };

            await _database.AddTransactionAsync(transaction);
            await LoadAsync();

            // Clear the form but keep Category and Date for quick repeat entries
            AmountText = string.Empty;
            Note = null;
        }

        // Generates DeleteCommand, which takes the Transaction to delete as a parameter
        [RelayCommand]
        private async Task DeleteAsync(Transaction transaction)
        {
            await _database.DeleteTransactionAsync(transaction);
            Transactions.Remove(transaction);
        }
    }
}