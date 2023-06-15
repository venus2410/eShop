using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategory(GetPublicProductPagingRequest request);
    }
}
