using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.User
{
    public class UserUpdateRequest
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
