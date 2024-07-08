using CurrencyAppUI.Models;

namespace CurrencyAppUI.Repo.Interface
{
    public interface ICurrencyTransactionRepo
    {
        Task<List<UserTransactionResponse>> GetAllTransactions(string currencyTag);

        Task<List<UserTransactionResponse>> GetInTransactions(string currencyTag);

        Task<List<UserTransactionResponse>> GetOutTransactions(string currencyTag);
    }
}