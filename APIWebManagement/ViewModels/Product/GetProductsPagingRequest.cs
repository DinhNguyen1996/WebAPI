using APIWebManagement.ViewModels.Models;

namespace APIWebManagement.ViewModels.Product
{
    public class GetProductsPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
