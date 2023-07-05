using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Slides
{
    public interface ISlidesService
    {
        Task<ServiceResult<List<SlideVM>>> GetAll();
    }
}
