using eShop.Application.System.Users;
using eShop.ViewModel.System.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
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
        private readonly IValidator<LoginModelRequest> _loginValidator;
        private readonly IValidator<RegisterModelRequest> _registerValidator;
        public UsersController(IUserService service, IValidator<LoginModelRequest> loginValidator, IValidator<RegisterModelRequest> registerValidator)
        {
            _userService = service;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginModelRequest request)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            var validateResult=await _loginValidator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                validateResult.AddToModelState(this.ModelState);
                return BadRequest(ModelState);
            }
            var result =await _userService.Login(request);
            if(string.IsNullOrEmpty(result))
            {
                return BadRequest("Wrong user name or password");
            }
            return Ok(result);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModelRequest request)
        {
            var validateResult= await _registerValidator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                validateResult.AddToModelState(this.ModelState);
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register fail");
            }
            return Ok();
        }
    }
}
