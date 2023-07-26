using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.WebApp.Controllers.Components
{
    public class ProductTableViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResult<ProductVM> result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
