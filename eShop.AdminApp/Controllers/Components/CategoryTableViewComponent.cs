using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminApp.Controllers.Components
{
    public class CategoryTableViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(List<CategoryVM> result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
