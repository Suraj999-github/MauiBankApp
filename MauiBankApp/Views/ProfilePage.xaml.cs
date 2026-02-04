using MauiBankApp.Services.Mock;
using MauiBankApp.ViewModels;
namespace MauiBankApp.Views;
public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        // For now, create ViewModel directly
        BindingContext = new ProfileViewModel(new MockUserService());
    }

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}