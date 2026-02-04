using MauiBankApp.Views;

namespace MauiBankApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Register routes for navigation
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(BalancePage), typeof(BalancePage));
            Routing.RegisterRoute(nameof(TransactionHistoryPage), typeof(TransactionHistoryPage));
            Routing.RegisterRoute(nameof(SendTransactionPage), typeof(SendTransactionPage));
            Routing.RegisterRoute(nameof(QRCodePage), typeof(QRCodePage));
            Routing.RegisterRoute(nameof(MobileTopUpPage), typeof(MobileTopUpPage));
            Routing.RegisterRoute(nameof(WaterBillPage), typeof(WaterBillPage));
            Routing.RegisterRoute(nameof(ElectricityBillPage), typeof(ElectricityBillPage));
            Routing.RegisterRoute(nameof(SecuritySettingsPage), typeof(SecuritySettingsPage));
        }
    }
}
