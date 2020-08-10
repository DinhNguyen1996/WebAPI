using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Models;
using APIWebManagement.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        public UserService(DataContext dataContext, UserManager<User> userManager, IRoleService roleService)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _roleService = roleService;

        }

        public async Task<int> CreateUser(UserCreateRequest userCreateRequest)
        {
            if (userCreateRequest == null)
                throw new WebManagementException("Can not create User");

            var isUserExist = await _dataContext.Users.AnyAsync(x => x.UserName == userCreateRequest.UserName);
            if (isUserExist) throw new WebManagementException("Username is exist in database");

            var newUser = new User
            {
                UserName = userCreateRequest.UserName,
                PasswordHash = userCreateRequest.Password,
                Gender = userCreateRequest.Gender,
                Email = userCreateRequest.Email,
                IsActive = true,
                DateOfBirth = userCreateRequest.DateOfBirth,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(newUser, userCreateRequest.Password);
            if (!string.IsNullOrEmpty(userCreateRequest.RoleName))
            {
                //var checkRole = _roleManager.RoleExistsAsync(roleName);
                await _userManager.AddToRoleAsync(newUser, userCreateRequest.RoleName);
            }

            if (result.Succeeded)
            {
                return newUser.Id;
            }

            return 0;
        }

        public async Task<PagedResults<UserViewModel>> GetAllUser(GetUsersPagingRequest request)
        {
            var query = from u in _dataContext.Users
                        select new UserViewModel
                        {
                            UserID = u.Id,
                            UserName = u.UserName,
                            Gender = u.Gender,
                            IsActive = u.IsActive,
                            Email = u.Email,
                            DateOfBirth = u.DateOfBirth,
                            CreatedDate = u.CreatedDate,
                            UpdatedDate = u.UpdatedDate,
                        };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Gender.Contains(request.Keyword));
            }
            int totalRows = await query.CountAsync();
            var dataUsers = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();

            var pagedResult = new PagedResults<UserViewModel>()
            {
                TotalRecords = totalRows,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = dataUsers
            };

            return pagedResult;
        }

        public async Task<UserViewModel> GetUserById(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if (user == null)
                throw new WebManagementException("Can not find User");

            var userView = new UserViewModel
            {
                UserID = user.Id,
                UserName = user.UserName,
                Gender = user.Gender,
                Email = user.Email,
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate
            };
            return userView;
        }

        public async Task<int> UpdateUser(UserUpdateRequest userUpdateRequest)
        {
            var userUpdate = await _userManager.FindByIdAsync(userUpdateRequest.UserID.ToString());
            if (userUpdate == null)
                throw new WebManagementException("Can not find User");

            userUpdate.UserName = userUpdateRequest.UserName.Trim();
            userUpdate.Gender = userUpdateRequest.Gender.Trim();
            userUpdate.Email = userUpdateRequest.Email.Trim();
            userUpdate.NormalizedEmail = userUpdateRequest.Email.ToUpper().Trim();
            userUpdate.IsActive = userUpdateRequest.IsActive;
            userUpdate.DateOfBirth = userUpdateRequest.DateOfBirth;
            userUpdate.UpdatedDate = DateTime.Now;

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<int> DeleteUser(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if (user == null)
                throw new WebManagementException("Can not find User");

            _dataContext.Users.Remove(user);

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<bool> UserExist(string username)
        {
            if (await _dataContext.Users.AnyAsync(x => x.UserName == username))
                return true;

            return false;
        }

        public async Task<List<UserWithRoles>> GetUserWithRoles()
        {
            var userRoles = await (from u in _dataContext.Users
                                   select new UserWithRoles
                                   {
                                       UserID = u.Id,
                                       UserName = u.UserName,
                                       Roles = (from userRole in u.UserRoles
                                                join role in _dataContext.Roles on userRole.RoleId equals role.Id
                                                select role.Name).ToList()
                                   }).ToListAsync();

            return userRoles;
        }

        public async Task<List<string>> EditRoleUser(string userName, UserEditRoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = request.RoleNames;

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                throw new WebManagementException(Helpers.ConvertListErrorString(result.Errors.ToList()));

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                throw new WebManagementException(Helpers.ConvertListErrorString(result.Errors.ToList()));

            var lstRoles = new List<string>(await _userManager.GetRolesAsync(user));

            return lstRoles;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


    }
}
