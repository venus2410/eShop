using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using eShop.ViewModel.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eShop.Utilities.Constants;
using System.IO.Pipes;
using static eShop.Utilities.Constants.SystemConstant;

namespace eShop.AdminApp.Services
{
    public class LanguageApiClient : ILanguageApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public LanguageApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }
        public async Task<ServiceResult<List<LanguageVM>>> GetLanguages()
        {
            var token = _contextAccessor.HttpContext.Session.GetString(AppSetting.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration[AppSetting.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AppSetting.Bearer, token);

            var response = await client.GetAsync($"/api/languages");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<List<LanguageVM>>>(body);
            return result;
        }
    }
}
