using eShop.ViewModel.Catalog.DTOs;
using eShop.ViewModel.Catalog.Products.DTOs;
using eShop.Data.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShop.ViewModel.Catalog.Products.DTOs.Manage;

namespace eShop.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _context;
        public PublicProductService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<PageResult<ProductViewModel>> GetAllByCategory(PublicProductPagingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
