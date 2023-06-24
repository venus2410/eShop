using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory= httpClientFactory;
            _configuration= configuration;
        }

        public async Task<bool> Create(RegisterModelRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, Application.Json);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/", httpContent);
            
            return response.IsSuccessStatusCode;
        }

        public async Task<PageResult<UserViewModel>> GetUsersPaging(UserPagingRequest model)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",model.BearerToken);
            var response = await client.GetAsync($"/api/users/paging?" +
                $"{nameof(model.PageIndex)}={model.PageIndex}" +
                $"&{nameof(model.PageSize)}={model.PageSize}" +
                $"&{nameof(model.Keyword)}={model.Keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var result=JsonConvert.DeserializeObject<PageResult<UserViewModel>>(body);

            return result;
        }

        public async Task<string> Login(LoginModelRequest model)
        {
            var json = JsonConvert.SerializeObject(model);
            var httpContent= new StringContent(json,Encoding.UTF8, Application.Json);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration["BaseAddress"]);
            var response =await client.PostAsync("/api/users/authenticate",httpContent);
            var token=await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}
