using eShop.ApiIntergration;
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
        private readonly IRoleApiClient _roleApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
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
        public async Task<IActionResult> GetUserPaging(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var userRequest = new UserPagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };

            var result = await _userApiClient.GetUsersPaging(userRequest);
            return ViewComponent("UserTable",result.Data);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            var listRoles = _roleApiClient.GetAll();
            ViewBag.Roles = listRoles.Result.Data;
            return PartialView("_Create");
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }
            var result = await _userApiClient.Create(request);
            if (result.IsSucceed)
            {
                TempData["Message"] = "Tạo mới thành công";
            }
            else
            {
                TempData["Message"] = result.Errors;
            }
            return RedirectToAction("Index");
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
                    PhoneNumber=user.PhoneNumber,
                    Roles=user.Roles
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
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }
            var result = await _userApiClient.Update(request);
            if (result.IsSucceed)
            {
                TempData["Message"] = "Cập nhật thành công";
            }
            else
            {
                TempData["Message"] = "Cập nhật không thành công";
            }
            return RedirectToAction("Index");
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
        [AcceptVerbs("GET", "POST")]
        public IActionResult IsValidUserName(string userName, Guid? id)
        {
            var result=_userApiClient.IsValidUserName(userName, id);
            if (result.Result.IsSucceed) return Json(true);
            return Json(false);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult IsValidEmail(string email, Guid? id)
        {
            var result = _userApiClient.IsValidEmail(email, id);
            if (result.Result.IsSucceed) return Json(true);
            return Json(false);
        }
    }
}
