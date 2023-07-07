using eShop.Application.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CatergoriesController : Controller
    {
        private readonly ICatergoriesService _catergoriesService;
        public CatergoriesController(ICatergoriesService catergoriesService)
        {
            _catergoriesService = catergoriesService;
        }
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string? languageId )
        {
            var result=await _catergoriesService.GetAll(languageId);
            if(result.IsSucceed) return Ok(result);
            return BadRequest(result);
        }
    }
}
