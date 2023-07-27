using eShop.Utilities.Constants;
using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IBaseApiClient _baseApiClient;
        private const string baseURL = "/api/categories";
        public CategoryApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient = baseApiClient;
        }
        public async Task<ServiceResult<List<CategoryVM>>> GetCatergories(string languageId)
        {
            return await _baseApiClient.GetGeneralAsync<List<CategoryVM>>(baseURL+$"?languageId={languageId}");
        }

        public async Task<ServiceResult<CategoryVM>> GetById(int categoryId, string languageId)
        {
            var url = $"{baseURL}/{categoryId}/{languageId}";
            return await _baseApiClient.GetGeneralAsync<CategoryVM>(url);
        }

        public async Task<ServiceResult<bool>> Create(CategoryCreateVM model)
        {
            return await _baseApiClient.PostAsync<bool,CategoryCreateVM>(baseURL, model);
        }

        public async Task<ServiceResult<bool>> Update(CategoryUpdateVM model)
        {
            return await _baseApiClient.PutAsync<bool,CategoryUpdateVM>(baseURL, model.Id.ToString(), model);
        }

        public async Task<ServiceResult<CategoryUpdateVM>> GetForUpdate(int categoryId)
        {
            var url = $"{baseURL}/updatemodel/{categoryId}";
            return await _baseApiClient.GetGeneralAsync<CategoryUpdateVM>(url);
        }
    }
}
