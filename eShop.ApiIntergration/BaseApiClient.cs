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
using System.Reflection;
using System.IO;

namespace eShop.ApiIntergration
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
#nullable enable
        public async Task<ServiceResult<ReturnType>> GetByIdAsync<ReturnType>(string url, string id, string? languageId)
        {
            var client = CreateAuthenticatedClient();

            var route = $"{url}/{id}";
            if(!string.IsNullOrEmpty(languageId))
            {
                route += $"/{languageId}";
            }

            var response = await client.GetAsync(route);

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

        public async Task<ServiceResult<ReturnType>> PostWithFileAsync<ReturnType, Ptype>(string url, Ptype model)
        {
            var httpContent = new MultipartFormDataContent();

            foreach(PropertyInfo propertyInfo in model.GetType().GetProperties())
            {
                if(propertyInfo.PropertyType == typeof(IFormFile))
                {
                    if (propertyInfo.GetValue(model) != null)
                    {
                        IFormFile file = (IFormFile)propertyInfo.GetValue(model);
                        byte[] data;
                        using (var br = new BinaryReader(file.OpenReadStream()))
                        {
                            data = br.ReadBytes((int)file.OpenReadStream().Length);
                        }
                        ByteArrayContent bytes = new ByteArrayContent(data);
                        httpContent.Add(bytes, "thumbnailImage", file.FileName);
                    }
                }
                else
                {
                    if (propertyInfo.GetValue(model) != null)
                    {
                        if(propertyInfo.PropertyType.IsClass
    && !propertyInfo.PropertyType.FullName.StartsWith("System."))
                        {
                            var json = JsonConvert.SerializeObject(propertyInfo.GetValue(model));
                            httpContent.Add(new StringContent(json, Encoding.UTF8, "application/json"), propertyInfo.Name);
                        }
                        else
                        {
                            httpContent.Add(new StringContent(propertyInfo.GetValue(model).ToString()), propertyInfo.Name);
                        }
                        
                    }
                    
                }
            }

            var client = CreateAuthenticatedClient();
            var response = await client.PostAsync(url, httpContent);

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
