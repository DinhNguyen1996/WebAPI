using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Category;
using APIWebManagement.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;
        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<PagedResults<Category>> GetAllCategory(GetCategoriesPagingRequest request)
        {
            var query = from c in _dataContext.Categories
                        select c;

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword));
            }
            int totalRows = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();

            var pagedResult = new PagedResults<Category>()
            {
                TotalRecords = totalRows,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = data
            };

            return pagedResult;
        }
       
        public async Task<CategoryViewModel> GetById(int id)
        {
            if (id == 0) throw new WebManagementException("Can not find ID");

            var category = await _dataContext.Categories.FindAsync(id);

            if(category != null)
            {
                var categoryView = new CategoryViewModel
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name
                };

                return categoryView;
            }
            return null;
        }

        public async Task<int> CreateCategory(CategoryCreateRequest categoryCreateRequest)
        {
            if (categoryCreateRequest == null) throw new WebManagementException("Can not create Category");

            var newCategory = new Category
            {
                Name = categoryCreateRequest.Name
            };

            await _dataContext.AddAsync(newCategory);
            await _dataContext.SaveChangesAsync();

            return newCategory.CategoryID;
        }

        public async Task<int> UpdateCategory(CategoryUpdateRequest categoryUpdateRequest)
        {
            var categoryUpdate = await _dataContext.Categories.FirstOrDefaultAsync(x => x.CategoryID == categoryUpdateRequest.CategoryID);
            if (categoryUpdate == null)
                throw new WebManagementException("Can not find category to update");

            _dataContext.Categories.Attach(categoryUpdate);
            categoryUpdate.Name = categoryUpdateRequest.Name;

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<int> DeleteCategory(int id)
        {
            var category = await _dataContext.Categories.FindAsync(id);
            if (category == null)
                throw new WebManagementException("Can not find category");

            _dataContext.Remove(category);

            return await _dataContext.SaveChangesAsync();
        }
    }
}
