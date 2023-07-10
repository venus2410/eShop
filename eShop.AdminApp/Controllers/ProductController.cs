using eShop.ApiIntergration;
using eShop.Utilities.Constants;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
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
        public ProductController(IProductApiClient userApiClient, ILanguageApiClient languageApiClient, IRoleApiClient roleApiClient, ICatergoryApiClient catergoryApiClient)
        {
            _productApiClient = userApiClient;
            _roleApiClient = roleApiClient;
            _languageApiClient = languageApiClient;
            _catergoryApiClient = catergoryApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5, string languageId = "vi")
        {
            ViewBag.keyword = keyword;

            var curLanguageId = HttpContext.Session.GetString(AppSetting.CurrentLanguageId);
            var catergories = _catergoryApiClient.GetCatergories(curLanguageId).Result.Data;
            ViewBag.Catergories = catergories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = x.Id == categoryId
            }).ToList();

            var pageSizes = new List<SelectListItem> {
                new SelectListItem {Text="5",Value="5", Selected=pageSize.ToString()=="5" },
                new SelectListItem {Text="10",Value="10", Selected=pageSize.ToString()=="10" },
                new SelectListItem {Text="20",Value="20", Selected=pageSize.ToString()=="20" },
                new SelectListItem {Text="50",Value="50", Selected=pageSize.ToString()=="50" },
                new SelectListItem {Text="100",Value="100", Selected=pageSize.ToString()=="100" }
            };
            ViewBag.pageSizes = pageSizes;


            var languages = _languageApiClient.GetLanguages().Result.Data;
            ViewBag.Languages = languages.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = x.Id == languageId
            }).ToList(); ;


            var request = new GetManageProductPagingRequest
            {
                Keyword = keyword,
                CategoryId = categoryId,
                PageIndex = pageIndex,  
                PageSize = pageSize,
                LanguageId = languageId
            };
            var result = await _productApiClient.GetPage(request);
            if (!result.IsSucceed)
            {
                ModelState.AddModelError("", result.Errors);
                return View();
            }
            return View(result.Data);
        }
        public async Task<IActionResult> GetProductPaging(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5, string languageId = "vi")
        {
            var a = Request.Query;
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
            var a = Request.Query;
            var result = await _languageApiClient.GetLanguages();
            ViewBag.LanguagesList = result.Data;
            var model = new ProductCreateRequest();
            foreach (var r in result.Data)
            {
                var translation = new Translation()
                {
                    LanguageId = r.Id,
                    Name = "N/A",
                    Description = "N/A",
                    Details = "N/A",
                    SeoDescription = "N/A",
                    SeoTitle = "N/A",
                    SeoAlias = "N/A"
                };
                model.Translations.Add(translation);
            }
            return PartialView("_Create", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }
            var result=await _productApiClient.Create(request);
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
        public async Task<IActionResult> Update(int id)
        {
            var response = await _productApiClient.GetProductTranslation(id);
            var translations = response.Data;
            var model = new ProductUpdateRequest
            {
                Id = id,
                Translations = translations
            };

            return PartialView("_Update", model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }
            var result=await _productApiClient.Update(request);
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
    }
}
