using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Users;
using System.Threading.Tasks;

namespace eShop.AdminApp.Services
{
    public interface IUserApiClient
    {
        public Task<string> Login(LoginModelRequest model);
        public Task<PageResult<UserViewModel>> GetUsersPaging(UserPagingRequest model);
    }
}
