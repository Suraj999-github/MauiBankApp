using MauiBankApp.Models;
using MauiBankApp.Services.Interfaces;

namespace MauiBankApp.Services.Mock
{
    public class MockTransactionService : ITransactionService
    {
        private readonly List<Transaction> _mockTransactions = new()
        {
            new Transaction { Id = "1", Amount = 500.00m, Type = "Credit", Description = "Salary Deposit", Recipient = "ABC Corp", Date = DateTime.Now.AddDays(-1), Status = "Completed" },
            new Transaction { Id = "2", Amount = 120.50m, Type = "Debit", Description = "Grocery Store", Recipient = "Fresh Mart", Date = DateTime.Now.AddDays(-2), Status = "Completed" },
            new Transaction { Id = "3", Amount = 45.00m, Type = "Debit", Description = "Netflix Subscription", Recipient = "Netflix", Date = DateTime.Now.AddDays(-3), Status = "Completed" },
            new Transaction { Id = "4", Amount = 1000.00m, Type = "Credit", Description = "Freelance Payment", Recipient = "Client XYZ", Date = DateTime.Now.AddDays(-5), Status = "Completed" },
            new Transaction { Id = "5", Amount = 250.00m, Type = "Debit", Description = "Electricity Bill", Recipient = "Power Company", Date = DateTime.Now.AddDays(-7), Status = "Completed" },
            new Transaction { Id = "6", Amount = 75.30m, Type = "Debit", Description = "Restaurant", Recipient = "Food Palace", Date = DateTime.Now.AddDays(-8), Status = "Completed" },
            new Transaction { Id = "7", Amount = 300.00m, Type = "Credit", Description = "Refund", Recipient = "Amazon", Date = DateTime.Now.AddDays(-10), Status = "Completed" },
            new Transaction { Id = "8", Amount = 50.00m, Type = "Debit", Description = "Mobile Top-up", Recipient = "Verizon", Date = DateTime.Now.AddDays(-12), Status = "Completed" },
        };

        public async Task<ApiResponse<decimal>> GetBalanceAsync()
        {
            await Task.Delay(500); // Simulate API delay
            return new ApiResponse<decimal>
            {
                IsSuccess = true,
                Data = 12500.75m,
                Message = "Balance retrieved successfully"
            };
        }

        public async Task<ApiResponse<List<Transaction>>> GetTransactionsAsync(int limit = 10)
        {
            await Task.Delay(500);
            return new ApiResponse<List<Transaction>>
            {
                IsSuccess = true,
                Data = _mockTransactions.Take(limit).ToList(),
                Message = "Transactions retrieved successfully"
            };
        }

        public async Task<ApiResponse<bool>> SendMoneyAsync(decimal amount, string recipientAccount)
        {
            await Task.Delay(1000);

            if (amount <= 0 || string.IsNullOrEmpty(recipientAccount))
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Invalid amount or recipient"
                };
            }

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = $"Successfully sent ${amount} to account {recipientAccount}"
            };
        }

        public async Task<ApiResponse<bool>> TopUpMobileAsync(string operatorName, decimal amount, string phoneNumber)
        {
            await Task.Delay(800);
            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = $"Successfully topped up ${amount} to {phoneNumber} ({operatorName})"
            };
        }

        public async Task<ApiResponse<bool>> PayWaterBillAsync(string consumerNumber, decimal amount)
        {
            await Task.Delay(800);
            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = $"Water bill paid successfully for consumer #{consumerNumber}"
            };
        }

        public async Task<ApiResponse<bool>> PayElectricityBillAsync(string accountNumber, decimal amount)
        {
            await Task.Delay(800);
            return new ApiResponse<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = $"Electricity bill paid successfully for account #{accountNumber}"
            };
        }
    }
}
