using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services
{
    public interface IBusinessUnitOfWork
    {
        public ICategoryService Category { get; }

        public IProductService Product { get; }

        public IAuthenticationService Authentication { get; }



    }
}
