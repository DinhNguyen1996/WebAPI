using APIWebManagement.Data.Entities;
using APIWebManagement.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleResponseView> GetAllRole();
        Task<int> CreateRole(RequestCreateRole requestCreateRole);
        Task<RoleResponseView> GetRoleById(int id);
        Task<int> DeleteRole(int id);
        Task<int> UpdatedRole(RoleUpdateRequest roleUpdateRequest);
        Task<RoleResponseView> GetRoleByName(string name);
    }
}
