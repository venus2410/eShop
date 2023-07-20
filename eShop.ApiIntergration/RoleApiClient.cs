using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Roles;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IBaseApiClient _baseApiClient;
        private const string baseURL = "/api/roles";
        public RoleApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient = baseApiClient;
        }
        public async Task<ServiceResult<List<RoleVM>>> GetAll()
        {
            return await _baseApiClient.GetGeneralAsync<List<RoleVM>>(baseURL);
        }
    }
}
