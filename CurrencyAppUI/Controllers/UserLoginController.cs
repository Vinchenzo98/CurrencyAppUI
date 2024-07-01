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

            var json = JsonConvert.SerializeObject(loginUser);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/UserLogin/login", data);

            if (response.IsSuccessStatusCode)
            {
                var userDto = await response.Content.ReadFromJsonAsync<UserViewModel>();
                HttpContext.Session.SetString("UserJwt", userDto.Token);

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