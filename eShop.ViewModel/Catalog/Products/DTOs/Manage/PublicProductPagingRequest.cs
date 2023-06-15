using eShop.ViewModel.Catalog.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Products.DTOs.Manage
{
    public class PublicProductPagingRequest : PagingRequestBase
    {
        public int CategoryIds { get; set; }
    }
}
