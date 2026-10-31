using API.ActionFilters;
using Asp.Versioning;
using Core.Business.Services;
using Core.Entities.Concrete.DTOs;
using Core.Entities.Concrete.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController:ControllerBase
    {
        private readonly IBusinessUnitOfWork _uow;

        public ProductsController(IBusinessUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _uow.Product.GetAllProductsAsync();
            return Ok(products);
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
            var product = await _uow.Product.GetProductByIdAsync(id);
            if (product == null) return NotFound(new { message = "Ürün bulunamadı." });
            return Ok(product);
        }
        [Authorize]
        [HttpGet("category/{categoryId}")]

        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _uow.Product.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }


        [Authorize]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> Create(CreateProductDto createProductDto)
        {
            await _uow.Product.AddProductAsync(createProductDto);
            return Ok(new { message = "Ürün başarıyla eklendi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            await _uow.Product.UpdateProductAsync(updateProductDto);
            return Ok(new { message = "Ürün başarıyla güncellendi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            await _uow.Product.DeleteProductAsync(id);
            return Ok(new { message = "Ürün başarıyla silindi." });
        }

    }
}
