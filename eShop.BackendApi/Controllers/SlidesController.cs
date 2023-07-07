using eShop.Application.Catalog.Categories;
using eShop.Application.Catalog.Slides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SlidesController : Controller
    {
        private readonly ISlidesService _slidesService;
        public SlidesController(ISlidesService slidesService)
        {
            _slidesService = slidesService;
        }
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _slidesService.GetAll();
            if (result.IsSucceed) return Ok(result);
            return BadRequest(result);
        }
    }
}
