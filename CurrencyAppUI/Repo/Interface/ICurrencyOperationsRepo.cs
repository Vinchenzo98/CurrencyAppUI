using CurrencyAppUI.Models;

namespace CurrencyAppUI.Repo.Interface
{
    public interface ICurrencyOperationsRepo
    {
        Task<UserOperationsResponse> DepositCurrency(UserDepositRequest userDepositRequest);

        Task<UserOperationsResponse> ExchangeCurrency(string amount, string baseCurrency, string targetCurrency);

        Task<UserOperationsResponse> SendCurrency(UserSendCurrencyRequest sendCurrencyRequest);

        Task<UserOperationsResponse> WithdrawCurrency(UserWithdrawRequest userWithdrawRequest);
    }
}