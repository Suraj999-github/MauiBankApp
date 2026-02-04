using MauiBankApp.Services.Mock;
using MauiBankApp.ViewModels;
namespace MauiBankApp.Views;
public partial class ProfilePage : ContentPage
{
    //public ProfilePage()
    //{
    //    InitializeComponent();

    //    // Get ViewModel from DI container
    //    var viewModel = MauiProgram.Services.GetService<ProfileViewModel>();
    //    BindingContext = viewModel;
    //}

  
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