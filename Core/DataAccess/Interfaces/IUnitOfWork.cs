using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataAccess.Interfaces
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        public IProductRepository ProductRepository { get;}

        public ICategoryRepository CategoryRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
