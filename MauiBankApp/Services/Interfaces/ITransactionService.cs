using MauiBankApp.Models;

namespace MauiBankApp.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse<decimal>> GetBalanceAsync();
        Task<ApiResponse<List<Transaction>>> GetTransactionsAsync(int limit = 10);
        Task<ApiResponse<bool>> SendMoneyAsync(decimal amount, string recipientAccount);
        Task<ApiResponse<bool>> TopUpMobileAsync(string operatorName, decimal amount, string phoneNumber);
        Task<ApiResponse<bool>> PayWaterBillAsync(string consumerNumber, decimal amount);
        Task<ApiResponse<bool>> PayElectricityBillAsync(string accountNumber, decimal amount);
    }
}
