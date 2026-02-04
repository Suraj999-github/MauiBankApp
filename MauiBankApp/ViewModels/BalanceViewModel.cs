using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Extensions;
using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.Views;
using System.Collections.ObjectModel;

namespace MauiBankApp.ViewModels
{
    public partial class BalanceViewModel : BaseViewModel
    {
        private readonly ITransactionService _transactionService;
      
        [ObservableProperty]
        private User _user = new();

        [ObservableProperty]
        private decimal _currentBalance;

        [ObservableProperty]
        private decimal _availableBalance;

        [ObservableProperty]
        private decimal _totalCredits;

        [ObservableProperty]
        private decimal _totalDebits;

        [ObservableProperty]
        private ObservableCollection<BalanceDetail> _balanceDetails;

        public BalanceViewModel(ITransactionService transactionService, User user)
        {
            _transactionService = transactionService;
            _user = user;
            Title = "Balance Details";
            CurrentBalance = user?.Balance ?? 0;
            AvailableBalance = user?.Balance ?? 0;
            BalanceDetails = new ObservableCollection<BalanceDetail>();

            InitializeBalanceDetails();
            LoadBalanceDataCommand.ExecuteAsync(null);
        }

        private void InitializeBalanceDetails()
        {
            BalanceDetails = new ObservableCollection<BalanceDetail>
            {
                new BalanceDetail { Title = "Available Balance", Amount = CurrentBalance, Type = "available", Icon = "wallet.png" },
                new BalanceDetail { Title = "Total Credits", Amount = 0, Type = "credit", Icon = "credit.png" },
                new BalanceDetail { Title = "Total Debits", Amount = 0, Type = "debit", Icon = "debit.png" },
                new BalanceDetail { Title = "Pending Transactions", Amount = 0, Type = "pending", Icon = "pending.png" },
                new BalanceDetail { Title = "Account Limit", Amount = 50000, Type = "limit", Icon = "limit.png" },
                new BalanceDetail { Title = "Withdrawable Amount", Amount = CurrentBalance, Type = "withdrawable", Icon = "withdraw.png" }
            };
        }

        [RelayCommand]
        private async Task LoadBalanceDataAsync()
        {
            if (IsBusy || _transactionService == null) return;

            try
            {
                IsBusy = true;

                // Get balance from service
                var balanceResponse = await _transactionService.GetBalanceAsync();

                if (balanceResponse.IsSuccess)
                {
                    CurrentBalance = balanceResponse.Data;
                    AvailableBalance = balanceResponse.Data;
                }

                // Get transactions to calculate credits/debits
                var transactionsResponse = await _transactionService.GetTransactionsAsync(limit: 100);

                if (transactionsResponse.IsSuccess && transactionsResponse.Data != null)
                {
                    var transactions = transactionsResponse.Data;
                    TotalCredits = transactions.Where(t => t.Type?.ToLower() == "credit").Sum(t => t.Amount);
                    TotalDebits = transactions.Where(t => t.Type?.ToLower() == "debit").Sum(t => t.Amount);

                    // Update balance details
                    UpdateBalanceDetails();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Failed to load balance data: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RefreshBalanceAsync()
        {
            await LoadBalanceDataAsync();
        }

        [RelayCommand]
        private async Task ViewTransactionHistoryAsync()
        {
            // Navigate to transaction history
            var navigationService = ServiceHelper.GetService<INavigationService>();
            await navigationService.NavigateToAsync<TransactionHistoryPage>();
        }

        [RelayCommand]
        private async Task RequestStatementAsync()
        {
            await Application.Current.MainPage.DisplayAlert(
                "Statement Request",
                "Your account statement has been sent to your registered email.",
                "OK");
        }

        private void UpdateBalanceDetails()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BalanceDetails[0].Amount = AvailableBalance; // Available Balance
                BalanceDetails[1].Amount = TotalCredits;     // Total Credits
                BalanceDetails[2].Amount = TotalDebits;      // Total Debits
                BalanceDetails[3].Amount = 0;                // Pending (hardcoded for now)
                BalanceDetails[4].Amount = 50000;            // Account Limit
                BalanceDetails[5].Amount = AvailableBalance; // Withdrawable Amount
            });
        }
    }

    public class BalanceDetail
    {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string FormattedAmount => $"${Amount:F2}";
        public Color AmountColor => Type switch
        {
            "credit" => Color.FromArgb("#43A047"), // Green
            "debit" => Color.FromArgb("#E53935"),  // Red
            _ => Color.FromArgb("#263238")         // Dark theme color
        };
    }
}