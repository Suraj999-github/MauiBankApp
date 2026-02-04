using MauiBankApp.ViewModels;

namespace MauiBankApp.Views;

public partial class TransactionHistoryPage : ContentPage
{
    public TransactionHistoryPage(TransactionHistoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TransactionHistoryViewModel viewModel)
        {
            viewModel.LoadTransactionsCommand.Execute(null);
        }
    }
}