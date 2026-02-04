using MauiBankApp.Extensions;
using MauiBankApp.ViewModels;
using MauiBankApp.Views;

namespace MauiBankApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //debug handler for uncaught exceptions
#if DEBUG
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"FirstChanceException: {e.Exception}");
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"UnhandledException: {e.ExceptionObject}");
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
            };
#endif

            var loginViewModel = ServiceHelper.GetService<LoginViewModel>();
            var loginPage = new LoginPage(loginViewModel);
            MainPage = new NavigationPage(loginPage);
        }
    }
}