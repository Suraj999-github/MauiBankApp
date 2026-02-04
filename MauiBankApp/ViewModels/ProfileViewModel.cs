using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;

namespace MauiBankApp.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private User _user = new();
        public ProfileViewModel(IUserService userService)
        {
            _userService = userService;
            Title = "Profile";
            LoadUserProfileCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadUserProfileAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var result = await _userService.GetUserProfileAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    User = result.Data;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"Failed to load profile", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load profile: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
