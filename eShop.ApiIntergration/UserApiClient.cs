using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.ApiIntergration
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IBaseApiClient _baseApiClient;
        private const string baseURL = "/api/users";
        public UserApiClient(IBaseApiClient baseApiClient)
        {
            _baseApiClient= baseApiClient;
        }

        public async Task<ServiceResult<bool>> Create(UserCreateRequest request)
        {
            return await _baseApiClient.PostAsync<bool, UserCreateRequest>(baseURL, request);
        }

        public async Task<ServiceResult<bool>> Delete(Guid Id)
        {
            return await _baseApiClient.DeleteAsync<bool>(baseURL, Id.ToString());
        }

        public async Task<ServiceResult<UserViewModel>> GetById(Guid model)
        {
            return await _baseApiClient.GetByIdAsync<UserViewModel>(baseURL, model.ToString());
        }

        public async Task<ServiceResult<PageResult<UserViewModel>>> GetUsersPaging(UserPagingRequest model)
        {
            string url = $"/api/users/paging?" +
                $"{nameof(model.PageIndex)}={model.PageIndex}" +
                $"&{nameof(model.PageSize)}={model.PageSize}" +
                $"&{nameof(model.Keyword)}={model.Keyword}";
            return await _baseApiClient.GetAllAsync<PageResult<UserViewModel>>(url);
        }

        public async Task<ServiceResult<string>> Login(UserLoginRequest model)
        {
            return await _baseApiClient.LoginAsync<string, UserLoginRequest>(baseURL+"/authenticate", model);
        }

        public async Task<ServiceResult<bool>> Update(UserUpdateRequest request)
        {
            return await _baseApiClient.PutAsync<bool,UserUpdateRequest>(baseURL,request.Id.ToString(), request);
        }
    }
}
