using CurrencyAppUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CurrencyAppUI.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public UserLoginController(
            IHttpClientFactory clientFactory,
            IConfiguration configuration
            )
        {
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
            _configuration = configuration;
            var baseUrl = _configuration["CurrnecyWebAPI:BaseUrl"];
            _client.BaseAddress = new System.Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                HttpContext.Session.SetString("UserJwt", userDto.Token);
                TempData["UserTag"] = userDto.UserTag;
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