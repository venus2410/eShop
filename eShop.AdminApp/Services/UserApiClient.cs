using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory= httpClientFactory;
        }
        public async Task<string> Login(LoginModelRequest model)
        {
            var json = JsonConvert.SerializeObject(model);
            var httpContent= new StringContent(json,Encoding.UTF8, Application.Json);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new System.Uri("https://localhost:44341");
            var response =await client.PostAsync("/api/users/authenticate",httpContent);
            var token=await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}
