using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers.Components
{
    public class ProductTableViewComponent:ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
