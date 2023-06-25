using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Common
{
    public class ServiceResult<T>
    {
        public bool IsSucceed { get; set; }
        public string Errors { get; set; }
        public T Data { get; set; }
    }
}
