﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.Role
{
    public class RoleUpdateRequest
    {
        public long RoleID { get; set; }
        public string Name { get; set; }
    }
}
