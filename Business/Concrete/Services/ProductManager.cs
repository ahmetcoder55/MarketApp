using AutoMapper;
using Core.Business.Services;
using Core.DataAccess.Interfaces;
using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Core.Entities.Concrete.Params;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete.Services
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        private const string ProductCacheKey = "productListCacheKey";

        public ProductManager(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task AddProductAsync(CreateProductDto productDto)
        {
            if (productDto != null)
            {
                var product = _mapper.Map<Product>(productDto);
                await _unitOfWork.ProductRepository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                _memoryCache.Remove(ProductCacheKey);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new NullReferenceException("Ürün Bulunamadı.");
            }
            _unitOfWork.ProductRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync();

            _memoryCache.Remove(ProductCacheKey);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            if (_memoryCache.TryGetValue(ProductCacheKey, out IEnumerable<ProductDto> cachedProducts))
            {
                return cachedProducts;
            }

            var products = await _unitOfWork.ProductRepository.GetAllAsync();

            if (products == null || !products.Any())
            {
                return Enumerable.Empty<ProductDto>();
            }

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _memoryCache.Set(ProductCacheKey, productDtos, cacheOptions);

            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            return _mapper.Map<ProductDto>(await _unitOfWork.ProductRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return _mapper.Map<IEnumerable<ProductDto>>(await _unitOfWork.ProductRepository.GetProductsByCategoryAsync(categoryId));
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var product = _mapper.Map<Product>(updateProductDto);
            _unitOfWork?.ProductRepository.Update(product); // Null-safe koruması veya standart kullanım
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            _memoryCache.Remove(ProductCacheKey);
        }

        public async Task<PagedResult<ProductDto>> GetProductsPagedAsync(PaginationParams paginationParams)
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();

            if (products == null || !products.Any())
            {
                return new PagedResult<ProductDto>(Enumerable.Empty<ProductDto>(), 0, paginationParams.PageNumber, paginationParams.PageSize);
            }

            var totalCount = products.Count(); 

            var pagedProducts = products
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(pagedProducts);

            return new PagedResult<ProductDto>(productDtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}