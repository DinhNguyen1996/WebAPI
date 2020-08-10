using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIWebManagement.Utilities
{
    public class Helpers
    {
        public static string ConvertListErrorString(List<IdentityError> results)
        {
            StringBuilder stringErrors = new StringBuilder();
            foreach (var item in results)
            {
                stringErrors.Append("- " + item.Description + "</br>");
            }
            return stringErrors.ToString();
        }
    }
}
