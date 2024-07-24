using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyAppUI.Controllers
{
    public class SendCurrencyController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICurrencyOperationsRepo _currencyOperationsRepo;
        private readonly ICurrencyTransactionRepo _currencyTransactionRepo;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;

        public SendCurrencyController(
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

        [HttpPost]
        public async Task<ActionResult> Send(UserSendCurrencyRequest userSendCurrency)
        {
            var userModelJson = _contextAccessor.HttpContext.Session.GetString("UserModel");
            if (string.IsNullOrEmpty(userModelJson))
            {
                return RedirectToAction("Profile", "UserProfile");
            }

            var getCurrencyTag = await _profileAccountsRepo.GetAccountTypeRepo();

            if (getCurrencyTag == null)
            {
                return RedirectToAction("Profile", "UserProfile");
            }

            //ViewData["Title"] = "Currency";

            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(userModelJson);

            await _currencyOperationsRepo.SendCurrency(userSendCurrency);

            var currencyTag = getCurrencyTag.FirstOrDefault(ct => ct.AccountType == userSendCurrency.currencyTag);

            var allTransactions = await _currencyTransactionRepo.GetAllTransactions(currencyTag.AccountType);
            var inTransactions = await _currencyTransactionRepo.GetInTransactions(currencyTag.AccountType);
            var outTransactions = await _currencyTransactionRepo.GetOutTransactions(currencyTag.AccountType);

            userViewModel.AllTransactions = allTransactions;
            userViewModel.InTransactions = inTransactions;
            userViewModel.OutTransactions = outTransactions;

            return View(userViewModel);
        }
    }
}