using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.User
{
    public class UserWithRoles
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
