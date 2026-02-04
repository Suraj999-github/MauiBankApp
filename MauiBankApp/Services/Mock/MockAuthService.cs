using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;
using MauiBankApp.Utils;

namespace MauiBankApp.Services.Mock
{
    public class MockAuthService : IAuthService
    {
        private const string MockToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        private readonly User _mockUser = new()
        {
            Id = "1",
            Name = "Suraj Goud",
            Email = "suraj.goud@eg.com",
            Phone = "+977 980000000001",
            AccountNumber = "1234567890",
            Balance = 12500.75m,
            CreatedAt = DateTime.Now.AddYears(-1)
        };

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            // Simulate API delay
            await Task.Delay(1000);

            // Mock validation
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email and password are required"
                };
            }

            // Mock successful login for demo
            if (email.Contains("@") && password.Length >= 6)
            {
                // Store token securely
                await SecureStorageHelper.SetTokenAsync(MockToken);

                return new AuthResponse
                {
                    IsSuccess = true,
                    Token = MockToken,
                    Message = "Login successful",
                    User = _mockUser
                };
            }

            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid credentials"
            };
        }

        public async Task<bool> LogoutAsync()
        {
            SecureStorage.Default.Remove("auth_token");
            return true;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync("auth_token") ?? string.Empty;
        }
    }
}
