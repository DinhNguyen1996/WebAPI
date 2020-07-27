using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Models;
using APIWebManagement.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedResults<UserViewModel>> GetAllUser(GetUsersPagingRequest request);
        Task<UserViewModel> GetUserById(int id);
        Task<int> CreateUser(UserCreateRequest userCreateRequest);
        Task<int> UpdateUser(UserUpdateRequest userUpdateRequest);
        Task<int> DeleteUser(int id);
        Task<bool> UserExist(string username);
        Task<List<UserWithRoles>> GetUserWithRoles();
        Task<List<string>> EditRoleUser(string userName, UserEditRoleRequest request);
    }
}
