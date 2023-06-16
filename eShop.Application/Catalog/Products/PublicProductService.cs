using eShop.ViewModel.Catalog.Common;
using eShop.Data.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eShop.ViewModel.Catalog.Products;

namespace eShop.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _context;
        public PublicProductService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<PageResult<ProductViewModel>> GetAllByCategory(GetPublicProductPagingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
