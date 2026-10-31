using API.ActionFilters;
using Asp.Versioning;
using Core.Business.Services;
using Core.Entities.Concrete.DTOs;
using Core.Entities.Concrete.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IBusinessUnitOfWork _uow;

        public CategoriesController(IBusinessUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _uow.Category.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [Authorize]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams paginationParams)
        {
            var result = await _uow.Product.GetProductsPagedAsync(paginationParams);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var category = await _uow.Category.GetCategoryByIdAsync(id);
            if (category == null) return NotFound(new { message = "Kategori bulunamadı." });
            return Ok(category);
        }

        [Authorize]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            await _uow.Category.AddCategoryAsync(categoryDto);
            return Ok(new { message = "Kategori başarıyla eklendi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            await _uow.Category.UpdateCategoryAsync(categoryDto);
            return Ok(new { message = "Kategori başarıyla güncellendi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            await _uow.Category.DeleteCategoryAsync(id);
            return Ok(new { message = "Kategori başarıyla silindi." });
        }
    }
}
