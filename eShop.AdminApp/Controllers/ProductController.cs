using eShop.AdminApp.Services;
using eShop.Utilities.Constants;
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
        private readonly IRoleApiClient _roleApiClient;
        private readonly IConfiguration _configuration;
        public ProductController(IProductApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _productApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5, string languageId="vi")
        {
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
    }
}
