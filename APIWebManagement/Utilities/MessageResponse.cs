using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Utilities
{
    public class MessageResponse
    {
        public string Message { get; set; }
        public MessageResponse(string message)
        {
            Message = message;
        }
    }
}
