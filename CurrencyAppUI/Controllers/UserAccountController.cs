using CurrencyAppUI.Models;
using CurrencyAppUI.Repo;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyAppUI.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly HttpClient _client;
        private readonly ICurrencyTransactionRepo _currencyTransactionRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;
        

        public UserAccountController(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor,
            ICurrencyTransactionRepo currencyTransactionRepo,
            IGetProfileAccountsRepo getProfileAccountsRepo
         )
        {
            _httpContextAccessor = httpContextAccessor;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
            _currencyTransactionRepo = currencyTransactionRepo;
            _profileAccountsRepo = getProfileAccountsRepo;
        }

        public async Task<ActionResult> CreateAccount()
        {
            var model = new AccountTypeRequest();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreationRequest(AccountTypeRequest accountTypeRequest)
        {
            await _profileAccountsRepo.CreateAccountRepo(accountTypeRequest);
            return RedirectToAction("Profile", "UserProfile");
        }

        [HttpGet]
        public async Task<ActionResult> EUR()
        {
            var userModelJson = _httpContextAccessor.HttpContext.Session.GetString("UserModel");
            if (string.IsNullOrEmpty(userModelJson))
            {
                return RedirectToAction("Profile", "UserProfile");
            }
            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(userModelJson);

            var accountName = ViewData["Title"] = "EUR";

            var getCurrencyTag = await _profileAccountsRepo.GetAccountTypeRepo();

            if (getCurrencyTag == null)
            {
                return RedirectToAction("Profile", "UserProfile");
            }

            var currencyTag = getCurrencyTag.FirstOrDefault(ct => ct.AccountType == accountName.ToString());

            var allTransactions = await _currencyTransactionRepo.GetAllTransactions(currencyTag.AccountType);
            var inTransactions = await _currencyTransactionRepo.GetInTransactions(currencyTag.AccountType);
            var outTransactions = await _currencyTransactionRepo.GetOutTransactions(currencyTag.AccountType);

            userViewModel.AllAccounts = getCurrencyTag;
            userViewModel.AllTransactions = allTransactions;
            userViewModel.InTransactions = inTransactions;
            userViewModel.OutTransactions = outTransactions;

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> GBP()
        {
            var userModelJson = _httpContextAccessor.HttpContext.Session.GetString("UserModel");
            if (string.IsNullOrEmpty(userModelJson))
            {
                return RedirectToAction("Profile", "UserProfile");
            }
            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(userModelJson);

            var accountName = ViewData["Title"] = "GBP";

            var getCurrencyTag = await _profileAccountsRepo.GetAccountTypeRepo();

            if (getCurrencyTag == null)
            {
                return RedirectToAction("Profile", "UserProfile");
            }

            var currencyTag = getCurrencyTag.FirstOrDefault(ct => ct.AccountType == accountName.ToString());

            var allTransactions = await _currencyTransactionRepo.GetAllTransactions(currencyTag.AccountType);
            var inTransactions = await _currencyTransactionRepo.GetInTransactions(currencyTag.AccountType);
            var outTransactions = await _currencyTransactionRepo.GetOutTransactions(currencyTag.AccountType);

            userViewModel.AllAccounts = getCurrencyTag;
            userViewModel.AllTransactions = allTransactions;
            userViewModel.InTransactions = inTransactions;
            userViewModel.OutTransactions = outTransactions;

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> USD()
        {
            var userModelJson = _httpContextAccessor.HttpContext.Session.GetString("UserModel");
            if (string.IsNullOrEmpty(userModelJson))
            {
                return RedirectToAction("Profile", "UserProfile");
            }
            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(userModelJson);

            var accountName = ViewData["Title"] = "USD";

            var getCurrencyTag = await _profileAccountsRepo.GetAccountTypeRepo();

            if (getCurrencyTag == null)
            {
                return RedirectToAction("Profile", "UserProfile");
            }

            var currencyTag = getCurrencyTag.FirstOrDefault(ct => ct.AccountType == accountName.ToString());

            var allTransactions = await _currencyTransactionRepo.GetAllTransactions(currencyTag.AccountType);
            var inTransactions = await _currencyTransactionRepo.GetInTransactions(currencyTag.AccountType);
            var outTransactions = await _currencyTransactionRepo.GetOutTransactions(currencyTag.AccountType);

            userViewModel.AllAccounts = getCurrencyTag;
            userViewModel.AllTransactions = allTransactions;
            userViewModel.InTransactions = inTransactions;
            userViewModel.OutTransactions = outTransactions;

            return View(userViewModel);
        }
    }
}