using eShop.Application.Catalog.Products;
using eShop.ViewModel.Catalog.ProductImages;
using eShop.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        readonly IProductsService _productService;
        public ProductsController(IProductsService productService)
        {
            _productService = productService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery]GetManageProductPagingRequest request)
        {
            var result = await _productService.GetByPaging(request);
            if(!result.IsSucceed) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var result=await _productService.GetById(productId,languageId);
            if (result == null) return BadRequest($"Cannot find product with id: {productId}");
            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _productService.Create(request);
            if(!result.IsSucceed) return BadRequest(result);
            return Ok(result);            
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id,[FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var affectedResult = await _productService.Update(request);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _productService.Delete(productId);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("price/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var result =await _productService.UpdatePrice(productId, newPrice);
            if(!result) return BadRequest();
            return Ok();
        }

        // IMAGES
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId,[FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var imageId = await _productService.AddImage(productId, request);
            if (imageId <= 0) return BadRequest();

            var image = _productService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new {id=productId}, image);
        }
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var affectedResult = await _productService.UpdateImage(imageId,request);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            var affectedResult = await _productService.RemoveImage(imageId);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            var result = await _productService.GetImageById(imageId);
            if (result == null) return BadRequest($"Cannot find image with id: {imageId}");
            return Ok(result);
        }
        [HttpGet("featured/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedProduct(string languageId, int take)
        {
            var result=await _productService.GetFeaturedProduct(languageId, take);
            return Ok(result);
        }
    }
}
