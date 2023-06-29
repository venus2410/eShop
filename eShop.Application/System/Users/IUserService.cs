using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Users
{
    public interface IUserService
    {
        Task<ServiceResult<string>> Login(UserLoginRequest request);
        Task<ServiceResult<bool>> Register(UserCreateRequest request);
        Task<ServiceResult<bool>> Update(UserUpdateRequest request);
        Task<ServiceResult<PageResult<UserViewModel>>> GetUserPaging(UserPagingRequest request);
        Task<ServiceResult<UserViewModel>> GetUserById(Guid id);
        Task<ServiceResult<bool>> Delete(Guid id);
        Task<ServiceResult<bool>> RoleAssign(Guid Id,UserRoleAssignRequest request);
    }
}
