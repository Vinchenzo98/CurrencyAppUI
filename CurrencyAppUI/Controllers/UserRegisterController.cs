using CurrencyAppUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CurrencyAppUI.Controllers
{
    public class UserRegisterController : Controller
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public UserRegisterController(
            IConfiguration configuration,
            IHttpClientFactory clientFactory
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
        public ActionResult Register()
        {
            ViewData["Title"] = "Register";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(RegisterViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                return View("Register");
            }

            var json = JsonConvert.SerializeObject(registerUser);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/UserRegister/register", data);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadFromJsonAsync<UserViewModel>();
                return RedirectToAction("Login", "UserLogin");
            }
            else
            {
                ModelState.AddModelError("", "Invalid register attempt.");
                return View("Register");
            }
        }
    }
}