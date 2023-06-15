using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Common
{
    public class PageResult<Ttype>
    {
        public List<Ttype> Items { get;set; }
        public int TotalRecord { get; set; }
    }
}
