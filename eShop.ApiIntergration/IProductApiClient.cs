using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface IProductApiClient
    {
        Task<ServiceResult<PageResult<ProductVM>>> GetPage(GetManageProductPagingRequest request);
        Task<ServiceResult<bool>> Create(ProductCreateRequest request);
    }
}