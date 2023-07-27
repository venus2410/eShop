using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class ProductInCategoryVM
    {
        public CategoryVM Catergory { get; set; }
        public PageResult<ProductVM> Products { get; set; }
    }
}
