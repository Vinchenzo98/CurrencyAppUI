using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAppUI.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly ICurrencyTransactionRepo _currencyTransactionRepo;
        private readonly IGetProfileAccountsRepo _profileAccountsRepo;

        public UserProfileController(
            ICurrencyTransactionRepo currencyTransactionRepo,
            IGetProfileAccountsRepo profileAccountsRepo
            )
        {
            _currencyTransactionRepo = currencyTransactionRepo;
            _profileAccountsRepo = profileAccountsRepo;
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            var userTag = TempData["UserTag"]?.ToString();
            var currencyTag = TempData["currencyTag"]?.ToString();
            var userViewModel = new UserViewModel { UserTag = userTag };
            ViewData["Title"] = "Profile";

            var getUserAccounts = await _profileAccountsRepo.GetAccountTypeRepo();

            var allTransactions = await _currencyTransactionRepo.GetAllTransactions(currencyTag);
            var inTransactions = await _currencyTransactionRepo.GetInTransactions(currencyTag);
            var outTransactions = await _currencyTransactionRepo.GetOutTransactions(currencyTag);

            userViewModel.AllAccounts = getUserAccounts;
            userViewModel.AllTransactions = allTransactions;
            userViewModel.InTransactions = inTransactions;
            userViewModel.OutTransactions = outTransactions;

            return View(userViewModel);
        }
    }
}