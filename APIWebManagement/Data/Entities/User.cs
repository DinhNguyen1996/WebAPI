using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace APIWebManagement.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
