using CurrencyAppUI.Models;

namespace CurrencyAppUI.Repo.Interface
{
    public interface ICurrencyOperationsRepo
    {
        Task<UserOperationsResponse> DepositCurrency(UserDepositRequest userDepositRequest);

        Task<UserOperationsResponse> ExchangeCurrency(UserCurrencyExchangeRequest userCurrencyExchangeRequest);

        Task<UserOperationsResponse> SendCurrency(UserSendCurrencyRequest sendCurrencyRequest);

        Task<UserOperationsResponse> WithdrawCurrency(UserWithdrawRequest userWithdrawRequest);
    }
}