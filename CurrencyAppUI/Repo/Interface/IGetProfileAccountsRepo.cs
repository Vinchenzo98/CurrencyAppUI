using CurrencyAppUI.Models;

namespace CurrencyAppUI.Repo.Interface
{
    public interface IGetProfileAccountsRepo
    {
        Task<List<AccountTypeResponse>> GetAccountTypeRepo();
        Task<AccountTypeResponse> CreateAccountRepo(AccountTypeRequest accountTypeRequest);
    }
}