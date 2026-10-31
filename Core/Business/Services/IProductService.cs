using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Core.Entities.Concrete.Params;

namespace Core.Business.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductDto>> GetProductsPagedAsync(PaginationParams paginationParams);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task AddProductAsync(CreateProductDto productDto);

        Task UpdateProductAsync(UpdateProductDto updateProductDto);

        Task DeleteProductAsync(int id);
    }
}
