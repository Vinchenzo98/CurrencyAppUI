using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CurrencyAppUI.Repo
{
    public class CurrencyTransactionRepo : ICurrencyTransactionRepo
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CurrencyTransactionRepo> _logger;

        public CurrencyTransactionRepo(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrencyTransactionRepo> logger
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
            _logger = logger;
        }

        public async Task<List<UserTransactionResponse>> GetAllTransactions(string currencyTag)
        {
            var transactionRequest = new UserTransactionRequest
            {
                currencyTag = currencyTag
            };
            var json = JsonConvert.SerializeObject(transactionRequest);
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/UserTransaction/getAll";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsync(apiEndpoint, data);
            _logger.LogInformation($"The response content: {response.Content}");
            _logger.LogInformation($"{response.StatusCode}");
            _logger.LogInformation($"The request message: {response.RequestMessage}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var transactions = await response.Content.ReadFromJsonAsync<List<UserTransactionResponse>>();
                return transactions;
            }
            else
            {
                return new List<UserTransactionResponse>();
            }
        }

        public async Task<List<UserTransactionResponse>> GetInTransactions(string currencyTag)
        {
            var transactionRequest = new UserTransactionRequest
            {
                currencyTag = currencyTag
            };
            var json = JsonConvert.SerializeObject(transactionRequest);
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/UserTransaction/getPositive";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _client.PostAsync(apiEndpoint, data);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var transactions = await response.Content.ReadFromJsonAsync<List<UserTransactionResponse>>();
                return transactions;
            }
            else
            {
                return new List<UserTransactionResponse>();
            }
        }

        public async Task<List<UserTransactionResponse>> GetOutTransactions(string currencyTag)
        {
            var transactionRequest = new UserTransactionRequest
            {
                currencyTag = currencyTag
            };
            var json = JsonConvert.SerializeObject(transactionRequest);
            var token = _httpContextAccessor.HttpContext.Session.GetString("UserToken");
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/UserTransaction/getNegative";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _client.PostAsync(apiEndpoint, data);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var transactions = await response.Content.ReadFromJsonAsync<List<UserTransactionResponse>>();
                return transactions;
            }
            else
            {
                return new List<UserTransactionResponse>();
            }
        }
    }
}