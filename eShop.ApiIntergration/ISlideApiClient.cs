using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Slides;
using eShop.ViewModel.System.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface ISlideApiClient
    {
        Task<ServiceResult<List<SlideVM>>> GetAll();
    }
}
