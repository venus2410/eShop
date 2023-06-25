using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
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
        private readonly IHttpContextAccessor _contextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory= httpClientFactory;
            _configuration= configuration;
            _contextAccessor= contextAccessor;
        }

        public async Task<ServiceResult<bool>> Create(UserCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, Application.Json);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/", httpContent);
            var content=await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<bool>>(content);
            return result;
        }

        public async Task<ServiceResult<UserViewModel>> GetById(Guid model)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/users/{model}");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<UserViewModel>>(body);
            return result;
        }

        public async Task<ServiceResult<PageResult<UserViewModel>>> GetUsersPaging(UserPagingRequest model)
        {
            var token=_contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            
            var response = await client.GetAsync($"/api/users/paging?" +
                $"{nameof(model.PageIndex)}={model.PageIndex}" +
                $"&{nameof(model.PageSize)}={model.PageSize}" +
                $"&{nameof(model.Keyword)}={model.Keyword}");
            
            var body = await response.Content.ReadAsStringAsync();
            var result=JsonConvert.DeserializeObject<ServiceResult<PageResult<UserViewModel>>>(body);

            return result;
        }

        public async Task<ServiceResult<string>> Login(UserLoginRequest model)
        {
            
            var json = JsonConvert.SerializeObject(model);
            var httpContent= new StringContent(json,Encoding.UTF8, Application.Json);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration["BaseAddress"]);
            
            var response =await client.PostAsync("/api/users/authenticate",httpContent);
            var content=await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<string>>(content);
            return result;
        }

        public async Task<ServiceResult<bool>> Update(UserUpdateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, Application.Json);

            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"/api/users/{request.Id}", httpContent);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<bool>>(content);
            return result;
        }
    }
}
