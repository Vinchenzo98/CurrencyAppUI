using CurrencyAppUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CurrencyAppUI.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLoginController(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginUser(LoginViewModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            var userJson = JsonConvert.SerializeObject(loginUser);
            var userData = new StringContent(userJson, Encoding.UTF8, "application/json");
            var userResponse = await _client.PostAsync("/api/UserLogin/login", userData);

            if (userResponse.IsSuccessStatusCode)
            {
                var userDto = await userResponse.Content.ReadFromJsonAsync<UserViewModel>();
                _httpContextAccessor.HttpContext.Session.SetString("UserToken", userDto.Token);
                _httpContextAccessor.HttpContext.Session.SetString("UserModel", JsonConvert.SerializeObject(userDto));

                return RedirectToAction("Profile", "UserProfile");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Login");
            }
        }
    }
}