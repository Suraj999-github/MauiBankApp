using MauiBankApp.Models;

namespace MauiBankApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<string> GetTokenAsync();
    }
}
