using MauiBankApp.Services;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.Services.Mock;
using MauiBankApp.ViewModels;
using MauiBankApp.Views;
using Microsoft.Extensions.Logging;

namespace MauiBankApp
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; }
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

            // Register Services
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IAuthService, MockAuthService>();
            builder.Services.AddSingleton<IUserService, MockUserService>();
            builder.Services.AddSingleton<ITransactionService, MockTransactionService>();

            // Register ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<TransactionHistoryViewModel>();
            builder.Services.AddTransient<BalanceViewModel>();
            // Add other ViewModels

            // Register Views
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<BalancePage>();
            builder.Services.AddSingleton<TransactionHistoryPage>();
            builder.Services.AddSingleton<SendTransactionPage>();
            builder.Services.AddSingleton<QRCodePage>();
            builder.Services.AddSingleton<MobileTopUpPage>();
            builder.Services.AddSingleton<WaterBillPage>();
            builder.Services.AddSingleton<ElectricityBillPage>();
            builder.Services.AddSingleton<SecuritySettingsPage>();
            // Add other Pages
            return builder.Build();
        }
    }
}
