using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
using System.Text.Json;

namespace MauiBankApp.Services.Mock
{
    public class MockBiometricAuthService : IBiometricAuthService
    {
        private const string BiometricSettingsKey = "biometric_settings";
        private const string BiometricEnrollmentKey = "biometric_enrollment";
        private readonly string _cacheFilePath;

        public MockBiometricAuthService()
        {
            // Use cache directory for internal storage
            _cacheFilePath = Path.Combine(FileSystem.CacheDirectory, "biometric_data.json");
            InitializeCacheFile();
        }

        private void InitializeCacheFile()
        {
            if (!File.Exists(_cacheFilePath))
            {
                var initialData = new BiometricData
                {
                    IsEnabled = false,
                    EnrolledAt = null,
                    LastAuthenticatedAt = null,
                    BiometricType = "Fingerprint"
                };
                SaveBiometricData(initialData);
            }
        }

        private BiometricData LoadBiometricData()
        {
            try
            {
                if (File.Exists(_cacheFilePath))
                {
                    var json = File.ReadAllText(_cacheFilePath);
                    return JsonSerializer.Deserialize<BiometricData>(json) ?? new BiometricData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading biometric data: {ex.Message}");
            }

            return new BiometricData();
        }

        private void SaveBiometricData(BiometricData data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(_cacheFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving biometric data: {ex.Message}");
            }
        }

        public async Task<bool> IsBiometricAvailableAsync()
        {
            // Simulate API delay
            await Task.Delay(300);

            // Mock: Biometric is always available in this mock service
            // In production, this would check actual device capabilities
            return true;
        }

        public async Task<bool> IsBiometricEnabledAsync()
        {
            await Task.Delay(200);

            var data = LoadBiometricData();
            return data.IsEnabled;
        }

        public async Task<BiometricResponse> EnableBiometricAsync()
        {
            // Simulate fingerprint scanning delay
            await Task.Delay(800);

            try
            {
                // Load current data
                var data = LoadBiometricData();

                // Simulate biometric enrollment - scanning fingerprint
                data.IsEnabled = true;
                data.EnrolledAt = DateTime.Now;
                data.BiometricType = "Fingerprint";

                // Save to cache
                SaveBiometricData(data);

                // Also store in secure storage for quick access
                await SecureStorage.Default.SetAsync(BiometricSettingsKey, "enabled");
                await SecureStorage.Default.SetAsync(BiometricEnrollmentKey, DateTime.Now.ToString("o"));

                return new BiometricResponse
                {
                    IsSuccess = true,
                    Message = "Fingerprint scanned and saved successfully",
                    Status = BiometricStatus.Success,
                    EnrolledAt = data.EnrolledAt
                };
            }
            catch (Exception ex)
            {
                return new BiometricResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to scan fingerprint: {ex.Message}",
                    Status = BiometricStatus.Error
                };
            }
        }

        /// <summary>
        /// Store user credentials for biometric login
        /// </summary>
        public async Task<bool> StoreCredentialsForBiometricAsync(string userId, string email)
        {
            try
            {
                await SecureStorage.Default.SetAsync("biometric_user_id", userId);
                await SecureStorage.Default.SetAsync("biometric_user_email", email);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing biometric credentials: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get stored user credentials
        /// </summary>
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

        public async Task<bool> DisableBiometricAsync()
        {
            // Simulate API delay
            await Task.Delay(500);

            try
            {
                // Load and update data
                var data = LoadBiometricData();
                data.IsEnabled = false;
                data.EnrolledAt = null;
                data.LastAuthenticatedAt = null;

                // Save to cache
                SaveBiometricData(data);

                // Remove from secure storage
                SecureStorage.Default.Remove(BiometricSettingsKey);
                SecureStorage.Default.Remove(BiometricEnrollmentKey);
                SecureStorage.Default.Remove("biometric_user_id");
                SecureStorage.Default.Remove("biometric_user_email");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disabling biometric: {ex.Message}");
                return false;
            }
        }

        public async Task<BiometricResponse> AuthenticateAsync(string reason = "Verify your identity")
        {
            // Simulate biometric authentication delay
            await Task.Delay(1500);

            var data = LoadBiometricData();

            if (!data.IsEnabled)
            {
                return new BiometricResponse
                {
                    IsSuccess = false,
                    Message = "Biometric authentication is not enabled",
                    Status = BiometricStatus.NotEnrolled
                };
            }

            // Mock: Simulate 90% success rate for authentication
            var random = new Random();
            var isSuccess = random.Next(100) < 90;

            if (isSuccess)
            {
                // Update last authenticated time
                data.LastAuthenticatedAt = DateTime.Now;
                SaveBiometricData(data);

                return new BiometricResponse
                {
                    IsSuccess = true,
                    Message = "Authentication successful",
                    Status = BiometricStatus.Success,
                    EnrolledAt = data.EnrolledAt
                };
            }
            else
            {
                return new BiometricResponse
                {
                    IsSuccess = false,
                    Message = "Authentication failed. Please try again.",
                    Status = BiometricStatus.Failed
                };
            }
        }

        public async Task<string> GetBiometricTypeAsync()
        {
            await Task.Delay(100);

            var data = LoadBiometricData();
            return data.BiometricType;
        }

        // Internal data model for cache storage
        private class BiometricData
        {
            public bool IsEnabled { get; set; }
            public DateTime? EnrolledAt { get; set; }
            public DateTime? LastAuthenticatedAt { get; set; }
            public string BiometricType { get; set; } = "Fingerprint";
        }
    }
    //public class MockBiometricAuthService : IBiometricAuthService
    //{
    //    private const string BiometricSettingsKey = "biometric_settings";
    //    private const string BiometricEnrollmentKey = "biometric_enrollment";
    //    private readonly string _cacheFilePath;

    //    public MockBiometricAuthService()
    //    {
    //        // Use cache directory for internal storage
    //        _cacheFilePath = Path.Combine(FileSystem.CacheDirectory, "biometric_data.json");
    //        InitializeCacheFile();
    //    }

    //    private void InitializeCacheFile()
    //    {
    //        if (!File.Exists(_cacheFilePath))
    //        {
    //            var initialData = new BiometricData
    //            {
    //                IsEnabled = false,
    //                EnrolledAt = null,
    //                LastAuthenticatedAt = null,
    //                BiometricType = "Fingerprint"
    //            };
    //            SaveBiometricData(initialData);
    //        }
    //    }

    //    private BiometricData LoadBiometricData()
    //    {
    //        try
    //        {
    //            if (File.Exists(_cacheFilePath))
    //            {
    //                var json = File.ReadAllText(_cacheFilePath);
    //                return JsonSerializer.Deserialize<BiometricData>(json) ?? new BiometricData();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error loading biometric data: {ex.Message}");
    //        }

    //        return new BiometricData();
    //    }

    //    private void SaveBiometricData(BiometricData data)
    //    {
    //        try
    //        {
    //            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
    //            {
    //                WriteIndented = true
    //            });
    //            File.WriteAllText(_cacheFilePath, json);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error saving biometric data: {ex.Message}");
    //        }
    //    }

    //    public async Task<bool> IsBiometricAvailableAsync()
    //    {
    //        // Simulate API delay
    //        await Task.Delay(300);

    //        // Mock: Biometric is always available in this mock service
    //        // In production, this would check actual device capabilities
    //        return true;
    //    }

    //    public async Task<bool> IsBiometricEnabledAsync()
    //    {
    //        await Task.Delay(200);

    //        var data = LoadBiometricData();
    //        return data.IsEnabled;
    //    }

    //    public async Task<BiometricResponse> EnableBiometricAsync()
    //    {
    //        // Simulate API delay
    //        await Task.Delay(800);

    //        try
    //        {
    //            // Load current data
    //            var data = LoadBiometricData();

    //            // Simulate biometric enrollment
    //            data.IsEnabled = true;
    //            data.EnrolledAt = DateTime.Now;
    //            data.BiometricType = "Fingerprint";

    //            // Save to cache
    //            SaveBiometricData(data);

    //            // Also store in secure storage for quick access
    //            await SecureStorage.Default.SetAsync(BiometricSettingsKey, "enabled");
    //            await SecureStorage.Default.SetAsync(BiometricEnrollmentKey, DateTime.Now.ToString("o"));

    //            return new BiometricResponse
    //            {
    //                IsSuccess = true,
    //                Message = "Fingerprint authentication enabled successfully",
    //                Status = BiometricStatus.Success,
    //                EnrolledAt = data.EnrolledAt
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new BiometricResponse
    //            {
    //                IsSuccess = false,
    //                Message = $"Failed to enable biometric: {ex.Message}",
    //                Status = BiometricStatus.Error
    //            };
    //        }
    //    }

    //    public async Task<bool> DisableBiometricAsync()
    //    {
    //        // Simulate API delay
    //        await Task.Delay(500);

    //        try
    //        {
    //            // Load and update data
    //            var data = LoadBiometricData();
    //            data.IsEnabled = false;
    //            data.EnrolledAt = null;
    //            data.LastAuthenticatedAt = null;

    //            // Save to cache
    //            SaveBiometricData(data);

    //            // Remove from secure storage
    //            SecureStorage.Default.Remove(BiometricSettingsKey);
    //            SecureStorage.Default.Remove(BiometricEnrollmentKey);

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error disabling biometric: {ex.Message}");
    //            return false;
    //        }
    //    }

    //    public async Task<BiometricResponse> AuthenticateAsync(string reason = "Verify your identity")
    //    {
    //        // Simulate biometric authentication delay
    //        await Task.Delay(1500);

    //        var data = LoadBiometricData();

    //        if (!data.IsEnabled)
    //        {
    //            return new BiometricResponse
    //            {
    //                IsSuccess = false,
    //                Message = "Biometric authentication is not enabled",
    //                Status = BiometricStatus.NotEnrolled
    //            };
    //        }

    //        // Mock: Simulate 90% success rate for authentication
    //        var random = new Random();
    //        var isSuccess = random.Next(100) < 90;

    //        if (isSuccess)
    //        {
    //            // Update last authenticated time
    //            data.LastAuthenticatedAt = DateTime.Now;
    //            SaveBiometricData(data);

    //            return new BiometricResponse
    //            {
    //                IsSuccess = true,
    //                Message = "Authentication successful",
    //                Status = BiometricStatus.Success,
    //                EnrolledAt = data.EnrolledAt
    //            };
    //        }
    //        else
    //        {
    //            return new BiometricResponse
    //            {
    //                IsSuccess = false,
    //                Message = "Authentication failed. Please try again.",
    //                Status = BiometricStatus.Failed
    //            };
    //        }
    //    }

    //    public async Task<string> GetBiometricTypeAsync()
    //    {
    //        await Task.Delay(100);

    //        var data = LoadBiometricData();
    //        return data.BiometricType;
    //    }

    //    // Internal data model for cache storage
    //    private class BiometricData
    //    {
    //        public bool IsEnabled { get; set; }
    //        public DateTime? EnrolledAt { get; set; }
    //        public DateTime? LastAuthenticatedAt { get; set; }
    //        public string BiometricType { get; set; } = "Fingerprint";
    //    }
    //}
}
