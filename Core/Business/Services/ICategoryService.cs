using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Core.Entities.Concrete.Params;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryDto>> GetCategoriesPagedAsync(PaginationParams paginationParams);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryDto categoryDto);
        Task DeleteCategoryAsync(int id);

        Task UpdateCategoryAsync(CategoryDto categoryDto);
    }
}
