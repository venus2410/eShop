using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Languages
{
    public interface ILanguageService
    {
        Task<ServiceResult<List<LanguageVM>>> GetLanguageVMsAsync();
    }
}
