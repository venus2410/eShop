using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.System
{
    public class NavigationVM
    {
        public List<SelectListItem> Languages { get; set; }
        public string CurrentLanguageId { get; set; }
    }
}
