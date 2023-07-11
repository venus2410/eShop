using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class ProductPrices
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal OriginalPrice { get; set; }
    }
}
