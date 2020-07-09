using APIWebManagement.Data.Entities;
using APIWebManagement.ViewModels.Category;
using APIWebManagement.ViewModels.Models;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResults<Category>> GetAllCategory(GetCategoriesPagingRequest request);
        Task<CategoryViewModel> GetById(int id);
        Task<int> CreateCategory(CategoryCreateRequest categoryCreateRequest);
        Task<int> UpdateCategory(CategoryUpdateRequest categoryUpdateRequest);
        Task<int> DeleteCategory(int id);
    }
}
