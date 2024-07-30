using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAppUI.Controllers
{
    public class ExchangeCurrencyController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICurrencyOperationsRepo _currencyOperationsRepo;
        private readonly ICurrencyTransactionRepo _currencyTransactionRepo;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;

        public ExchangeCurrencyController(
                IHttpClientFactory httpClientFactory,
                IHttpContextAccessor contextAccessor,
                ICurrencyOperationsRepo currencyOperationsRepo,
                ICurrencyTransactionRepo currencyTransactionRepo,
                IGetProfileAccountsRepo getProfileAccountsRepo
            )
        {
            _contextAccessor = contextAccessor;
            _client = httpClientFactory.CreateClient("CurrencyAppUIClient");
            _currencyOperationsRepo = currencyOperationsRepo;
            _currencyTransactionRepo = currencyTransactionRepo;
            _profileAccountsRepo = getProfileAccountsRepo;
        }

        public async Task<ActionResult> Exchange()
        {
            var currency = TempData["CurrencyTag"].ToString();
            var userExchangeRequest = new UserCurrencyExchangeRequest
            {
                baseCurrency = currency
            };

            var getAllAccounts = await _profileAccountsRepo.GetAccountTypeRepo();

            var exchangeList = new List<AccountTypeResponse>();

            foreach (var account in getAllAccounts)
            {

                var exchangedCurrencyDTO = new AccountTypeResponse
                {
                    AccountType = account.AccountType
                };
                  
                var currentCurrencyAccount = exchangeList.FirstOrDefault(ct => ct.AccountType != currency);
                if (currentCurrencyAccount == null)
                {
                    exchangeList.Add(exchangedCurrencyDTO);
                }
            }
          
            var exchangedCurrencyData = ViewData["ExchangeAccount"] = exchangeList;
            TempData["ExchangeAccount"] = exchangedCurrencyData;

            return View(userExchangeRequest);
        }

        [HttpPost]
        public async Task<ActionResult> UserExchangeCurrency(UserCurrencyExchangeRequest userExchange)
        {
            await _currencyOperationsRepo.ExchangeCurrency(userExchange);
            return RedirectToAction(userExchange.baseCurrency, "UserAccount");
        }
    }
}
