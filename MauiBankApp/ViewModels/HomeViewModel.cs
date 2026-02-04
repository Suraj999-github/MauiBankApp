using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Extensions;
using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.Views;
using System.Collections.ObjectModel;

namespace MauiBankApp.ViewModels
{
    public class DashboardItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type PageType { get; set; }
        public object Parameter { get; set; }
    }

    public partial class HomeViewModel : BaseViewModel
    {
        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]
        private decimal _balance;

        [ObservableProperty]
        private ObservableCollection<Transaction> _recentTransactions;

        private readonly ITransactionService _transactionService;
        private readonly INavigationService _navigationService;
        public List<DashboardItem> DashboardItems { get; private set; }

        public HomeViewModel(INavigationService navigationService, ITransactionService transactionService, User user)
        {
            _navigationService = navigationService;
            _transactionService = transactionService;
            Title = "Dashboard";
            SetUser(user);
            InitializeDashboardItems();
            RecentTransactions = new ObservableCollection<Transaction>();

            // Load initial data
            LoadRecentTransactionsCommand.ExecuteAsync(null);
        }


        // (Optional) parameterless constructor for design-time / preview
        public HomeViewModel() : this(null, null, new User())
        {
        }

        private void SetUser(User user)
        {
            CurrentUser = user;
            Balance = user?.Balance ?? 0;
        }

        private void InitializeDashboardItems()
        {

            DashboardItems = new List<DashboardItem>
            {
               // new DashboardItem { Title = "View Balance", Icon = "wallet.png", PageType = typeof(BalancePage)},
               // new DashboardItem { Title = "Transactions", Icon = "history.png", PageType = typeof(TransactionHistoryPage) },
               // new DashboardItem { Title = "Send Money", Icon = "send.png", PageType = typeof(SendTransactionPage), Parameter = CurrentUser },
               // new DashboardItem { Title = "QR Code", Icon = "qrcode.png", PageType = typeof(QRCodePage) },
                new DashboardItem { Title = "Mobile Top-up", Icon = "mobile.png", PageType = typeof(MobileTopUpPage) },
                new DashboardItem { Title = "Water Bill", Icon = "water.png", PageType = typeof(WaterBillPage) },
                new DashboardItem { Title = "Electricity", Icon = "electricity.png", PageType = typeof(ElectricityBillPage) },
                new DashboardItem { Title = "Security", Icon = "security.png", PageType = typeof(SecuritySettingsPage) }
            };
        }
        [RelayCommand]
        private async Task LoadRecentTransactionsAsync()
        {
            if (IsBusy || _transactionService == null) return;

            try
            {
                IsBusy = true;

                var response = await _transactionService.GetTransactionsAsync(limit: 5);

                if (response.IsSuccess && response.Data != null)
                {
                    // Clear and update the collection on the main thread
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        RecentTransactions.Clear();
                        foreach (var transaction in response.Data)
                        {
                            RecentTransactions.Add(transaction);
                        }
                    });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Failed to load recent transactions",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"An error occurred: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToFeatureAsync(DashboardItem item)
        {
            if (IsBusy || item?.PageType == null) return;

            try
            {
                IsBusy = true;
                await _navigationService.NavigateToAsync(item.PageType, item.Parameter);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                SecureStorage.Default.Remove("auth_token");

                var loginPage = new LoginPage(ServiceHelper.GetService<LoginViewModel>());
                Application.Current.MainPage = new NavigationPage(loginPage);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
