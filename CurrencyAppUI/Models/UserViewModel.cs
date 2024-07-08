namespace CurrencyAppUI.Models
{
    public class UserViewModel
    {
        public List<AccountTypeResponse> AllAccounts { get; set; }
        public List<UserTransactionResponse> AllTransactions { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public List<UserTransactionResponse> InTransactions { get; set; }

        public string LastName { get; set; }
        public string Mobile { get; set; }
        public List<UserTransactionResponse> OutTransactions { get; set; }
        public string Token { get; set; }
        public string UserTag { get; set; }
    }
}