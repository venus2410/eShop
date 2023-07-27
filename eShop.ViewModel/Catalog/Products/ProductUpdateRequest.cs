using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int Id { set; get; }
        public List<TranslationOfProduct> Translations { get; set; } = new List<TranslationOfProduct>();
        public int CategoryId { get; set; }
        public bool IsFeatured { get; set; }
    }
}
