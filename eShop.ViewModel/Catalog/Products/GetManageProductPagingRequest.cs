using eShop.ViewModel.Catalog.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
