using Core.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete.Services
{
    public class BusinessUnitOfWork : IBusinessUnitOfWork
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IAuthenticationService _authenticationService;

        public BusinessUnitOfWork(ICategoryService categoryService, IProductService productService, IAuthenticationService authenticationService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _authenticationService = authenticationService;
        }

        public ICategoryService Category => _categoryService;

        public IProductService Product => _productService;

        public IAuthenticationService Authentication => _authenticationService;
    }
}
