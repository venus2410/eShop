using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface IProductApiClient
    {
        Task<ServiceResult<PageResult<ProductVM>>> GetPage(GetManageProductPagingRequest request);
        Task<ServiceResult<bool>> Create(ProductCreateRequest request);
        Task<ServiceResult<List<ProductVM>>> GetFeaturedProduct(string languageId, int take);
        Task<ServiceResult<List<ProductVM>>> GetLatestProduct(string languageId, int take);
    }
}