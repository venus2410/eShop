using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.Catalog.Common
{
    public class ServiceResultSuccess<T> : ServiceResult<T>
    {
        public ServiceResultSuccess()
        {
            IsSucceed = true;
        }
        public ServiceResultSuccess(T data)
        {
            IsSucceed = true;
            Data = data;
        }
    }
}
