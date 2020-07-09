using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.Category
{
    public class CategoryUpdateRequest
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}
