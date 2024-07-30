using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CurrencyAppUI.Repo
{
    public class GetProfileAccountsRepo : IGetProfileAccountsRepo
    {
        private readonly HttpClient _client;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetProfileAccountsRepo> _logger;

        public GetProfileAccountsRepo(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<GetProfileAccountsRepo> logger
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
            _logger = logger;
        }

        public async Task<List<AccountTypeResponse>> GetAccountTypeRepo()
        {
            var requestUrl = "/api/AccountType/get-accounts";
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine($"Request headers: {_client.DefaultRequestHeaders}");

            HttpResponseMessage response = await _client.GetAsync(requestUrl);

            _logger.LogInformation($"HttpGet {response}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request failed with status code {response.StatusCode}");
            }

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var exchangeResponse = JsonConvert.DeserializeObject<List<AccountTypeResponse>>(responseContent);

            return exchangeResponse;
        }

        public async Task<AccountTypeResponse> CreateAccountRepo(AccountTypeRequest accountTypeRequest)
        {
            var requestUrl = "/api/AccountType/create-account";
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var json = JsonConvert.SerializeObject(accountTypeRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine($"Request headers: {_client.DefaultRequestHeaders}");

            HttpResponseMessage response = await _client.PostAsync(requestUrl, data);

            _logger.LogInformation($"HttpPost {response}");

            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var currencySent = await response.Content.ReadFromJsonAsync<AccountTypeResponse>();
                return currencySent;
            }
            else
            {
                return new AccountTypeResponse();
            }

        }
    }
}