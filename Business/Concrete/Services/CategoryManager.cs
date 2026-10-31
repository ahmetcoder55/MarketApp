using AutoMapper;
using Core.Business.Services;
using Core.DataAccess.Interfaces;
using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Core.Entities.Concrete.Params;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete.Services
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        private const string CategoryCacheKey = "categoryListCacheKey";

        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            _memoryCache.Remove(CategoryCacheKey);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id); 
            if (category == null)
            {
                throw new NullReferenceException("Kategori Bulunamadı.");
            }
            _unitOfWork.CategoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            _memoryCache.Remove(CategoryCacheKey);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            if (_memoryCache.TryGetValue(CategoryCacheKey, out IEnumerable<CategoryDto> cachedCategories))
            {
                return cachedCategories;
            }

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories == null || !categories.Any())
            {
                return Enumerable.Empty<CategoryDto>();
            }

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _memoryCache.Set(CategoryCacheKey, categoryDtos, cacheOptions);

            return categoryDtos;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            return _mapper.Map<CategoryDto>(await _unitOfWork.CategoryRepository.GetByIdAsync(id));
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            _memoryCache.Remove(CategoryCacheKey);
        }

        public async Task<PagedResult<CategoryDto>> GetCategoriesPagedAsync(PaginationParams paginationParams)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories == null || !categories.Any())
            {
                return new PagedResult<CategoryDto>(Enumerable.Empty<CategoryDto>(), 0, paginationParams.PageNumber, paginationParams.PageSize);
            }

            var totalCount = categories.Count(); 

            var pagedCategories = categories
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(pagedCategories);

            return new PagedResult<CategoryDto>(categoryDtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}