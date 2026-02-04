namespace MauiBankApp.Utils
{
    public static class SecureStorageHelper
    {
        private const string AuthTokenKey = "auth_token";
        private const string UserIdKey = "user_id";

        public static async Task SetTokenAsync(string token)
        {
            await SecureStorage.Default.SetAsync(AuthTokenKey, token);
        }

        public static async Task<string> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(AuthTokenKey);
        }

        public static async Task ClearTokenAsync()
        {
            SecureStorage.Default.Remove(AuthTokenKey);
        }
    }
}
