using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CurrencyAppUI.Repo
{
    public class CurrencyTransactionRepo : ICurrencyTransactionRepo
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        public CurrencyTransactionRepo(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _client = clientFactory.CreateClient("CurrencyAppUIClient");
            _configuration = configuration;
            var baseUrl = _configuration["CurrnecyWebAPI:BaseUrl"];
            _client.BaseAddress = new System.Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<UserTransactionResponse>> GetAllTransactions(string currencyTag)
        {
            var json = JsonConvert.SerializeObject(currencyTag);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/UserTransacton/getAll";
            _session.GetString("UserJwt");
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

        public async Task<List<UserTransactionResponse>> GetInTransactions(string currencyTag)
        {
            var json = JsonConvert.SerializeObject(currencyTag);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/UserTransaction/getPositive";
            _session.GetString("UserJwt");
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
            var json = JsonConvert.SerializeObject(currencyTag);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var apiEndpoint = "/api/UserTransaction/getNegative";
            _session.GetString("UserJwt");
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