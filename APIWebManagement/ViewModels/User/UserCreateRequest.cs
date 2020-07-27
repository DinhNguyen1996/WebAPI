using System;
using System.ComponentModel.DataAnnotations;

namespace APIWebManagement.ViewModels.User
{
    public class UserCreateRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Gender { get; set; }
        public string RoleName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
