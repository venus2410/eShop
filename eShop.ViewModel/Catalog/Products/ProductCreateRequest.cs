using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal OriginalPrice { get; set; }
        [Required]
        public int Stock { get; set; }
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
        public string LanguageId { set; get; }
        [DataType(DataType.Upload)]
        public IFormFile ThumbnailImage { set; get; }
        public bool? IsFeatured { get; set; }
    }
}
