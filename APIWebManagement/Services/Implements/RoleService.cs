using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Implements
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _dataContext;
        public RoleService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<RoleResponseView> GetAllRole()
        {
            var roles = from role in _dataContext.Roles
                        select new RoleResponseView
                        {
                            RoleID = role.Id,
                            Name = role.Name
                        };

            return roles;
        }

        public async Task<int> CreateRole(RequestCreateRole request)
        {
            if (request == null)
                throw new WebManagementException("Can not create role");

            var roleExist = await _dataContext.Roles.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (roleExist != null)
                throw new WebManagementException("RoleName is exist in database");

            var newRole = new Role
            {
                Name = request.Name,
                NormalizedName = request.Name.ToUpper()
            };

            await _dataContext.AddAsync(newRole);
            await _dataContext.SaveChangesAsync();

            return newRole.Id;
        }

        public async Task<RoleResponseView> GetRoleById(int id)
        {
            var role = await _dataContext.Roles.FindAsync(id);
            if (role == null)
                throw new WebManagementException("Can not find role");

            var roleView = new RoleResponseView
            {
                RoleID = role.Id,
                Name = role.Name
            };

            return roleView;
        }

        public async Task<int> DeleteRole(int id)
        {
            var role = await _dataContext.Roles.FindAsync(id);
            if (role == null)
                throw new WebManagementException("Can not find role");

            _dataContext.Roles.Remove(role);

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<int> UpdatedRole(RoleUpdateRequest request)
        {
            var roleUpdate = await _dataContext.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleID);
            if (roleUpdate == null)
                throw new WebManagementException("Can not find role");

            roleUpdate.Name = request.Name;
            roleUpdate.NormalizedName = request.Name.ToUpper();

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<RoleResponseView> GetRoleByName(string name)
        {
            var role = await _dataContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (role == null)
                throw new WebManagementException("Can not find role");

            var roleView = new RoleResponseView
            {
                RoleID = role.Id,
                Name = role.Name
            };

            return roleView;
        }
    }
}
