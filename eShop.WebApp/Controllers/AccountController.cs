using eShop.ApiIntergration;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using static eShop.Utilities.Constants.SystemConstant;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using eShop.ViewModel.Catalog.Common;

namespace eShop.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public AccountController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove(AppSetting.Token);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Login(request);
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Errors);
                return View();
            }
            var token = result.Data;
            var userPrincipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString(AppSetting.Token, token);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
        public IActionResult Register(string email)
        {
            var user = new UserCreateRequest()
            {
                Email = email
            };
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            request.Roles.Add(new SelectItem
            {
                Name = UserSetting.DefaultRoleNameForUser,
                Selected = true
            });
            var createResult = await _userApiClient.Create(request);
            if (!createResult.IsSucceed)
            {
                ModelState.AddModelError("", createResult.Errors);
                return View(request);
            }

            var loginResult = await _userApiClient.Login(new UserLoginRequest
            {
                UserName = request.UserName,
                Password = request.Password,
                RememberMe = true
            });
            if (!loginResult.IsSucceed)
            {
                ModelState.AddModelError("", loginResult.Errors);
                return View(request);
            }

            var userPrincipal = this.ValidateToken(loginResult.Data);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddHours(2),
                IsPersistent = true
            };
            HttpContext.Session.SetString(AppSetting.Token, loginResult.Data);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            return RedirectToAction("Index", "Home");
        }
    }
}
