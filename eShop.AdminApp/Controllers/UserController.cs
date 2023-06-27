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
using eShop.ViewModel.Catalog.Common;

namespace eShop.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var userRequest = new UserPagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };

            var result = await _userApiClient.GetUsersPaging(userRequest);
            return View(result.Data);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            return PartialView("_Create");
        }
        [HttpPost]
        public async Task<JsonResult> Create(UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var rs = new ServiceResultFail<bool>("Dữ liệu không hợp lệ");
                return Json(rs);
            }
            var result = await _userApiClient.Create(request);
            return Json(result);
        }
        [HttpGet]
        public async Task<PartialViewResult> Update(Guid Id)
        {
            var result =await _userApiClient.GetById(Id);
            if (result.IsSucceed)
            {
                var user = result.Data;
                var userUpdate = new UserUpdateRequest()
                {
                    Id=user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Dob=user.Dob,
                    Email=user.Email,
                    PhoneNumber=user.PhoneNumber
                };
                return PartialView("_Update",userUpdate);
            }
            return PartialView("_Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var rs = new ServiceResultFail<bool>("Dữ liệu không hợp lệ");
                return Json(rs);
            }
            var result = await _userApiClient.Update(request);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            var result = await _userApiClient.Delete(request.Id);
            return Json(result); 
        }
    }
}
