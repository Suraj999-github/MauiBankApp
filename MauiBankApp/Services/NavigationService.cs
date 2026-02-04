using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.ViewModels;
using MauiBankApp.Views;

namespace MauiBankApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private INavigation Navigation =>
            Application.Current?.MainPage?.Navigation ??
            throw new InvalidOperationException("Navigation not available");

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToAsync<T>() where T : Page
        {
            await NavigateToAsync(typeof(T));
        }

        public async Task NavigateToAsync<T>(object parameter) where T : Page
        {
            await NavigateToAsync(typeof(T), parameter);
        }

        public async Task NavigateToAsync(Type pageType, object parameter = null)
        {
            try
            {
                var page = CreatePage(pageType, parameter);
                if (page != null)
                {
                    await Navigation.PushAsync(page);
                }
            }
            catch (Exception ex)
            {
                await HandleNavigationErrorAsync(ex);
            }
        }

        public async Task GoBackAsync()
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
        }

        public async Task NavigateToRootAsync()
        {
            await Navigation.PopToRootAsync();
        }

        private Page CreatePage(Type pageType, object parameter)
        {
            return pageType.Name switch
            {
                nameof(HomePage) => CreateHomePage(parameter as User),
                nameof(TransactionHistoryPage) => CreateTransactionHistoryPage(),
                //  nameof(SendTransactionPage) => CreateSendTransactionPage(parameter as User),
                // nameof(ProfilePage) => CreateProfilePage(parameter as User),
                nameof(BalancePage) => CreateBalancePage(parameter as User),
                nameof(QRCodePage) => CreateQRCodePage(),
                // nameof(MobileTopUpPage) => CreateMobileTopUpPage(),
                // nameof(WaterBillPage) => CreateWaterBillPage(),
                // nameof(ElectricityBillPage) => CreateElectricityBillPage(),
                nameof(SecuritySettingsPage) => CreateSecuritySettingsPage(),
                _ => CreateDefaultPage(pageType)
            };
        }

        private Page CreateHomePage(User user)
        {
            var transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
            var navigationService = this; // Pass self for navigation
            var viewModel = new HomeViewModel(navigationService, transactionService, user);
            return new HomePage(viewModel);
        }

        private Page CreateTransactionHistoryPage()
        {
            var transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
            var viewModel = new TransactionHistoryViewModel(transactionService);
            return new TransactionHistoryPage(viewModel);
        }

        //private Page CreateSendTransactionPage(User user)
        //{
        //    var transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
        //    var viewModel = new SendTransactionViewModel(transactionService, user);
        //    return new SendTransactionPage(viewModel);
        //}

        //private Page CreateProfilePage(User user)
        //{
        //    // If ProfileViewModel exists
        //    var profileViewModel = new ProfileViewModel(user);
        //    return new ProfilePage(profileViewModel);
        //}

        private Page CreateBalancePage(User user)
        {
            var transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
            var viewModel = new BalanceViewModel(transactionService, user);
            return new BalancePage(viewModel);
        }
        private Page CreateQRCodePage()
        {
            // Assuming QRCodePage has parameterless constructor or gets its ViewModel via DI
            return new QRCodePage();
        }

        //private Page CreateMobileTopUpPage()
        //{
        //    var transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
        //    var viewModel = new MobileTopUpViewModel(transactionService);
        //    return new MobileTopUpPage(viewModel);
        //}

        //private Page CreateWaterBillPage()
        //{
        //    var transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
        //    var viewModel = new WaterBillViewModel(transactionService);
        //    return new WaterBillPage(viewModel);
        //}

        //private Page CreateElectricityBillPage()
        //{
        //    var transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
        //    var viewModel = new ElectricityBillViewModel(transactionService);
        //    return new ElectricityBillPage(viewModel);
        //}

        private Page CreateSecuritySettingsPage()
        {
            return new SecuritySettingsPage();
        }

        private Page CreateDefaultPage(Type pageType)
        {
            // Try to get from DI first
            var page = _serviceProvider.GetService(pageType) as Page;
            if (page != null)
                return page;

            // Fallback to Activator.CreateInstance
            return Activator.CreateInstance(pageType) as Page;
        }

        private async Task HandleNavigationErrorAsync(Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Navigation Error",
                $"Failed to navigate: {ex.Message}",
                "OK");
        }
    }
}
