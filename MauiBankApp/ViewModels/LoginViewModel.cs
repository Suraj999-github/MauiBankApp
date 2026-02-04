using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Extensions;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.Views;
using System.Text.Json;

namespace MauiBankApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _email = "suraj.goud@eg.com";

        [ObservableProperty]
        private string _password = "password123";

        [ObservableProperty]
        private string _errorMessage;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Login";
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Validate inputs
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Please enter email and password";
                    return;
                }

                // Call auth service
                var result = await _authService.LoginAsync(Email, Password);

                // 🔍 DEBUG: convert object → JSON
                var formatted = JsonSerializer.Serialize(
                    result,
                    new JsonSerializerOptions { WriteIndented = true });

                System.Diagnostics.Debug.WriteLine("LOGIN RESPONSE (OBJECT → JSON):");
                System.Diagnostics.Debug.WriteLine(formatted);



                if (result.IsSuccess)
                {

                    // Save token securely
                    await SecureStorage.Default.SetAsync("auth_token", result.Token);

                    // Pass user to HomeViewModel
                    var transactionService = ServiceHelper.GetService<ITransactionService>();

                    var homeVm = new HomeViewModel(transactionService ,result.User);
                    var homePage = new HomePage(homeVm);

                    await Application.Current.MainPage.Navigation.PushAsync(homePage);

                    // Remove login page
                    Application.Current.MainPage.Navigation.RemovePage(
                        Application.Current.MainPage.Navigation.NavigationStack.First());
                }
                else
                {
                    ErrorMessage = result.Message ?? "Login failed";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
