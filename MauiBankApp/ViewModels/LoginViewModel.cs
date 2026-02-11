using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.Views;


namespace MauiBankApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IBiometricAuthService _biometricService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _email = "suraj.goud@eg.com";

        [ObservableProperty]
        private string _password = "password123";

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isBiometricAvailable;

        [ObservableProperty]
        private bool isBiometricEnabled;

        [ObservableProperty]
        private string savedUserEmail = string.Empty;

        public bool IsNotBusy => !IsBusy;

        public LoginViewModel(
            IAuthService authService,
            IBiometricAuthService biometricService,
            INavigationService navigationService)
        {
            _authService = authService;
            _biometricService = biometricService;
            _navigationService = navigationService;

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                // Check if biometric is available and enabled
                IsBiometricAvailable = await _biometricService.IsBiometricAvailableAsync();
                IsBiometricEnabled = await _biometricService.IsBiometricEnabledAsync();

                if (IsBiometricEnabled)
                {
                    // Get stored credentials
                    var (userId, email) = await _biometricService.GetStoredCredentialsAsync();
                    SavedUserEmail = email;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing login: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task Login()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Validate input
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Please enter both email and password";
                    return;
                }

                // Call auth service
                var result = await _authService.LoginAsync(Email, Password);

                if (result.IsSuccess)
                {
                    // Login successful - navigate to home
                    await NavigateToHome(result.User);
                }
                else
                {
                    ErrorMessage = result.Message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LoginWithBiometric()
        {
            if (IsBusy || !IsBiometricEnabled) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Authenticate with biometric
                var authResult = await _biometricService.AuthenticateAsync(
                    "Scan your fingerprint to login");

                if (authResult.IsSuccess)
                {
                    // Get stored credentials
                    var (userId, email) = await _biometricService.GetStoredCredentialsAsync();

                    if (!string.IsNullOrEmpty(email))
                    {
                        // Auto-login with stored credentials
                        // In production, you'd validate the biometric auth with backend
                        // and get a new token
                        var loginResult = await _authService.LoginAsync(email, "biometric_auth_token");

                        if (loginResult.IsSuccess)
                        {
                            await NavigateToHome(loginResult.User);
                        }
                        else
                        {
                            // Fallback: show manual login
                            Email = email;
                            ErrorMessage = "Biometric authentication successful. Please enter your password.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "No saved credentials found. Please login manually.";
                    }
                }
                else
                {
                    ErrorMessage = authResult.Message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Biometric login failed: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToHome(Models.User user)
        {
            // Navigate to home page
            await _navigationService.NavigateToAsync<HomePage>(user);
        }
    }
    //public partial class LoginViewModel : BaseViewModel
    //{
    //    private readonly IAuthService _authService;
    //    private readonly INavigationService _navigationService;
    //    [ObservableProperty]
    //    private string _email = "suraj.goud@eg.com";

    //    [ObservableProperty]
    //    private string _password = "password123";

    //    [ObservableProperty]
    //    private string _errorMessage;

    //    public LoginViewModel(INavigationService navigationService, IAuthService authService)
    //    {
    //        _navigationService = navigationService;
    //        _authService = authService;
    //        Title = "Login";
    //    }

    //    [RelayCommand]
    //    private async Task LoginAsync()
    //    {
    //        if (IsBusy) return;

    //        try
    //        {
    //            IsBusy = true;
    //            ErrorMessage = string.Empty;

    //            // Validate inputs
    //            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
    //            {
    //                ErrorMessage = "Please enter email and password";
    //                return;
    //            }

    //            // Call auth service
    //            var result = await _authService.LoginAsync(Email, Password);

    //            // DEBUG: convert object → JSON
    //            var formatted = JsonSerializer.Serialize(
    //                result,
    //                new JsonSerializerOptions { WriteIndented = true });

    //            System.Diagnostics.Debug.WriteLine("LOGIN RESPONSE (OBJECT → JSON):");
    //            System.Diagnostics.Debug.WriteLine(formatted);



    //            if (result.IsSuccess)
    //            {

    //                // Save token securely
    //                await SecureStorage.Default.SetAsync("auth_token", result.Token);

    //                // Pass user to HomeViewModel
    //                var transactionService = ServiceHelper.GetService<ITransactionService>();

    //                var homeVm = new HomeViewModel(_navigationService, transactionService, result.User);
    //                var homePage = new HomePage(homeVm);

    //                await Application.Current.MainPage.Navigation.PushAsync(homePage);

    //                // Remove login page
    //                Application.Current.MainPage.Navigation.RemovePage(
    //                    Application.Current.MainPage.Navigation.NavigationStack.First());
    //            }
    //            else
    //            {
    //                ErrorMessage = result.Message ?? "Login failed";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorMessage = $"Error: {ex.Message}";
    //        }
    //        finally
    //        {
    //            IsBusy = false;
    //        }
    //    }
    //    private async Task NavigateToRegisterAsync()
    //    {
    //        await Application.Current.MainPage.DisplayAlert(
    //            "Info",
    //            "Registration feature coming soon!",
    //            "OK");
    //    }

    //    [RelayCommand]
    //    private async Task ForgotPasswordAsync()
    //    {
    //        await Application.Current.MainPage.DisplayAlert(
    //            "Info",
    //            "Password reset feature coming soon!",
    //            "OK");
    //    }
    //}
}
