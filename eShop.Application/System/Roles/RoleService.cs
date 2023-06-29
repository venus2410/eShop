using eShop.Data.Entities;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _rolemaManager;
        public RoleService(RoleManager<AppRole> rolemaManager)
        {
            _rolemaManager = rolemaManager;
        }

        public async Task<ServiceResult<List<RoleVM>>> GetAll()
        {
            var roles =await _rolemaManager.Roles.Select(m => new RoleVM
            {
                Id= m.Id,
                Name= m.Name,
                Description= m.Description
            }).ToListAsync();
            return new ServiceResultSuccess<List<RoleVM>>(roles);
        }
    }
}
