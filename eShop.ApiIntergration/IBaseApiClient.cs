using eShop.ViewModel.Catalog.Common;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface IBaseApiClient
    {
        Task<ServiceResult<ReturnType>> LoginAsync<ReturnType, Ptype>(string url, Ptype model);
        Task<ServiceResult<ReturnType>> GetGeneralAsync<ReturnType>(string url);
        Task<ServiceResult<ReturnType>> GetByIdAsync<ReturnType>(string url, string id, string languageId="");
        Task<ServiceResult<ReturnType>> PostAsync<ReturnType, Ptype>(string url, Ptype model);
        Task<ServiceResult<ReturnType>> PostWithFileAsync<ReturnType, Ptype>(string url, Ptype model);
        Task<ServiceResult<ReturnType>> PutAsync<ReturnType, Ptype>(string url, string id, Ptype model);
        Task<ServiceResult<ReturnType>> DeleteAsync<ReturnType>(string url, string id);
    }
}
