using Core.DataAccess.Interfaces;
using Core.Entities.Concrete;
using DataAccess.Concrete.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.Repositories
{
    public class CategoryRepository : GenericRepository<AppDbContext, Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
