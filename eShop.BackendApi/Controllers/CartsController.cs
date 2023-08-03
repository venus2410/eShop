using eShop.Application.Catalog.Carts;
using eShop.ViewModel.Catalog.Carts;
using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartsController : Controller
    {
        readonly ICartsService _cartService;
        public CartsController(ICartsService cartsService)
        {
            _cartService = cartsService;
        }
        [HttpPost("{userName}/{productId}/{quantity}")]
        public async Task<IActionResult> AddProduct(AddToCartRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResultFail<bool>("Invalid model"));
            }
            var result=await _cartService.AddProduct(request);
            if(result.IsSucceed) { return Ok(result); }
            return BadRequest(result);
        }
    }
}
