using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CurrencyAppUI.Repo
{
    public class GetUserListRepo : IGetUserListRepo
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserListRepo(
                IHttpContextAccessor httpContextAccessor,
                IHttpClientFactory clientFactory
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            var apiEndpoint = "/api/UserLogin/get-users";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync(apiEndpoint);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var exchangeResponse = JsonConvert.DeserializeObject<List<UserViewModel>>(responseContent);

            return exchangeResponse;
        }
    }
}