using MauiBankApp.ViewModels;

namespace MauiBankApp.Views;

public partial class BalancePage : ContentPage
{
    public BalancePage(BalanceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is BalanceViewModel viewModel)
        {
            viewModel.RefreshBalanceCommand.ExecuteAsync(null);
        }
    }
}