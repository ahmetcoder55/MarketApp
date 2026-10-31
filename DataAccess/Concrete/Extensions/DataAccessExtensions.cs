using Core.DataAccess.Interfaces;
using DataAccess.Concrete.Data.Context;
using DataAccess.Concrete.Repositories;
using DataAccess.Concrete.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.Extensions
{
    public static class DataAccessExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
        }
        public static void AddDataAccess(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
