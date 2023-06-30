using eShop.Application.System.Languages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LanguagesController : Controller
    {
        private readonly ILanguageService _languageService;
        public LanguagesController(ILanguageService languageService)
        {
            _languageService= languageService;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result=await _languageService.GetLanguageVMsAsync();
            if(result.IsSucceed)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
