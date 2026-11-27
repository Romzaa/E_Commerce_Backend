using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithTypeBrandSpecification : BaseSpecification<Product, int>
    {
        public ProductWithTypeBrandSpecification(ProductQueryParams queryParams) 
            : base( p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value) 
            && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value) 
            && (string.IsNullOrEmpty( queryParams.SearchValue) || p.Name.ToLower().Contains(queryParams.SearchValue.ToLower())) )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            AddInclude(p => p.Images);
            AddInclude(p => p.Reviews);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderByAsc(p => p.Name);
                        break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                        break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderByAsc(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderByAsc(p => p.Id);
                    break;
            }

            AddPagination(queryParams.PageIndex, queryParams.PageSize);
        }
        public ProductWithTypeBrandSpecification( int Id) : base( p => p.Id == Id)
        {

            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            AddInclude(p => p.Images);
            AddInclude(p => p.Reviews);
        }
    }
}
