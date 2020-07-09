using APIWebManagement.Data.Entities;
using APIWebManagement.ViewModels.Models;
using APIWebManagement.ViewModels.Product;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Interfaces
{
    public interface IProductService
    {
        Task<PagedResults<Product>> GetAllProduct(GetProductsPagingRequest request);
        Task<ProductViewModel> GetById(int id);
        Task<int> CreateProduct(ProductCreateRequest productCreateRequest);
        Task<int> UpdateProduct(ProductUpdateRequest productUpdateRequest);
        Task<int> DeleteProduct(int id);
    }
}
