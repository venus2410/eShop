﻿using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.System.Users;
using System;
using System.Threading.Tasks;

namespace eShop.ApiIntergration
{
    public interface IUserApiClient
    {
        public Task<ServiceResult<string>> Login(UserLoginRequest model);
        public Task<ServiceResult<PageResult<UserViewModel>>> GetUsersPaging(UserPagingRequest model);
        public Task<ServiceResult<UserViewModel>> GetById(Guid Id);
        public Task<ServiceResult<bool>> Create(UserCreateRequest request);
        public Task<ServiceResult<bool>> Update(UserUpdateRequest request);
        public Task<ServiceResult<bool>> Delete(Guid Id);
        public Task<ServiceResult<bool>> IsValidUserName(string userName,Guid? id);
        public Task<ServiceResult<bool>> IsValidEmail(string email, Guid? id);
    }
}
