using BudgetTracker.ViewModels;

namespace BudgetTracker.Views
{
    public partial class TransactionsPage : ContentPage
    {
        private readonly TransactionsViewModel _viewModel;
        //Typed access for bindings inside the list rows
        public TransactionsViewModel ViewModel => _viewModel;

        // DI supplies the ViewModel
        public TransactionsPage(TransactionsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        // Runs every time the page comes on screen
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}