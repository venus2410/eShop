using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class ProductUpdateRequest:ProductCreateRequest
    {
        public int Id { set; get; }
    }
}
