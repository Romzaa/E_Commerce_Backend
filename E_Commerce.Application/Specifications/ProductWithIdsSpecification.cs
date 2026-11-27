using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithIdsSpecification : BaseSpecification<Product, int>
    {
        public ProductWithIdsSpecification(HashSet<int> ids) : base(p => ids.Contains(p.Id))
        {
        }
    }
}
