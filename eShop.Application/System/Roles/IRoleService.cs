using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Roles
{
    public interface IRoleService
    {
        Task<ServiceResult<List<RoleVM>>> GetAll();
        Task<ServiceResult<bool>> Create(RoleVM model);
    }
}
