using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModel.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        [Required]
        public string Caption { get; set; }
        [Required]
        public bool IsDefault { get; set; }
        [Required]
        public int SortOrder { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public List<IFormFile> ImageFiles { get; set; }
    }
}
