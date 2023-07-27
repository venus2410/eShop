using eShop.ApiIntergration;
using eShop.ViewModel.Catalog.Products;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Security.Policy;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstant;

namespace eShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ISharedCultureLocalizer _loc;
        private readonly IProductApiClient _productApi;
        private readonly ICategoryApiClient _catergoryApi;
        private readonly IConfiguration _configuration;
        public ProductController(ISharedCultureLocalizer loc, ICategoryApiClient catergoryApi, IProductApiClient productApi,IConfiguration configuration)
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
        [ViewData]
        public string BaseAddress { get; set; }
        public async Task<IActionResult> ProductsByCategory(int categoryId, string culture, int pageIndex=1)
        {
            ViewBag.Category = (await _catergoryApi.GetById(categoryId, culture)).Data;
            ViewData[nameof(this.BaseAddress)] = _configuration[AppSetting.BaseAddress];

            var requestProductPage = new GetManageProductPagingRequest {
                PageIndex= pageIndex,
                PageSize=5,
                CategoryId=categoryId,
                LanguageId=culture
            };
            var products=(await _productApi.GetPage(requestProductPage)).Data;
            return View(products);
        }
        public async Task<IActionResult> Detail(string culture, int id)
        {
            var product =await _productApi.GetById(id,culture);
            if(!product.IsSucceed) return NotFound();
            return View(product.Data);
        }
        public async Task<IActionResult> Search(string culture, GetManageProductPagingRequest request)
        {
            TempData["CategoryId"] = request.CategoryId;
            TempData["Keyword"] = request.Keyword;
            request.PageSize = ProductSetting.NumberOfProductWebAppTable;
            request.LanguageId=culture;
            var products = (await _productApi.GetPage(request)).Data;
            return View(products);
        }
    }
}
