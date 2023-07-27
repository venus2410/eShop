using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface ICategoryApiClient
    {
        Task<ServiceResult<List<CategoryVM>>> GetCatergories(string languageId);
        Task<ServiceResult<CategoryVM>> GetById(int categoryId,string languageId);
        Task<ServiceResult<bool>> Create(CategoryCreateVM model);
        Task<ServiceResult<bool>> Update(CategoryUpdateVM model);
        Task<ServiceResult<CategoryUpdateVM>> GetForUpdate(int categoryId);
    }
}
