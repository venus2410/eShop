using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface IProductApiClient
    {
        Task<ServiceResult<PageResult<ProductVM>>> GetPage(GetManageProductPagingRequest request);
        Task<ServiceResult<bool>> Create(ProductCreateRequest request);
        Task<ServiceResult<bool>> Update(ProductUpdateRequest request);
        Task<ServiceResult<List<ProductVM>>> GetFeaturedProduct(string languageId, int take);
        Task<ServiceResult<List<ProductVM>>> GetLatestProduct(string languageId, int take);
        Task<ServiceResult<List<TranslationOfProduct>>> GetProductTranslation(int productId);
        Task<ServiceResult<ProductVM>> GetById(int productId, string languageId);
    }
}