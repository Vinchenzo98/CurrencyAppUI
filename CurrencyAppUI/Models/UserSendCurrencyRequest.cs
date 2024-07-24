namespace CurrencyAppUI.Models
{
    public class UserSendCurrencyRequest
    {
        public decimal amount { get; set; }
        public string currencyTag { get; set; }
        public string userTag { get; set; }
    }
}