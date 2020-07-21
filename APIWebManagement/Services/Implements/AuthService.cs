using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.ViewModels.Login;
using APIWebManagement.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthService(DataContext dataContext, IConfiguration configuration, UserManager<User> userManager,
        SignInManager<User> signInManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<UserResponseLogin> Login(UserForLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                var userLogin = new UserViewModel
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    Gender = user.Gender,
                    IsActive = user.IsActive,
                    DateOfBirth = user.DateOfBirth,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate
                };

                return new UserResponseLogin { Token = GenerateJWTToken(user).Result, User = userLogin };
            }
            return new UserResponseLogin { };
        }

        private async Task<string> GenerateJWTToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDecriptor);

            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
    }
}
