using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
using System.Collections.ObjectModel;

namespace MauiBankApp.ViewModels
{
    public partial class TransactionHistoryViewModel : BaseViewModel
    {
        private readonly ITransactionService _transactionService;

        [ObservableProperty]
        private ObservableCollection<Transaction> _transactions;

        [ObservableProperty]
        private bool _showEmptyState;

        public TransactionHistoryViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            Title = "Transaction History";
            Transactions = new ObservableCollection<Transaction>();
        }

        [RelayCommand]
        private async Task LoadTransactionsAsync()
        {
            if (IsBusy || _transactionService == null) return;

            try
            {
                IsBusy = true;
                ShowEmptyState = false;

                var response = await _transactionService.GetTransactionsAsync(limit: 20);

                if (response.IsSuccess && response.Data != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Transactions.Clear();
                        foreach (var transaction in response.Data.OrderByDescending(t => t.Date))
                        {
                            Transactions.Add(transaction);
                        }
                        ShowEmptyState = Transactions.Count == 0;
                    });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Failed to load transactions",
                        "OK");
                    ShowEmptyState = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"An error occurred: {ex.Message}",
                    "OK");
                ShowEmptyState = true;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}