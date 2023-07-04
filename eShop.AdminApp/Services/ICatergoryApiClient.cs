using eShop.ViewModel.Catalog.Catergories;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminApp.Services
{
    public interface ICatergoryApiClient
    {
        Task<ServiceResult<List<CatergoryVM>>> GetCatergories(string languageId);
    }
}
