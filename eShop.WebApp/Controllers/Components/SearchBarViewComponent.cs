using eShop.ApiIntergration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShop.WebApp.Controllers.Components
{
    public class SearchBarViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public SearchBarViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoryApiClient.GetCatergories(CultureInfo.CurrentCulture.Name);
            return View(items.Data);
        }
    }
}
