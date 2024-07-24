namespace CurrencyAppUI.Models
{
    public class UserOperationsResponse
    {
        public string AccountType { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyID { get; set; }
        public int UserID { get; set; }
    }
}