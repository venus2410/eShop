using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class Translation
    {
        public string LanguageId { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string Description { set; get; }
        [Required]
        public string Details { set; get; }
        [Required]
        public string SeoDescription { set; get; }
        [Required]
        public string SeoTitle { set; get; }
        [Required]
        public string SeoAlias { get; set; }
    }
}
