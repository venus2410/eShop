using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminApp.Services
{
    public interface IRoleApiClient
    {
        public Task<ServiceResult<List<RoleVM>>> GetAll();
    }
}
