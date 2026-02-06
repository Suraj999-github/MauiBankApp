using MauiBankApp.Services;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.Services.Mock;
using MauiBankApp.ViewModels;
using MauiBankApp.Views;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
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
                .UseBarcodeReader() 
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.UseBarcodeReader();
            // Services
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IAuthService, MockAuthService>();
            builder.Services.AddSingleton<IUserService, MockUserService>();
            builder.Services.AddSingleton<ITransactionService, MockTransactionService>();

            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<TransactionHistoryViewModel>();
            builder.Services.AddTransient<BalanceViewModel>();

            // Views
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

            return builder.Build();
        }
    }
}
