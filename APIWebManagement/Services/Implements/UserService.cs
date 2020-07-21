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
        public UserService(DataContext dataContext, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
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
                IsActive = true,
                DateOfBirth = userCreateRequest.DateOfBirth,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(newUser, userCreateRequest.Password);
            if (result.Succeeded)
            {
                return newUser.Id;
            }

            return 0;
        }

        public async Task<PagedResults<UserViewModel>> GetAllUser(GetUsersPagingRequest request)
        {
            var query = from user in _dataContext.Users
                        select user;

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Gender.Contains(request.Keyword));
            }
            int totalRows = await query.CountAsync();
            var dataUsers = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();

            var lstUsers = new List<UserViewModel>();
            if (dataUsers.Count > 0)
            {
                foreach (var item in dataUsers)
                {
                    var userView = new UserViewModel
                    {
                        UserID = item.Id,
                        UserName = item.UserName,
                        Gender = item.Gender,
                        IsActive = item.IsActive,
                        DateOfBirth = item.DateOfBirth,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate
                    };
                    lstUsers.Add(userView);
                }
            }

            var pagedResult = new PagedResults<UserViewModel>()
            {
                TotalRecords = totalRows,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = lstUsers
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
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate
            };
            return userView;
        }

        public async Task<int> UpdateUser(UserUpdateRequest userUpdateRequest)
        {
            var userUpdate = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == userUpdateRequest.UserID);
            if (userUpdate == null)
                throw new WebManagementException("Can not find User");

            userUpdate.UserName = userUpdateRequest.UserName;
            userUpdate.Gender = userUpdateRequest.Gender;
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
