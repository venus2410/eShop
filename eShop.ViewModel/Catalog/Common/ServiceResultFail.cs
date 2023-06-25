using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Common
{
    public class ServiceResultFail<T>:ServiceResult<T>
    {
        public ServiceResultFail()
        {
            IsSucceed= false;
        }
        public ServiceResultFail(string errorList)
        {
            IsSucceed= false;
            Errors= errorList;
        }
    }
}
