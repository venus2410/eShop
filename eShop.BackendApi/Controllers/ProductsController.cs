using eShop.Application.Catalog.Products;
using eShop.ViewModel.Catalog.ProductImages;
using eShop.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        readonly IPublicProductService _publicService;
        readonly IManageProductService _manageService;
        public ProductsController(IPublicProductService publicService, IManageProductService manageService)
        {
            _publicService = publicService;
            _manageService = manageService;
        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId, [FromQuery]GetPublicProductPagingRequest request)
        {
            var result = await _publicService.GetAllByCategoryId(languageId,request);
            return Ok(result);
        }
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var result=await _manageService.GetById(productId,languageId);
            if (result == null) return BadRequest($"Cannot find product with id: {productId}");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var productId = await _manageService.Create(request);
            if(productId<=0) return BadRequest();

            var product = _manageService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById),new {productId=productId, languageId=request.LanguageId},product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var affectedResult = await _manageService.Update(request);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _manageService.Delete(productId);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }

        [HttpPatch("price/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var result =await _manageService.UpdatePrice(productId, newPrice);
            if(!result) return BadRequest();
            return Ok();
        }

        // IMAGES
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId,[FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var imageId = await _manageService.AddImage(productId, request);
            if (imageId <= 0) return BadRequest();

            var image = _manageService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new {productId=productId ,imageId = imageId}, image);
        }
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var affectedResult = await _manageService.UpdateImage(imageId,request);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            var affectedResult = await _manageService.RemoveImage(imageId);
            if (affectedResult <= 0) return BadRequest();
            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            var result = await _manageService.GetImageById(imageId);
            if (result == null) return BadRequest($"Cannot find image with id: {imageId}");
            return Ok(result);
        }
    }
}
