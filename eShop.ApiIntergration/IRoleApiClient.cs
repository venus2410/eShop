using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface IRoleApiClient
    {
        public Task<ServiceResult<List<RoleVM>>> GetAll();
        public Task<ServiceResult<bool>> Create(RoleVM model);
    }
}
