using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.System.Users
{
    public class UserPagingRequest:PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
