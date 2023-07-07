using eShop.ApiIntergration;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using static eShop.Utilities.Constants.SystemConstant;

namespace eShop.AdminApp.Controllers.Components
{
    public class NavigationViewComponent:ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;
        public NavigationViewComponent(ILanguageApiClient languageApiClient)
        {
            _languageApiClient= languageApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dataFromApi= await _languageApiClient.GetLanguages();
            var languages = dataFromApi.Data;
            var currentLanguageId = HttpContext.Session.GetString(AppSetting.CurrentLanguageId);
            var items = languages.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id,
                Selected = currentLanguageId == null ? x.IsDefault : x.Id == currentLanguageId
            }).ToList();
            currentLanguageId??=languages.Where(l=>l.IsDefault).Select(l=>l.Id).FirstOrDefault();

            var navigationVM = new NavigationVM {
                Languages = items,
                CurrentLanguageId= currentLanguageId
            };

            return View("Default",navigationVM);
        }
    }
}
