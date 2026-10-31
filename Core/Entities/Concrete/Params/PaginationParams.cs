using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete.Params
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1; // Varsayılan 1. sayfa

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : (value < 1 ? 1 : value);
        }
    }
}
