using APIWebManagement.Data.Entities;
using APIWebManagement.Services.Interfaces;
using APIWebManagement.Utilities;
using APIWebManagement.ViewModels.Models;
using APIWebManagement.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly DataContext _dataContext;
        public ProductService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<PagedResults<Product>> GetAllProduct(GetProductsPagingRequest request)
        {
            var query = from pro in _dataContext.Products
                        select pro;

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Code.Contains(request.Keyword) || x.Name.Contains(request.Keyword));
            }
            int totalRows = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Include(x => x.Category).ToListAsync();

            var pagedResult = new PagedResults<Product>()
            {
                TotalRecords = totalRows,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Data = data
            };

            return pagedResult;
        }

        public async Task<int> CreateProduct(ProductCreateRequest productCreateRequest)
        {
            if (productCreateRequest == null) throw new WebManagementException("Can not find request");

            var existProduct = await _dataContext.Products.FirstOrDefaultAsync(x => x.Code == productCreateRequest.Code);
            if (existProduct != null)
                throw new WebManagementException("Product has Code which is existed on database");

            var newProduct = new Product
            {
                Code = productCreateRequest.Code,
                Name = productCreateRequest.Name,
                SalePrice = productCreateRequest.SalePrice,
                OriginalPrice = productCreateRequest.OriginalPrice,
                CreatedDate = DateTime.Now,
                CategoryID = productCreateRequest.CategoryID
            };

            await _dataContext.AddAsync(newProduct);
            await _dataContext.SaveChangesAsync();

            return newProduct.ProductID;
        }

        public async Task<ProductViewModel> GetById(int id)
        {
            var product = await _dataContext.Products.FindAsync(id);
            if (product == null)
                throw new WebManagementException("Can not find product");

            var productView = new ProductViewModel
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Code = product.Code,
                SalePrice = product.SalePrice,
                OriginalPrice = product.OriginalPrice,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                CategoryID = product.CategoryID
            };

            return productView;
        }

        public async Task<int> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            if (productUpdateRequest.CategoryID == 0)
                throw new WebManagementException("Can not find ID Product");

            var productUpdate = await _dataContext.Products.FirstOrDefaultAsync(x => x.ProductID == productUpdateRequest.ProductID);
            if (productUpdate == null)
                throw new WebManagementException("Can not find product to update");

            productUpdate.Name = productUpdateRequest.Name;
            productUpdate.SalePrice = productUpdateRequest.SalePrice;
            productUpdate.OriginalPrice = productUpdateRequest.OriginalPrice;
            productUpdate.UpdatedDate = DateTime.Now;
            productUpdate.CategoryID = productUpdateRequest.CategoryID;

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(int id)
        {
            var product = await _dataContext.Products.FindAsync(id);
            if (product == null)
                throw new WebManagementException("Can not find product");

            _dataContext.Remove(product);

            return await _dataContext.SaveChangesAsync();
        }
    }
}
