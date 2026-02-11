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
        private string biometricType = "Fingerprint";

        [ObservableProperty]
        private DateTime? enrolledDate;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        // Events for UI animations
        public event EventHandler? ScanStarted;
        public event EventHandler<(bool success, string message)>? ScanCompleted;

        public SecuritySettingsViewModel(
            IBiometricAuthService biometricService,
            IAuthService authService)
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

                // Check if biometric is available
                IsBiometricAvailable = await _biometricService.IsBiometricAvailableAsync();

                if (IsBiometricAvailable)
                {
                    // Check if already enabled
                    IsBiometricEnabled = await _biometricService.IsBiometricEnabledAsync();

                    // Get biometric type
                    BiometricType = await _biometricService.GetBiometricTypeAsync();

                    // Get enrolled date if enabled
                    if (IsBiometricEnabled)
                    {
                        var enrollmentDate = await SecureStorage.Default.GetAsync("biometric_enrollment");
                        if (!string.IsNullOrEmpty(enrollmentDate) &&
                            DateTime.TryParse(enrollmentDate, out var date))
                        {
                            EnrolledDate = date;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error initializing: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ToggleBiometric(bool enable)
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                StatusMessage = string.Empty;

                if (enable)
                {
                    // Notify UI to start scan animation
                    ScanStarted?.Invoke(this, EventArgs.Empty);

                    // Enable biometric - this simulates fingerprint scanning
                    var result = await _biometricService.EnableBiometricAsync();

                    if (result.IsSuccess)
                    {
                        IsBiometricEnabled = true;
                        EnrolledDate = result.EnrolledAt;
                        StatusMessage = "Fingerprint authentication enabled successfully!";

                        // Store user credentials for biometric login
                        var user = await GetCurrentUserAsync();
                        if (user != null)
                        {
                            await _biometricService.StoreCredentialsForBiometricAsync(
                                user.Id,
                                user.Email);
                        }

                        // Notify success
                        ScanCompleted?.Invoke(this, (true, "Success!"));
                    }
                    else
                    {
                        IsBiometricEnabled = false;
                        StatusMessage = result.Message;

                        // Notify error
                        ScanCompleted?.Invoke(this, (false, result.Message));
                    }
                }
                else
                {
                    // Disable biometric
                    var success = await _biometricService.DisableBiometricAsync();

                    if (success)
                    {
                        IsBiometricEnabled = false;
                        EnrolledDate = null;
                        StatusMessage = "Fingerprint authentication disabled";
                    }
                    else
                    {
                        StatusMessage = "Failed to disable biometric authentication";
                        // Revert toggle
                        IsBiometricEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                IsBiometricEnabled = !enable; // Revert

                if (enable)
                {
                    ScanCompleted?.Invoke(this, (false, ex.Message));
                }
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
                StatusMessage = string.Empty;

                // Notify UI to start scan animation
                ScanStarted?.Invoke(this, EventArgs.Empty);

                // Test biometric authentication
                var result = await _biometricService.AuthenticateAsync(
                    "Test your fingerprint authentication");

                if (result.IsSuccess)
                {
                    StatusMessage = "✓ Authentication successful!";
                    ScanCompleted?.Invoke(this, (true, "Authentication successful!"));
                }
                else
                {
                    StatusMessage = $"✗ {result.Message}";
                    ScanCompleted?.Invoke(this, (false, result.Message));
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                ScanCompleted?.Invoke(this, (false, ex.Message));
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task<Models.User?> GetCurrentUserAsync()
        {
            // In a real app, you would get the current user from auth service
            // For now, return a mock user
            return new Models.User
            {
                Id = "1",
                Name = "Suraj Goud",
                Email = "suraj.goud@eg.com",
                Phone = "+977 980000000001",
                AccountNumber = "1234567890",
                Balance = 12500.75m,
                CreatedAt = DateTime.Now.AddYears(-1)
            };
        }
    }
}
