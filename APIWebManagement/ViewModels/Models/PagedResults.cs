using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.Models
{
    public class PagedResults<T> : PagedResultBase
    {
        public List<T> Data { get; set; }
    }
}
