namespace MauiBankApp.Models
{
    public class BiometricResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public BiometricStatus Status { get; set; }
        public DateTime? EnrolledAt { get; set; }
    }

    public enum BiometricStatus
    {
        Success,
        NotAvailable,
        NotEnrolled,
        Failed,
        Cancelled,
        Locked,
        Error
    }
}
