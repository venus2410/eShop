using eShop.ViewModel.Catalog.Carts;
using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public class CartApiClient : ICartApiClient
    {
        string baseURL = "/api/carts";
        readonly IBaseApiClient _baseApiClient;
        public CartApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient= baseApiClient;
        }
        public async Task<ServiceResult<bool>> AddProduct(AddToCartRequest request)
        {
            var url = $"{baseURL}/{request.UserName}/{request.ProductId}/{request.Quantity}";
            return await _baseApiClient.PostAsync<bool,AddToCartRequest>(url, request);
        }
    }
}
