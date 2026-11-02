using BudgetTracker.Services;
using BudgetTracker.ViewModels;
using BudgetTracker.Views;
using Microsoft.Extensions.Logging;

namespace BudgetTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            //Services: one shared instance for the apps lifetime
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<TransactionsViewModel>();
            builder.Services.AddTransient<TransactionsPage>();
            return builder.Build();
        }
    }
}
