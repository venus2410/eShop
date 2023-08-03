using eShop.ViewModel.Catalog.Carts;
using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Carts
{
    public interface ICartsService
    {
        Task<ServiceResult<bool>> AddProduct(AddToCartRequest request);
    }
}
