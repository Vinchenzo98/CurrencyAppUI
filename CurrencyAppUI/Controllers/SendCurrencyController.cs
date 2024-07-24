using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAppUI.Controllers
{
    public class SendCurrencyController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICurrencyOperationsRepo _currencyOperationsRepo;
        private readonly ICurrencyTransactionRepo _currencyTransactionRepo;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;
        private readonly IGetUserListRepo _userListRepo;

        public SendCurrencyController(
                IHttpClientFactory httpClientFactory,
                IHttpContextAccessor contextAccessor,
                ICurrencyOperationsRepo currencyOperationsRepo,
                ICurrencyTransactionRepo currencyTransactionRepo,
                IGetProfileAccountsRepo getProfileAccountsRepo,
                IGetUserListRepo getUserListRepo
            )
        {
            _contextAccessor = contextAccessor;
            _client = httpClientFactory.CreateClient("CurrencyAppUIClient");
            _currencyOperationsRepo = currencyOperationsRepo;
            _currencyTransactionRepo = currencyTransactionRepo;
            _profileAccountsRepo = getProfileAccountsRepo;
            _userListRepo = getUserListRepo;
        }

        public async Task<ActionResult> Send()
        {
            var currency = TempData["CurrencyTag"].ToString();
            var userSendRequest = new UserSendCurrencyRequest
            {
                currencyTag = currency
            };

            var getAllUsers = await _userListRepo.GetAllUsers();

            var userTagList = new List<UserViewModel>();
            foreach (var user in getAllUsers)
            {
                var userTagDTO = new UserViewModel
                {
                    UserTag = user.UserTag
                };

                userTagList.Add(userTagDTO);
            }
            var userTagData = ViewData["UserTagData"] = userTagList;
            TempData["UserTagsList"] = userTagData;

            return View(userSendRequest);
        }

        [HttpPost]
        public async Task<ActionResult> UserSendCurrency(UserSendCurrencyRequest userSendCurrency)
        {
            await _currencyOperationsRepo.SendCurrency(userSendCurrency);
            return RedirectToAction(userSendCurrency.currencyTag, "UserAccount");
        }
    }
}