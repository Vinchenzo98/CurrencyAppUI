using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyAppUI.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;

        public UserProfileController(
            IGetProfileAccountsRepo profileAccountsRepo,
             IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _profileAccountsRepo = profileAccountsRepo;
            _httpContextAccessor = httpContextAccessor;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            var userModelJson = _httpContextAccessor.HttpContext.Session.GetString("UserModel");
            if (string.IsNullOrEmpty(userModelJson))
            {
                return RedirectToAction("Login", "UserLogin");
            }
            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(userModelJson);
            ViewData["Title"] = "Profile";

            var getUserAccounts = await _profileAccountsRepo.GetAccountTypeRepo();

            userViewModel.AllAccounts = getUserAccounts;

            return View(userViewModel);
        }
    }
}