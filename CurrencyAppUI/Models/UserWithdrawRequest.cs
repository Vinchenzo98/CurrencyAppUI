namespace CurrencyAppUI.Models
{
    public class UserWithdrawRequest
    {
        public decimal Amount { get; set; }
        public string currencyTag { get; set; }
    }
}