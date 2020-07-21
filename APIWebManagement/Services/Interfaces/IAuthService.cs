using APIWebManagement.ViewModels.Login;
using APIWebManagement.ViewModels.User;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserForLoginRequest request);
    }
}
