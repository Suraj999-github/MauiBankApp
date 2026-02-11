using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiBankApp.Services.Interfaces;
namespace MauiBankApp.ViewModels
{
    public partial class SecuritySettingsViewModel : ObservableObject
    {
        private readonly IBiometricAuthService _biometricService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private bool isBiometricAvailable;

        [ObservableProperty]
        private bool isBiometricEnabled;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string biometricType = "Fingerprint";

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private DateTime? enrolledDate;

        public SecuritySettingsViewModel(IBiometricAuthService biometricService, IAuthService authService)
        {
            _biometricService = biometricService;
            _authService = authService;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;

                // Check if biometric is available on device
                IsBiometricAvailable = await _biometricService.IsBiometricAvailableAsync();

                if (IsBiometricAvailable)
                {
                    // Check if biometric is enabled for user
                    IsBiometricEnabled = await _biometricService.IsBiometricEnabledAsync();

                    // Get biometric type
                    BiometricType = await _biometricService.GetBiometricTypeAsync();

                    if (IsBiometricEnabled)
                    {
                        StatusMessage = $"{BiometricType} authentication is enabled";
                    }
                    else
                    {
                        StatusMessage = $"{BiometricType} authentication is available but not enabled";
                    }
                }
                else
                {
                    StatusMessage = "Biometric authentication is not available on this device";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ToggleBiometric(bool newValue)
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;

                if (newValue)
                {
                    // Enable biometric - simulate fingerprint scan
                    StatusMessage = "Scanning fingerprint...";

                    var response = await _biometricService.EnableBiometricAsync();

                    if (response.IsSuccess)
                    {
                        // Get current user info from auth service
                        var isAuthenticated = await _authService.IsAuthenticatedAsync();
                        if (isAuthenticated)
                        {
                            // In production, get user from a user service
                            // For now, we'll use mock data
                            await _biometricService.StoreCredentialsForBiometricAsync("1", "suraj.goud@eg.com");
                        }

                        IsBiometricEnabled = true;
                        EnrolledDate = response.EnrolledAt;
                        StatusMessage = response.Message;
                        await Application.Current.MainPage.DisplayAlert(
                            "Success",
                            "Fingerprint scanned and saved! You can now login using your fingerprint.",
                            "OK");
                    }
                    else
                    {
                        // Revert the switch if operation failed
                        IsBiometricEnabled = false;
                        StatusMessage = response.Message;
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            response.Message,
                            "OK");
                    }
                }
                else
                {
                    // Disable biometric
                    var result = await _biometricService.DisableBiometricAsync();

                    if (result)
                    {
                        IsBiometricEnabled = false;
                        EnrolledDate = null;
                        StatusMessage = $"{BiometricType} authentication disabled";
                        await Application.Current.MainPage.DisplayAlert(
                            "Success",
                            $"{BiometricType} authentication has been disabled",
                            "OK");
                    }
                    else
                    {
                        // Revert the switch if operation failed
                        IsBiometricEnabled = true;
                        StatusMessage = "Failed to disable biometric authentication";
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            "Failed to disable biometric authentication",
                            "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Revert the switch on error
                IsBiometricEnabled = !newValue;
                StatusMessage = $"Error: {ex.Message}";
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"An error occurred: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task TestAuthentication()
        {
            if (IsLoading || !IsBiometricEnabled) return;

            try
            {
                IsLoading = true;
                StatusMessage = "Authenticating...";

                var response = await _biometricService.AuthenticateAsync("Verify your identity to test fingerprint");

                if (response.IsSuccess)
                {
                    StatusMessage = "Authentication successful!";
                    await Application.Current.MainPage.DisplayAlert(
                        "Success",
                        "Fingerprint authentication successful!",
                        "OK");
                }
                else
                {
                    StatusMessage = response.Message;
                    await Application.Current.MainPage.DisplayAlert(
                        "Failed",
                        response.Message,
                        "OK");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"An error occurred: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GoBack()
        {
            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
    //public partial class SecuritySettingsViewModel : ObservableObject
    //{
    //    private readonly IBiometricAuthService _biometricService;

    //    [ObservableProperty]
    //    private bool isBiometricAvailable;

    //    [ObservableProperty]
    //    private bool isBiometricEnabled;

    //    [ObservableProperty]
    //    private bool isLoading;

    //    [ObservableProperty]
    //    private string biometricType = "Fingerprint";

    //    [ObservableProperty]
    //    private string statusMessage = string.Empty;

    //    [ObservableProperty]
    //    private DateTime? enrolledDate;

    //    public SecuritySettingsViewModel(IBiometricAuthService biometricService)
    //    {
    //        _biometricService = biometricService;
    //        _ = InitializeAsync();
    //    }

    //    private async Task InitializeAsync()
    //    {
    //        try
    //        {
    //            IsLoading = true;

    //            // Check if biometric is available on device
    //            IsBiometricAvailable = await _biometricService.IsBiometricAvailableAsync();

    //            if (IsBiometricAvailable)
    //            {
    //                // Check if biometric is enabled for user
    //                IsBiometricEnabled = await _biometricService.IsBiometricEnabledAsync();

    //                // Get biometric type
    //                BiometricType = await _biometricService.GetBiometricTypeAsync();

    //                if (IsBiometricEnabled)
    //                {
    //                    StatusMessage = $"{BiometricType} authentication is enabled";
    //                }
    //                else
    //                {
    //                    StatusMessage = $"{BiometricType} authentication is available but not enabled";
    //                }
    //            }
    //            else
    //            {
    //                StatusMessage = "Biometric authentication is not available on this device";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = $"Error: {ex.Message}";
    //        }
    //        finally
    //        {
    //            IsLoading = false;
    //        }
    //    }

    //    [RelayCommand]
    //    private async Task ToggleBiometric(bool newValue)
    //    {
    //        if (IsLoading) return;

    //        try
    //        {
    //            IsLoading = true;

    //            if (newValue)
    //            {
    //                // Enable biometric
    //                var response = await _biometricService.EnableBiometricAsync();

    //                if (response.IsSuccess)
    //                {
    //                    IsBiometricEnabled = true;
    //                    EnrolledDate = response.EnrolledAt;
    //                    StatusMessage = response.Message;
    //                    await Application.Current.MainPage.DisplayAlert(
    //                        "Success",
    //                        response.Message,
    //                        "OK");
    //                }
    //                else
    //                {
    //                    // Revert the switch if operation failed
    //                    IsBiometricEnabled = false;
    //                    StatusMessage = response.Message;
    //                    await Application.Current.MainPage.DisplayAlert(
    //                        "Error",
    //                        response.Message,
    //                        "OK");
    //                }
    //            }
    //            else
    //            {
    //                // Disable biometric
    //                var result = await _biometricService.DisableBiometricAsync();

    //                if (result)
    //                {
    //                    IsBiometricEnabled = false;
    //                    EnrolledDate = null;
    //                    StatusMessage = $"{BiometricType} authentication disabled";
    //                    await Application.Current.MainPage.DisplayAlert(
    //                        "Success",
    //                        $"{BiometricType} authentication has been disabled",
    //                        "OK");
    //                }
    //                else
    //                {
    //                    // Revert the switch if operation failed
    //                    IsBiometricEnabled = true;
    //                    StatusMessage = "Failed to disable biometric authentication";
    //                    await Application.Current.MainPage.DisplayAlert(
    //                        "Error",
    //                        "Failed to disable biometric authentication",
    //                        "OK");
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // Revert the switch on error
    //            IsBiometricEnabled = !newValue;
    //            StatusMessage = $"Error: {ex.Message}";
    //            await Application.Current.MainPage.DisplayAlert(
    //                "Error",
    //                $"An error occurred: {ex.Message}",
    //                "OK");
    //        }
    //        finally
    //        {
    //            IsLoading = false;
    //        }
    //    }

    //    [RelayCommand]
    //    private async Task TestAuthentication()
    //    {
    //        if (IsLoading || !IsBiometricEnabled) return;

    //        try
    //        {
    //            IsLoading = true;
    //            StatusMessage = "Authenticating...";

    //            var response = await _biometricService.AuthenticateAsync("Verify your identity to test fingerprint");

    //            if (response.IsSuccess)
    //            {
    //                StatusMessage = "Authentication successful!";
    //                await Application.Current.MainPage.DisplayAlert(
    //                    "Success",
    //                    "Fingerprint authentication successful!",
    //                    "OK");
    //            }
    //            else
    //            {
    //                StatusMessage = response.Message;
    //                await Application.Current.MainPage.DisplayAlert(
    //                    "Failed",
    //                    response.Message,
    //                    "OK");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = $"Error: {ex.Message}";
    //            await Application.Current.MainPage.DisplayAlert(
    //                "Error",
    //                $"An error occurred: {ex.Message}",
    //                "OK");
    //        }
    //        finally
    //        {
    //            IsLoading = false;
    //        }
    //    }

    //    [RelayCommand]
    //    private async Task GoBack()
    //    {
    //        if (Application.Current?.MainPage?.Navigation != null)
    //        {
    //            await Application.Current.MainPage.Navigation.PopAsync();
    //        }
    //    }
    //}
}
