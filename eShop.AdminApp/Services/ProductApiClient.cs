using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminApp.Services
{
    public class ProductApiClient : IProductApiClient
    {
        private const string BaseUrl = "/api/products";
        private readonly IBaseApiClient _baseApiClient;
        public ProductApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient = baseApiClient;
        }

        public async Task<ServiceResult<PageResult<ProductVM>>> GetPage(GetManageProductPagingRequest request)
        {
            string getURI = BaseUrl + $"/paging?" + $"{nameof(request.PageIndex)}={request.PageIndex}&" + $"{nameof(request.PageSize)}={request.PageSize}&" + $"{nameof(request.Keyword)}={request.Keyword}&" + $"{nameof(request.LanguageId)}={request.LanguageId}&" + $"{nameof(request.CategoryId)}={request.CategoryId}";
            //Dictionary<string,string> dictionary=(Dictionary<string, string>) request;
            //QueryHelpers.AddQueryString(getURI, dictionary);
            return await _baseApiClient.GetAllAsync<PageResult<ProductVM>>(getURI);
        }
    }
}
