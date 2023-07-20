using eShop.ApiIntergration;
using eShop.Utilities.Constants;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.ProductImages;
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
            var result = await _languageApiClient.GetLanguages();
            ViewBag.LanguagesList = result.Data;
            var model = new ProductCreateRequest();
            foreach (var r in result.Data)
            {
                var translation = new TranslationOfProduct()
                {
                    LanguageId = r.Id,
                    Name = ProductSetting.DefaultProductInfor,
                    Description = ProductSetting.DefaultProductInfor,
                    Details = ProductSetting.DefaultProductInfor,
                    SeoDescription = ProductSetting.DefaultProductInfor,
                    SeoTitle = ProductSetting.DefaultProductInfor,
                    SeoAlias = ProductSetting.DefaultProductInfor
                };
                model.Translations.Add(translation);
            }
            var languageId = "vi";
            ViewData["Categories"] = (await _catergoryApiClient.GetCatergories(languageId)).Data.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

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
            var result = await _productApiClient.Create(request);
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
            var result = await _languageApiClient.GetLanguages();
            ViewBag.LanguagesList = result.Data;
            var model = (await _productApiClient.GetForUpdate(id)).Data;

            ViewData["Categories"] = (await _catergoryApiClient.GetCatergories("vi")).Data.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = x.Id == model.CategoryId
            });

            return PartialView("_Update", model);
        }
        [TempData]
        public string Message { get; set; }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }
            var result = await _productApiClient.Update(request);
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
        public IActionResult AddImage(int productId)
        {
            ViewData["ProductId"] = productId;
            return PartialView("_AddImage");
        }
        [HttpPost]
        public async Task<IActionResult> AddImage(int productId, ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                Message = "Dữ liệu không hợp lệ";
            }
            var result = await _productApiClient.AddImage(productId, request);
            if (result.IsSucceed)
            {
                Message = "Thêm ảnh thành công";
            }
            else
            {
                Message = "Thêm ảnh không thành công";
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveImage(int productId)
        {
            ViewData["ProductId"]=productId;
            var images = await _productApiClient.GetImages(productId);
            return PartialView("_RemoveImage", images.Data);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveImage(List<SelectItem> images, int productId)
        {
            if (!ModelState.IsValid)
            {
                Message = "Dữ liệu không hợp lệ";
            }
            var deleteImages = images.Where(x => x.Selected).Select(x => int.Parse(x.Id)).ToList();
            var result = await _productApiClient.RemoveImages(deleteImages, productId);
            if (result.IsSucceed)
            {
                Message = "Xóa ảnh thành công";
            }
            else
            {
                Message = "Xóa ảnh không thành công";
            }
            return RedirectToAction("Index");
        }
    }
}
