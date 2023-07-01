using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using static eShop.Utilities.Constants.SystemConstant;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System;

namespace eShop.AdminApp.Services
{
    public class BaseApiClient:IBaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string token;
        public BaseApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            token = _contextAccessor.HttpContext.Session.GetString(AppSetting.Token);
        }
        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri(_configuration[AppSetting.BaseAddress]);
            return client;
        }
        private HttpClient CreateAuthenticatedClient()
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AppSetting.Bearer, token);
            return client;
        }
        public async Task<ServiceResult<ReturnType>> LoginAsync<ReturnType, Ptype>(string url, Ptype model)
        {
            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, Application.Json);

            var client = CreateClient();

            var response = await client.PostAsync(url, httpContent);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<ReturnType>>(body);
            return result;
        }

        public async Task<ServiceResult<ReturnType>> GetAllAsync<ReturnType>(string url)
        {
            var client=CreateAuthenticatedClient();

            var response = await client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<ReturnType>>(body);
            return result;
        }
        public async Task<ServiceResult<ReturnType>> GetByIdAsync<ReturnType>(string url, string id)
        {
            var client = CreateAuthenticatedClient();

            var response = await client.GetAsync($"{url}/{id}");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<ReturnType>>(body);
            return result;
        }
        public async Task<ServiceResult<ReturnType>> PostAsync<ReturnType,Ptype>(string url, Ptype model)
        {
            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, Application.Json);

            var client = CreateAuthenticatedClient();

            var response = await client.PostAsync(url,httpContent);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<ReturnType>>(body);
            return result;
        }
        public async Task<ServiceResult<ReturnType>> PutAsync<ReturnType, Ptype>(string url,string id, Ptype model)
        {
            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, Application.Json);

            var client = CreateAuthenticatedClient();

            var response = await client.PutAsync($"{url}/{id}", httpContent);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<ReturnType>>(body);
            return result;
        }
        public async Task<ServiceResult<ReturnType>> DeleteAsync<ReturnType>(string url, string id)
        {
            var client = CreateAuthenticatedClient();

            var response = await client.DeleteAsync($"{url}/{id}");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResult<ReturnType>>(body);
            return result;
        }
        //public async Task<ServiceResult<ReturnType>> Test<ReturnType, Ptype>(string url, Ptype model, Func<string,HttpContent,Task<HttpResponseMessage>> func)
        //{
        //    var json = JsonConvert.SerializeObject(model);
        //    var httpContent = new StringContent(json, Encoding.UTF8, Application.Json);

        //    var client = CreateAuthenticatedClient();

        //    var response = await client.PostAsync(url, httpContent);

        //    var body = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<ServiceResult<ReturnType>>(body);
        //    return result;
        //}
        
    }
}
