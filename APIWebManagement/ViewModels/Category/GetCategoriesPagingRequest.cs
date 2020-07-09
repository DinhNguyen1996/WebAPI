using APIWebManagement.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.ViewModels.Category
{
    public class GetCategoriesPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
