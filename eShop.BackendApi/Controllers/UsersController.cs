using eShop.Application.System.Users;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService service)
        {
            _userService = service;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginModelRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result =await _userService.Login(request);
            if(string.IsNullOrEmpty(result))
            {
                return BadRequest("Wrong user name or password");
            }
            return Ok(new {token= result });
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterModelRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register fail");
            }
            return Ok();
        }
    }
}
