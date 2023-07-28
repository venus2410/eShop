using eShop.ApiIntergration;
using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryApiClient _categoryApiClient;
        private ILanguageApiClient _languageApiClient;
        public CategoryController(ICategoryApiClient apiClient, ILanguageApiClient languageApiClient)
        {
            _categoryApiClient = apiClient;
            _languageApiClient = languageApiClient;
        }
        public IActionResult Index()
        {
            var categories = _categoryApiClient.GetCatergories(CultureInfo.CurrentCulture.Name);
            return View(categories.Result.Data);
        }
        public async Task<IActionResult> Create()
        {
            var languages = await _languageApiClient.GetLanguages();
            ViewBag.LanguagesList = languages.Data;

            var categories = (await _categoryApiClient.GetCatergories(CultureInfo.CurrentCulture.Name)).Data;
            ViewBag.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            var model = new CategoryCreateVM();
            foreach (var language in languages.Data)
            {
                model.Translations.Add(new CategoryTranslationVM
                {
                    LanguageId = language.Id
                });
            }
            return PartialView("_Create", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }
            var result = await _categoryApiClient.Create(model);
            if (result.IsSucceed)
            {
                TempData["Message"] = "Tạo mới thành công";
            }
            else
            {
                TempData["Message"] = "Tạo mới không thành công";
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int categoryId)
        {
            var category=(await _categoryApiClient.GetForUpdate(categoryId)).Data;
            var languages = await _languageApiClient.GetLanguages();
            ViewBag.LanguagesList = languages.Data;

            var categories = (await _categoryApiClient.GetCatergories(CultureInfo.CurrentCulture.Name)).Data;
            categories=categories.Where(x=>x.Id!=category.Id).ToList();
            ViewBag.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected=x.Id==category.ParentId
            });

            return View("_Update",category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }
            var result = await _categoryApiClient.Update(model);
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
        public async Task<IActionResult> Delete(int id)
        {
            var serviceResult=new ServiceResult<bool>();
            if (!ModelState.IsValid)
            {
                serviceResult.IsSucceed = false;
                serviceResult.Errors= "Dữ liệu không hợp lệ";
                return Json(serviceResult);
            }
            var result = await _categoryApiClient.Delete(id);
            if (result.IsSucceed)
            {
                serviceResult.IsSucceed = true;
            }
            else
            {
                serviceResult.IsSucceed = false;
                serviceResult.Errors = "Xóa không thành công";
            }
            return Json(serviceResult);
        }
    }
}
