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
        Task<string> Login(LoginModelRequest request);
        Task<bool> Register(RegisterModelRequest request);
        Task<PageResult<UserViewModel>> GetUserPaging(UserPagingRequest request);
    }
}
