using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Categories
{
    public interface ICategoriesService
    {
        Task<ServiceResult<List<CategoryVM>>> GetAll(string languageId);
        Task<ServiceResult<CategoryVM>> GetById(int categoryId, string languageId);
        Task<ServiceResult<CategoryUpdateVM>> GetForUpdate(int categoryId);
        Task<ServiceResult<bool>> Create(CategoryCreateVM model);
        Task<ServiceResult<bool>> Update(CategoryUpdateVM model);
        Task<ServiceResult<bool>> Delete(int categoryId);
    }
}
