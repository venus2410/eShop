using eShop.ApiIntergration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace eShop.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult IsValidUserName(string userName, Guid? id)
        {
            var result = _userApiClient.IsValidUserName(userName, id);
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
