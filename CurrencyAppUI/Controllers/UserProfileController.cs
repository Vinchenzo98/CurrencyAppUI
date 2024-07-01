using Microsoft.AspNetCore.Mvc;

namespace CurrencyAppUI.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Profile()
        {
            ViewData["Title"] = "Profile";
            return View();
        }
    }
}