namespace MauiBankApp.Views;

public partial class BasePage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Set navigation bar color
        if (Parent is NavigationPage navPage)
        {
            navPage.BarBackgroundColor = Color.FromArgb("#007AFF");
            navPage.BarTextColor = Colors.White;
        }
    }
}