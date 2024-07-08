using CurrencyAppUI.Models;
using CurrencyAppUI.Repo.Interface;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CurrencyAppUI.Repo
{
    public class GetProfileAccountsRepo : IGetProfileAccountsRepo
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public GetProfileAccountsRepo(
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

        public async Task<List<AccountTypeResponse>> GetAccountTypeRepo()
        {
            var requestUrl = "/api/AccountType/get-accounts";
            _session.GetString("UserJwt");
            HttpResponseMessage response = await _client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var exchangeResponse = JsonConvert.DeserializeObject(responseContent);

            return (List<AccountTypeResponse>)exchangeResponse;
        }
    }
}