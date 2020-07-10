using APIWebManagement.ViewModels.Models;

namespace APIWebManagement.ViewModels.User
{
    public class GetUsersPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
