namespace MauiBankApp.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string AccountNumber { get; set; }

        public string UserId { get; set; }
        public string AccountType { get; set; }   // Savings, Current

        public decimal Balance { get; set; }
        public string Currency { get; set; }

        public bool IsActive { get; set; }
        public DateTime OpenedAt { get; set; }
    }

}
