namespace MauiBankApp.Views;

public partial class SecuritySettingsPage : ContentPage
{
    public SecuritySettingsPage()
    {
        InitializeComponent();
    }
    private async void GoBackButton_Clicked(object sender, EventArgs e)
    {
        // Navigate back
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
        else
        {
            // Optionally, close the app or navigate to home
            await DisplayAlert("Notice", "No page to go back to.", "OK");
        }
    }
}