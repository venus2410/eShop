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
            return await _baseApiClient.GetGeneralAsync<List<CatergoryVM>>(baseURL+$"?languageId={languageId}");
        }

        public async Task<ServiceResult<CatergoryVM>> GetById(int categoryId, string languageId)
        {
            var url = $"{baseURL}/{categoryId}/{languageId}";
            return await _baseApiClient.GetGeneralAsync<CatergoryVM>(url);
        }
    }
}
