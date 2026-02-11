using MauiBankApp.Models;

namespace MauiBankApp.Services.Interfaces
{
    public interface IBiometricAuthService
    {
        /// <summary>
        /// Check if biometric authentication is available on the device
        /// </summary>
        Task<bool> IsBiometricAvailableAsync();

        /// <summary>
        /// Check if biometric is enrolled/enabled for the user
        /// </summary>
        Task<bool> IsBiometricEnabledAsync();

        /// <summary>
        /// Enable biometric authentication for the user
        /// </summary>
        Task<BiometricResponse> EnableBiometricAsync();

        /// <summary>
        /// Disable biometric authentication for the user
        /// </summary>
        Task<bool> DisableBiometricAsync();

        /// <summary>
        /// Authenticate user using biometric
        /// </summary>
        Task<BiometricResponse> AuthenticateAsync(string reason = "Verify your identity");

        /// <summary>
        /// Get biometric authentication type available (Fingerprint, FaceID, etc.)
        /// </summary>
        Task<string> GetBiometricTypeAsync();

        /// <summary>
        /// Store user credentials for biometric login
        /// </summary>
        Task<bool> StoreCredentialsForBiometricAsync(string userId, string email);

        /// <summary>
        /// Get stored user credentials for biometric login
        /// </summary>
        Task<(string userId, string email)> GetStoredCredentialsAsync();
    }
}
