
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
namespace MauiBankApp.ViewModels
{
    public partial class TransactionHistoryViewModel : BaseViewModel
    {
        private readonly ITransactionService _transactionService;

        [ObservableProperty]
        private List<Transaction> _transactions;

        [ObservableProperty]
        private bool _showEmptyState;

        public TransactionHistoryViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            Title = "Transaction History";
            Transactions = new List<Transaction>();
            LoadTransactionsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadTransactions()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var result = await _transactionService.GetTransactionsAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    Transactions = result.Data;
                    ShowEmptyState = !Transactions.Any();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", result.Message ?? "Failed to load transactions", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load transactions: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
