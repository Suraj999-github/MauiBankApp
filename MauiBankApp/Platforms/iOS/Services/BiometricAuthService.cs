using Foundation;
using LocalAuthentication;
using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
using UIKit;

namespace MauiBankApp.Platforms.iOS.Services
{
    public class BiometricAuthService : IBiometricAuthService
    {
        private const string BiometricSettingsKey = "biometric_settings";
        private const string BiometricEnrollmentKey = "biometric_enrollment";

        public async Task<bool> IsBiometricAvailableAsync()
        {
            try
            {
                var context = new LAContext();
                NSError error;
                var canEvaluate = context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error);

                return canEvaluate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking biometric availability: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsBiometricEnabledAsync()
        {
            try
            {
                var isEnabled = await SecureStorage.Default.GetAsync(BiometricSettingsKey);
                return isEnabled == "enabled";
            }
            catch
            {
                return false;
            }
        }

        public async Task<BiometricResponse> EnableBiometricAsync()
        {
            try
            {
                // First authenticate to enable
                var authResult = await AuthenticateAsync("Authenticate to enable biometric login");

                if (authResult.IsSuccess)
                {
                    // Store settings
                    await SecureStorage.Default.SetAsync(BiometricSettingsKey, "enabled");
                    await SecureStorage.Default.SetAsync(BiometricEnrollmentKey, DateTime.Now.ToString("o"));

                    return new BiometricResponse
                    {
                        IsSuccess = true,
                        Message = "Biometric authentication enabled successfully",
                        Status = BiometricStatus.Success,
                        EnrolledAt = DateTime.Now
                    };
                }
                else
                {
                    return authResult;
                }
            }
            catch (Exception ex)
            {
                return new BiometricResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to enable biometric: {ex.Message}",
                    Status = BiometricStatus.Error
                };
            }
        }

        public async Task<bool> DisableBiometricAsync()
        {
            try
            {
                SecureStorage.Default.Remove(BiometricSettingsKey);
                SecureStorage.Default.Remove(BiometricEnrollmentKey);
                SecureStorage.Default.Remove("biometric_user_id");
                SecureStorage.Default.Remove("biometric_user_email");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<BiometricResponse> AuthenticateAsync(string reason = "Verify your identity")
        {
            try
            {
                var context = new LAContext();
                NSError authError;

                // Check if biometric authentication is available
                var canEvaluate = context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out authError);

                if (!canEvaluate)
                {
                    return new BiometricResponse
                    {
                        IsSuccess = false,
                        Message = authError?.LocalizedDescription ?? "Biometric authentication not available",
                        Status = BiometricStatus.NotAvailable
                    };
                }

                // Perform authentication
                var taskCompletionSource = new TaskCompletionSource<BiometricResponse>();

                context.EvaluatePolicy(
                    LAPolicy.DeviceOwnerAuthenticationWithBiometrics,
                    reason,
                    (success, error) =>
                    {
                        if (success)
                        {
                            taskCompletionSource.SetResult(new BiometricResponse
                            {
                                IsSuccess = true,
                                Message = "Authentication successful",
                                Status = BiometricStatus.Success,
                                EnrolledAt = DateTime.Now
                            });
                        }
                        else
                        {
                            var status = BiometricStatus.Failed;
                            var message = "Authentication failed";

                            if (error != null)
                            {
                                //var laError = (LAError)(int)error.Code;

                                //status = laError switch
                                //{
                                //    LAError.UserCancel => BiometricStatus.UserCanceled,
                                //    LAError.SystemCancel => BiometricStatus.UserCanceled,
                                //    LAError.AuthenticationFailed => BiometricStatus.Failed,
                                //    LAError.PasscodeNotSet => BiometricStatus.NotEnrolled,
                                //    LAError.BiometryNotEnrolled => BiometricStatus.NotEnrolled,
                                //    LAError.BiometryNotAvailable => BiometricStatus.NotAvailable,
                                //    LAError.BiometryLockout => BiometricStatus.LockedOut,
                                //    _ => BiometricStatus.Error
                                //};

                                message = error.LocalizedDescription;
                            }

                            taskCompletionSource.SetResult(new BiometricResponse
                            {
                                IsSuccess = false,
                                Message = message,
                                Status = status
                            });
                        }
                    }
                );

                return await taskCompletionSource.Task;
            }
            catch (Exception ex)
            {
                return new BiometricResponse
                {
                    IsSuccess = false,
                    Message = $"Authentication error: {ex.Message}",
                    Status = BiometricStatus.Error
                };
            }
        }

        public async Task<string> GetBiometricTypeAsync()
        {
            try
            {
                var context = new LAContext();

                // iOS 11+ supports BiometryType
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    return context.BiometryType switch
                    {
                        LABiometryType.FaceId => "Face ID",
                        LABiometryType.TouchId => "Touch ID",
                        _ => "Biometric"
                    };
                }

                return "Touch ID";
            }
            catch
            {
                return "Biometric";
            }
        }

        public async Task<bool> StoreCredentialsForBiometricAsync(string userId, string email)
        {
            try
            {
                await SecureStorage.Default.SetAsync("biometric_user_id", userId);
                await SecureStorage.Default.SetAsync("biometric_user_email", email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(string userId, string email)> GetStoredCredentialsAsync()
        {
            try
            {
                var userId = await SecureStorage.Default.GetAsync("biometric_user_id");
                var email = await SecureStorage.Default.GetAsync("biometric_user_email");
                return (userId ?? string.Empty, email ?? string.Empty);
            }
            catch
            {
                return (string.Empty, string.Empty);
            }
        }
    }
}
