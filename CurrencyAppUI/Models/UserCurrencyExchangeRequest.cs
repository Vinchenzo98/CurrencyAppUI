namespace CurrencyAppUI.Models
{
    public class UserCurrencyExchangeRequest
    {
        public string amount { get; set; }
        public string baseCurrency { get; set; }
        public string targetCurrency { get; set; }
    }
}