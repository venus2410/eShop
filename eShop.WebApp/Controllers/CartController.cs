using eShop.ApiIntergration;
using eShop.ViewModel.Catalog.Carts;
using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.WebApp.Controllers
{
    public class CartController : Controller
    {
        readonly ICartApiClient _cartApiClient;
        public CartController(ICartApiClient cartApiClient)
        {
            _cartApiClient = cartApiClient;
        }
        public async Task<IActionResult> AddToCart(int productId)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ServiceResultFail<bool>("Model invalid"));
            }
            if(string.IsNullOrEmpty(User.Identity.Name))
            {
                return Json(new ServiceResultFail<bool>("You must sign in first"));
            }
            var request = new AddToCartRequest
            {
                ProductId = productId,
                UserName = User.Identity.Name
            };
            return Json(await _cartApiClient.AddProduct(request));
        }
    }
}
