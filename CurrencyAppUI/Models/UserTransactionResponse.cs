namespace CurrencyAppUI.Models
{
    public class UserTransactionResponse
    {
        public string Amount { get; set; }
        public string CurrencyTag { get; set; }
        public DateTime TimeSent { get; set; }
    }
}