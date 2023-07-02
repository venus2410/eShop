using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Categories
{
    public interface ICatergoriesService
    {
        Task<ServiceResult<List<CatergoryVM>>> GetAll(string languageId);
    }
}
