using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface ICatergoryApiClient
    {
        Task<ServiceResult<List<CatergoryVM>>> GetCatergories(string languageId);
        Task<ServiceResult<CatergoryVM>> GetById(int categoryId,string languageId);
    }
}
