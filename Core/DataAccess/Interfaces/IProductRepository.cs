using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataAccess.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    }
}
