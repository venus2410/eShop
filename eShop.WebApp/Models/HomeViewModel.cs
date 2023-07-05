using eShop.ViewModel.Catalog.Products;
using eShop.ViewModel.Catalog.Slides;
using System.Collections.Generic;

namespace eShop.WebApp.Models
{
    public class HomeViewModel
    {
        public string BaseAddress { get; set; }
        public List<SlideVM> Slides { get; set; }
        public List<ProductVM> FeaturedProducts { get; set; }
    }
}
