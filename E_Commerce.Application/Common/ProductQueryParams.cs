using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SearchValue { get; set; } 
        public ProductSortingOptions Sort {  get; set; }


        // Pagination
        private const int MaxPageSize = 50;
        private const int DefaultPageSize = 12;

        public int PageIndex { get;  set; } = 1;
        private int pageSize = DefaultPageSize;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ?  MaxPageSize : (value < 11 ?  DefaultPageSize :  value ); 
        }
    }
}
