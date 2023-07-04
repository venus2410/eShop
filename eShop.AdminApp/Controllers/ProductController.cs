using eShop.AdminApp.Services;
using eShop.Utilities.Constants;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstant;

namespace eShop.AdminApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ILanguageApiClient _languageApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly ICatergoryApiClient _catergoryApiClient;
        public ProductController(IProductApiClient userApiClient,ILanguageApiClient languageApiClient, IRoleApiClient roleApiClient, ICatergoryApiClient catergoryApiClient)
        {
            _productApiClient = userApiClient;
            _roleApiClient = roleApiClient;
            _languageApiClient = languageApiClient;
            _catergoryApiClient = catergoryApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5, string languageId="vi")
        {
            var curLanguageId = HttpContext.Session.GetString(AppSetting.CurrentLanguageId);
            var languages =_languageApiClient.GetLanguages().Result.Data;
            ViewBag.Languages = languages;
            var catergories=_catergoryApiClient.GetCatergories(curLanguageId).Result.Data;
            ViewBag.Catergories = catergories;
            var request = new GetManageProductPagingRequest
            {
                Keyword= keyword,
                CategoryId= categoryId,
                PageIndex= pageIndex,
                PageSize= pageSize,
                LanguageId=languageId
            };
            var result = await _productApiClient.GetPage(request);
            if(!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Errors);
                return View();
            }
            return View(result.Data);
        }
        public async Task<IActionResult> GetProductPaging(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5, string languageId = "vi")
        {
            var request = new GetManageProductPagingRequest
            {
                Keyword = keyword,
                CategoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId
            };

            var result = await _productApiClient.GetPage(request);
            return ViewComponent("ProductTable", result.Data);
        }
        public async Task<IActionResult> Create()
        {
            var result=await _languageApiClient.GetLanguages();
            ViewBag.LanguagesList = result.Data;
            return PartialView("_Create");
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return Json(new ServiceResultFail<bool>("Dữ liệu không hợp lệ"));
            }
            return Json(await _productApiClient.Create(request));
        }
    }
}
