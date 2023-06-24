using eShop.AdminApp.Services;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace eShop.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration= configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex=1, int pageSize=50)
        {
            var userRequest = new UserPagingRequest()
            {
                BearerToken=HttpContext.Session.GetString("Token"),
                PageIndex=pageIndex,
                PageSize=pageSize,
                Keyword=keyword
            };

            var result =await _userApiClient.GetUsersPaging(userRequest);

            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterModelRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result =await _userApiClient.Create(request);
            if (result) return RedirectToAction("Index","User");
            return View(request);
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Login"); 
        }
    }
}
