using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Test success!");
        }
    }
}
