using eShop.ViewModel.Catalog.Common;
using System.Threading.Tasks;

namespace eShop.AdminApp.Services
{
    public interface IBaseApiClient
    {
        Task<ServiceResult<ReturnType>> LoginAsync<ReturnType, Ptype>(string url, Ptype model);
        Task<ServiceResult<ReturnType>> GetAllAsync<ReturnType>(string url);
        Task<ServiceResult<ReturnType>> GetByIdAsync<ReturnType>(string url, string id);
        Task<ServiceResult<ReturnType>> PostAsync<ReturnType, Ptype>(string url, Ptype model);
        Task<ServiceResult<ReturnType>> PostWithFileAsync<ReturnType, Ptype>(string url, Ptype model);
        Task<ServiceResult<ReturnType>> PutAsync<ReturnType, Ptype>(string url, string id, Ptype model);
        Task<ServiceResult<ReturnType>> DeleteAsync<ReturnType>(string url, string id);
    }
}
