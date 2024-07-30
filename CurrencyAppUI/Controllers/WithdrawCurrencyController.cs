using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyAppUI.Controllers
{
    public class WithdrawCurrencyController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICurrencyOperationsRepo _currencyOperationsRepo;
        private readonly ICurrencyTransactionRepo _currencyTransactionRepo;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;

        public WithdrawCurrencyController(
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

        public async Task<ActionResult> Withdraw()
        {
            var currency = TempData["CurrencyTag"].ToString();
            var userWithdrawRequest = new UserWithdrawRequest
            {
                currencyTag = currency
            };

            return View(userWithdrawRequest);
        }

        [HttpPost]
        public async Task<ActionResult> UserWithdrawCurrency(UserWithdrawRequest userWithdraw)
        {
            await _currencyOperationsRepo.WithdrawCurrency(userWithdraw);
            return RedirectToAction(userWithdraw.currencyTag, "UserAccount");
        }
    }
}