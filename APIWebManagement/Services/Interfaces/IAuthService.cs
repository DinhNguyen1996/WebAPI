using APIWebManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Login(string username, string password);
    }
}
