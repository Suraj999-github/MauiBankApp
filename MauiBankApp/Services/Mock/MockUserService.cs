using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;

namespace MauiBankApp.Services.Mock
{
    public class MockUserService : IUserService
    {
        private User _mockUser = new()
        {
            Id = "1",
            Name = "Suraj Goud",
            Email = "suraj.goud@eg.com",
            Phone = "+977 980000000001",
            AccountNumber = "1234567890",
            Balance = 12500.75m,
            CreatedAt = DateTime.Now.AddYears(-1)
        };

        private string _mockPin = "1234";

        public async Task<ApiResponse<User>> GetUserProfileAsync()
        {
            // Simulate API delay
            await Task.Delay(800);

            return new ApiResponse<User>
            {
                IsSuccess = true,
                Data = _mockUser,
                Message = "User profile fetched successfully",
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<bool>> UpdateProfileAsync(User user)
        {
            await Task.Delay(800);

            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Invalid user data",
                    StatusCode = 400
                };
            }

            // Update mock data
            _mockUser.Name = user.Name;
            _mockUser.Email = user.Email;
            _mockUser.Phone = user.Phone;

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = "Profile updated successfully",
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<bool>> ChangePinAsync(string oldPin, string newPin)
        {
            await Task.Delay(800);

            if (string.IsNullOrWhiteSpace(oldPin) || string.IsNullOrWhiteSpace(newPin))
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "PIN values are required",
                    StatusCode = 400
                };
            }

            if (oldPin != _mockPin)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Old PIN is incorrect",
                    StatusCode = 401
                };
            }

            if (newPin.Length < 4)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "PIN must be at least 4 digits",
                    StatusCode = 400
                };
            }

            _mockPin = newPin;

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = "PIN changed successfully",
                StatusCode = 200
            };
        }
    }
}
