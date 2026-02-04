using MauiBankApp.Models;

namespace MauiBankApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<User>> GetUserProfileAsync();
        Task<ApiResponse<bool>> UpdateProfileAsync(User user);
        Task<ApiResponse<bool>> ChangePinAsync(string oldPin, string newPin);
    }
}
