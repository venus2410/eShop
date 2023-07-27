using eShop.Application.System.Roles;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService= roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleVM model)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(new ServiceResultFail<bool>());
            }
            var result= await _roleService.Create(model);
            if(result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
