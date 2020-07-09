using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Utilities
{
    public class WebManagementException : Exception
    {
        public WebManagementException()
        {
        }

        public WebManagementException(string message) : base(message)
        {
        }

        public WebManagementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
