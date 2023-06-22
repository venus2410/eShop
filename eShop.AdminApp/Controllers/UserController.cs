using eShop.AdminApp.Services;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        public UserController(IUserApiClient userApiClient)
        {
              _userApiClient= userApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModelRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result=await _userApiClient.Login(request);
            return View();
        }
    }
}
