using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyAppUI.Controllers
{
    public class DepositCurrencyController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICurrencyOperationsRepo _currencyOperationsRepo;
        private readonly ICurrencyTransactionRepo _currencyTransactionRepo;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;

        public DepositCurrencyController(
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

        public async Task<ActionResult> Deposit()
        {
            var currency = TempData["CurrencyTag"].ToString();
            var userDepositRequest = new UserDepositRequest
            {
                currencyTag = currency
            };

            return View(userDepositRequest);
        }

        [HttpPost]
        public async Task<ActionResult> UserDepositCurrency(UserDepositRequest userDeposit)
        {
            await _currencyOperationsRepo.DepositCurrency(userDeposit);
            return RedirectToAction(userDeposit.currencyTag, "UserAccount");
        }
    }
}