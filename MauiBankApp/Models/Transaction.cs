namespace MauiBankApp.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // "Credit", "Debit"
        public string Description { get; set; }
        public string Recipient { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // "Completed", "Pending", "Failed"
    }
}
