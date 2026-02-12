#if ANDROID
using Android.OS;
using Android.Util;
using Android.Runtime; 
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using Java.Lang;
using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;


using BiometricManager = AndroidX.Biometric.BiometricManager;
using BiometricPrompt = AndroidX.Biometric.BiometricPrompt;

namespace MauiBankApp.Platforms.Android.Services
{
    public class BiometricAuthService : IBiometricAuthService
    {
        private const string BiometricEnabledKey = "biometric_enabled";
        private const string BiometricUserIdKey = "biometric_user_id";
        private const string BiometricUserEmailKey = "biometric_user_email";

        // Define constants for biometric error codes
        private const int BiometricErrorHwUnavailable = 1;
        private const int BiometricErrorUnableToProcess = 2;
        private const int BiometricErrorTimeout = 3;
        private const int BiometricErrorNoSpace = 4;
        private const int BiometricErrorCanceled = 5;
        private const int BiometricErrorLockout = 7;
        private const int BiometricErrorVendor = 8;
        private const int BiometricErrorLockoutPermanent = 9;
        private const int BiometricErrorUserCanceled = 10;
        private const int BiometricErrorNoBiometrics = 11;
        private const int BiometricErrorHwNotPresent = 12;
        private const int BiometricErrorNegativeButtonPressed = 13;
        private const int BiometricErrorNoDeviceCredential = 14;
        private const int BiometricErrorSecurityUpdateRequired = 15;

        // ---------------- AVAILABILITY ----------------

        public async Task<bool> IsBiometricAvailableAsync()
        {
            var activity = Platform.CurrentActivity;
            if (activity == null)
                return false;

            try
            {
                var biometricManager = BiometricManager.From(activity);

                var authenticators = BiometricManager.Authenticators.BiometricStrong
                    | BiometricManager.Authenticators.BiometricWeak;

                int canAuthenticate = biometricManager.CanAuthenticate(authenticators);

                return canAuthenticate == (int)BiometricManager.BiometricSuccess;
            }
            catch (System.Exception ex)
            {
                Log.Error("BiometricAuth", $"Error checking availability: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsBiometricEnabledAsync()
        {
            return await SecureStorage.Default.GetAsync(BiometricEnabledKey) == "true";
        }

        // ---------------- ENABLE / DISABLE ----------------

        public async Task<BiometricResponse> EnableBiometricAsync()
        {
            var auth = await AuthenticateAsync("Confirm biometric to enable login");

            if (!auth.IsSuccess)
                return auth;

            await SecureStorage.Default.SetAsync(BiometricEnabledKey, "true");

            auth.Message = "Biometric authentication enabled";
            return auth;
        }

        public Task<bool> DisableBiometricAsync()
        {
            SecureStorage.Default.Remove(BiometricEnabledKey);
            SecureStorage.Default.Remove(BiometricUserIdKey);
            SecureStorage.Default.Remove(BiometricUserEmailKey);
            return Task.FromResult(true);
        }

        // ---------------- AUTHENTICATION ----------------

        public async Task<BiometricResponse> AuthenticateAsync(string reason)
        {
            var activity = Platform.CurrentActivity;

            if (activity is not FragmentActivity fragmentActivity)
            {
                return new BiometricResponse
                {
                    IsSuccess = false,
                    Status = BiometricStatus.Error,
                    Message = "Invalid Android activity"
                };
            }

            var tcs = new TaskCompletionSource<BiometricResponse>();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var executor = ContextCompat.GetMainExecutor(fragmentActivity);

                   
                    var promptInfo = new BiometricPrompt.PromptInfo.Builder()
                        .SetTitle("Biometric Authentication")
                        .SetSubtitle(reason)
                        .SetNegativeButtonText("Cancel")
                        .SetAllowedAuthenticators(
                            BiometricManager.Authenticators.BiometricStrong
                            | BiometricManager.Authenticators.BiometricWeak)
                        .Build();

                    var biometricPrompt = new BiometricPrompt(
                        fragmentActivity,
                        executor,
                        new AuthCallback(tcs));

                    biometricPrompt.Authenticate(promptInfo);
                }
                catch (System.Exception ex)
                {
                    tcs.TrySetResult(new BiometricResponse
                    {
                        IsSuccess = false,
                        Status = BiometricStatus.Error,
                        Message = $"Failed to start biometric: {ex.Message}"
                    });
                }
            });

            return await tcs.Task;
        }

        // ---------------- CREDENTIAL STORAGE ----------------

        public Task<string> GetBiometricTypeAsync()
        {
            return Task.FromResult("Biometric (Fingerprint / Face)");
        }

        public async Task<bool> StoreCredentialsForBiometricAsync(string userId, string email)
        {
            await SecureStorage.Default.SetAsync(BiometricUserIdKey, userId);
            await SecureStorage.Default.SetAsync(BiometricUserEmailKey, email);
            return true;
        }

        public async Task<(string userId, string email)> GetStoredCredentialsAsync()
        {
            var userId = await SecureStorage.Default.GetAsync(BiometricUserIdKey) ?? string.Empty;
            var email = await SecureStorage.Default.GetAsync(BiometricUserEmailKey) ?? string.Empty;
            return (userId, email);
        }

        // ---------------- CALLBACK ----------------

        private sealed class AuthCallback : BiometricPrompt.AuthenticationCallback
        {
            private readonly TaskCompletionSource<BiometricResponse> _tcs;

            public AuthCallback(TaskCompletionSource<BiometricResponse> tcs)
            {
                _tcs = tcs;
            }

            public override void OnAuthenticationSucceeded(
                BiometricPrompt.AuthenticationResult result)
            {
                _tcs.TrySetResult(new BiometricResponse
                {
                    IsSuccess = true,
                    Status = BiometricStatus.Success,
                    Message = "Authentication successful"
                });
            }

            public override void OnAuthenticationFailed()
            {
                // Don't complete the task on failed attempts - user can try again
                // Just log or show a message through other means
            }

            public override void OnAuthenticationError(
                [GeneratedEnum] int errorCode,
                ICharSequence errString)
            {
                var message = errString?.ToString() ?? "Authentication error";
                var status = MapErrorCodeToStatus(errorCode);

                _tcs.TrySetResult(new BiometricResponse
                {
                    IsSuccess = false,
                    Status = status,
                    Message = message
                });
            }

            private BiometricStatus MapErrorCodeToStatus(int errorCode)
            {
                switch (errorCode)
                {
                    // User cancellation
                    case BiometricErrorNegativeButtonPressed:
                    case BiometricErrorUserCanceled:
                    case BiometricErrorCanceled:
                        return BiometricStatus.UserCanceled;

                    // Lockout states
                    case BiometricErrorLockout:
                    case BiometricErrorLockoutPermanent:
                        return BiometricStatus.LockedOut;

                    // No biometrics enrolled
                    case BiometricErrorNoBiometrics:
                        return BiometricStatus.NotEnrolled;

                    // Hardware issues
                    case BiometricErrorHwUnavailable:
                    case BiometricErrorHwNotPresent:
                        return BiometricStatus.NotAvailable;

                    // Timeout
                    case BiometricErrorTimeout:
                        return BiometricStatus.Timeout;

                    // Default error
                    default:
                        return BiometricStatus.Error;
                }
            }
        }
    }
}
#endif