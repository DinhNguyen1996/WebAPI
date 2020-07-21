using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.User
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class UserResponseLogin
    {
        public string Token { get; set; }
        public UserViewModel User { get; set; }
    }
}
