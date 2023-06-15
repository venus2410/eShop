using eShop.ViewModel.Catalog.DTOs;
using eShop.ViewModel.Catalog.Products.DTOs;
using eShop.ViewModel.Catalog.Products.DTOs.Manage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategory(PublicProductPagingRequest request);
    }
}
