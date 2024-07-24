using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CurrencyAppUI.Repo
{
    public class CurrencyOperationsRepo : ICurrencyOperationsRepo
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetProfileAccountsRepo> _logger;

        public CurrencyOperationsRepo(
                IHttpContextAccessor httpContextAccessor,
                IHttpClientFactory clientFactory
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
        }

        public async Task<UserOperationsResponse> DepositCurrency(UserDepositRequest userDepositRequest)
        {
            var json = JsonConvert.SerializeObject(userDepositRequest);
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/CurrencyDeposit/deposit";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsync(apiEndpoint, data);

            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var currencySent = await response.Content.ReadFromJsonAsync<UserOperationsResponse>();
                return currencySent;
            }
            else
            {
                return new UserOperationsResponse();
            }
        }

        public async Task<UserOperationsResponse> ExchangeCurrency(string amount, string baseCurrency, string targetCurrency)
        {
            var sendCurrencyRequest = new UserCurrencyExchangeRequest
            {
                amount = amount,
                baseCurrency = baseCurrency,
                targetCurrency = targetCurrency
            };

            var json = JsonConvert.SerializeObject(sendCurrencyRequest);
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/CurrencyExchange/exchange";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsync(apiEndpoint, data);

            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var currencySent = await response.Content.ReadFromJsonAsync<UserOperationsResponse>();
                return currencySent;
            }
            else
            {
                return new UserOperationsResponse();
            }
        }

        public async Task<UserOperationsResponse> SendCurrency(UserSendCurrencyRequest sendCurrencyRequest)
        {
            var json = JsonConvert.SerializeObject(sendCurrencyRequest);
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/CurrencyTransfer/transfer";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsync(apiEndpoint, data);

            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var currencySent = await response.Content.ReadFromJsonAsync<UserOperationsResponse>();
                return currencySent;
            }
            else
            {
                return new UserOperationsResponse();
            }
        }

        public async Task<UserOperationsResponse> WithdrawCurrency(UserWithdrawRequest userWithdrawRequest)
        {
            var json = JsonConvert.SerializeObject(userWithdrawRequest);
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/CurrencyWithdarw/withdraw";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsync(apiEndpoint, data);

            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var currencySent = await response.Content.ReadFromJsonAsync<UserOperationsResponse>();
                return currencySent;
            }
            else
            {
                return new UserOperationsResponse();
            }
        }
    }
}