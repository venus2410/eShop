using eShop.Application.Catalog.Categories;
using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _catergoriesService;
        public CategoriesController(ICategoriesService catergoriesService)
        {
            _catergoriesService = catergoriesService;
        }
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string? languageId)
        {
            var result = await _catergoriesService.GetAll(languageId);
            if (result.IsSucceed) return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{categoryId}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int categoryId, string languageId)
        {
            var result = await _catergoriesService.GetById(categoryId, languageId);
            if (result.IsSucceed) return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResultFail<bool>("Invalid Input"));
            }
            var result = await _catergoriesService.Create(model);
            if (result.IsSucceed) return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update(CategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResultFail<bool>("Invalid Input"));
            }
            var result = await _catergoriesService.Update(model);
            if (result.IsSucceed) return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("updatemodel/{categoryId}")]
        public async Task<IActionResult> GetForUpdate(int categoryId)
        {
            var result = await _catergoriesService.GetForUpdate(categoryId);
            if (result.IsSucceed) return Ok(result);
            return BadRequest(result);
        }
    }
}
