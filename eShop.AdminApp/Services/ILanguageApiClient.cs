using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using eShop.ViewModel.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminApp.Services
{
    public interface ILanguageApiClient
    {
        Task<ServiceResult<List<LanguageVM>>> GetLanguages();
    }
}
