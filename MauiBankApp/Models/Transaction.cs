namespace MauiBankApp.Models
{
    //public class Transaction
    //{
    //    public string Id { get; set; }
    //    public decimal Amount { get; set; }
    //    public string Type { get; set; } // "Credit", "Debit"
    //    public string Description { get; set; }
    //    public string Recipient { get; set; }
    //    public DateTime Date { get; set; }
    //    public string Status { get; set; } // "Completed", "Pending", "Failed"
    //}
    public class Transaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // Credit or Debit
        public string Description { get; set; }
        public string Recipient { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        // Helper properties for UI
        public string Icon => Type?.ToLower() switch
        {
            "credit" => "credit.png",
            "debit" => "debit.png",
            _ => "transaction.png"
        };

        public Color AmountColor => Type?.ToLower() switch
        {
            "credit" => Colors.Green,
            "debit" => Colors.Red,
            _ => Colors.Gray
        };

        public string FormattedAmount => Type?.ToLower() switch
        {
            "debit" => $"-${Math.Abs(Amount):F2}",
            _ => $"+${Amount:F2}"
        };
    }
}
