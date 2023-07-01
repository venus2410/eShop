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
using eShop.ViewModel.Catalog.Products;

namespace eShop.AdminApp.Services
{
    public class LanguageApiClient : ILanguageApiClient
    {
        private readonly IBaseApiClient _baseApiClient;
        private const string baseURL = "/api/languages";
        public LanguageApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient = baseApiClient;
        }
        public async Task<ServiceResult<List<LanguageVM>>> GetLanguages()
        {
            return await _baseApiClient.GetAllAsync<List<LanguageVM>>(baseURL);
        }
    }
}
