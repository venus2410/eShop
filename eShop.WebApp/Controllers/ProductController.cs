using eShop.ApiIntergration;
using eShop.ViewModel.Catalog.Products;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstant;

namespace eShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ISharedCultureLocalizer _loc;
        private readonly IProductApiClient _productApi;
        private readonly ICatergoryApiClient _catergoryApi;
        private readonly IConfiguration _configuration;
        public ProductController(ISharedCultureLocalizer loc, ICatergoryApiClient catergoryApi, IProductApiClient productApi,IConfiguration configuration)
        {
            _loc = loc;
            _catergoryApi = catergoryApi;
            _productApi = productApi;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ProductsByCategory(int categoryId, string culture, int page=1)
        {
            ViewBag.Category = (await _catergoryApi.GetById(categoryId, culture)).Data;
            ViewBag.BaseAddress = _configuration[AppSetting.BaseAddress];

            var requestProductPage = new GetManageProductPagingRequest {
                PageIndex= page,
                PageSize=10,
                CategoryId=categoryId,
                LanguageId=culture
            };
            var products=(await _productApi.GetPage(requestProductPage)).Data;
            return View(products);
        }
    }
}
