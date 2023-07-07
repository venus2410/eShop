using eShop.Utilities.Constants;
using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public class CatergoryApiClient : ICatergoryApiClient
    {
        private readonly IBaseApiClient _baseApiClient;
        private const string baseURL = "/api/catergories";
        public CatergoryApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient = baseApiClient;
        }
        public async Task<ServiceResult<List<CatergoryVM>>> GetCatergories(string languageId)
        {
            return await _baseApiClient.GetAllAsync<List<CatergoryVM>>(baseURL+$"?languageId={languageId}");
        }
    }
}
