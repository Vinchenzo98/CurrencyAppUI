namespace CurrencyAppUI.Models
{
    public class UserTransactionResponse
    {
        public decimal? Amount { get; set; }
        public string? CurrencyTag { get; set; }
        public DateTime TimeSent { get; set; }
    }
}