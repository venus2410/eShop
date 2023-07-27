using eShop.Data.Entities;
using eShop.Utilities.Exceptions;
using eShop.ViewModel.Catalog.Common;
using eShop.ViewModel.Catalog.Products;
using eShop.ViewModel.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices.WindowsRuntime;

namespace eShop.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ServiceResult<bool>> Delete(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return new ServiceResultFail<bool>("Không tìm thấy người dùng");
                }
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return new ServiceResultSuccess<bool>();
                }
                return new ServiceResultFail<bool>("Xóa không thành công");
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<UserViewModel>> GetUserById(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return new ServiceResultFail<UserViewModel>("Không tìm thấy người dùng");
                }
                var rolesOfUSer = (List<string>)await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.Select(r => new SelectItem
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    Selected = rolesOfUSer.Any(x => x == r.Name)
                }).ToList();
                var result = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    UserName = user.UserName,
                    Dob = user.Dob,
                    Roles = allRoles
                };
                return new ServiceResultSuccess<UserViewModel>(result);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<UserViewModel>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<PageResult<UserViewModel>>> GetUserPaging(UserPagingRequest request)
        {
            try
            {
                var query = _userManager.Users;
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(m => EF.Functions.Like(m.UserName, $"%{request.Keyword}%")
                        || EF.Functions.Like(m.Email, $"%{request.Keyword}%")
                        || EF.Functions.Like(m.PhoneNumber, $"%{request.Keyword}%")
                    );
                }
                //paging
                int totalRecord = await query.CountAsync();

                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new UserViewModel()
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        PhoneNumber = x.PhoneNumber,
                        UserName = x.UserName,
                        Email = x.Email
                    }).ToListAsync();

                // return data
                var result = new PageResult<UserViewModel>()
                {
                    Items = data,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = totalRecord
                };

                return new ServiceResultSuccess<PageResult<UserViewModel>>(result);
            }
            catch (Exception e)
            {
                return new ServiceResultFail<PageResult<UserViewModel>>(e.Message.ToString());
            }


        }

        public async Task<ServiceResult<bool>> IsValidEmail(string email, Guid? Id)
        {
            bool result;
            if (Id != null)
            {
                result = !await _userManager.Users.AnyAsync(x => x.Id != Id && x.Email == email);
                return new ServiceResult<bool> { IsSucceed = result, Data = result };
            }
            result = !await _userManager.Users.AnyAsync(x => x.Email == email);
            return new ServiceResult<bool> { IsSucceed = result, Data = result };
        }

        public async Task<ServiceResult<bool>> IsValidUserName(string userName, Guid? Id)
        {
            bool result;
            if (Id != null)
            {
                result = !await _userManager.Users.AnyAsync(x => x.Id != Id && x.UserName == userName);
                return new ServiceResult<bool> { IsSucceed = result, Data = result };
            }
            result = !await _userManager.Users.AnyAsync(x => x.UserName == userName);
            return new ServiceResult<bool> { IsSucceed = result, Data = result };
        }

        public async Task<ServiceResult<string>> Login(UserLoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    return new ServiceResultFail<string>("Sai tên đăng nhập hoặc mật khẩu");
                }

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
                if (!result.Succeeded)
                {
                    return new ServiceResultFail<string>("Sai tên đăng nhập hoặc mật khẩu");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                    _configuration["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds);

                return new ServiceResultSuccess<string>(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception e)
            {
                return new ServiceResultFail<string>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<bool>> Register(UserCreateRequest request)
        {
            try
            {
                if (await _userManager.Users.AnyAsync(u => u.UserName == request.UserName))
                {
                    return new ServiceResultFail<bool>("Tên đăng nhập đã tồn tại");
                }
                if (await _userManager.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return new ServiceResultFail<bool>("Email đã tồn tại");
                }

                var user = new AppUser()
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Dob = request.Dob
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + "\n";
                    }
                    return new ServiceResultFail<bool>(errors);
                }

                //create role for user
                var roles = request.Roles.Where(r => r.Selected).Select(r => r.Name).ToList();
                await _userManager.AddToRolesAsync(user, roles);

                return new ServiceResultSuccess<bool>();
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<bool>> RoleAssign(Guid Id, UserRoleAssignRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id.ToString());
                if (user == null)
                {
                    return new ServiceResultFail<bool>("Không tìm thấy người dùng");
                }
                foreach (var role in request.Roles)
                {
                    if (role.Selected)
                    {
                        if (await _userManager.IsInRoleAsync(user, role.Name) == false)
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }
                    }
                    else
                    {
                        if (await _userManager.IsInRoleAsync(user, role.Name) == true)
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                    }
                }
                return new ServiceResultSuccess<bool>();
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }
        }

        public async Task<ServiceResult<bool>> Update(UserUpdateRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return new ServiceResultFail<bool>("Không tìm thấy người dùng");
                }

                if (await _userManager.Users.AnyAsync(u => u.Email == request.Email && u.Id != request.Id))
                {
                    return new ServiceResultFail<bool>("Email đã tồn tại");
                }

                //update user
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;
                user.Dob = request.Dob;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new ServiceResultFail<bool>("Cập nhật không thành công");
                }

                //update roles of user
                var existingRoles = await _userManager.GetRolesAsync(user);
                var newRoles = request.Roles.Where(r => r.Selected).Select(r => r.Name).ToList();
                foreach (var r in existingRoles)
                {
                    if (!newRoles.Contains(r))
                    {
                        await _userManager.RemoveFromRoleAsync(user, r);
                    }
                }
                foreach (var r in newRoles)
                {
                    if (!existingRoles.Contains(r))
                    {
                        await _userManager.AddToRoleAsync(user, r);
                    }
                }

                return new ServiceResultSuccess<bool>();
            }
            catch (Exception e)
            {
                return new ServiceResultFail<bool>(e.Message.ToString());
            }

        }
    }
}
