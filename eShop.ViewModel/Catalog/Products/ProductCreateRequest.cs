﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class ProductCreateRequest
    {
        public ProductPrices Prices { get; set; }
        [Required]
        public int Stock { get; set; }
        public List<Translation> Translations { get; set; }=new List<Translation>();
        [DataType(DataType.Upload)]
        public IFormFile ThumbnailImage { set; get; }
        public bool? IsFeatured { get; set; }
    }
}
