using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Infrastucture.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T>  CreateQuery<T,TKey>(IQueryable<T> inputQuery, ISpecification<T,TKey> specs) where T :BaseEntity<TKey>
        {
            var query = inputQuery;

            if (specs.Criteria is not null)
                query = query.Where(specs.Criteria);

            if(specs.IncludeExpreissions.Any())
             query = specs.IncludeExpreissions.Aggregate(query, (current, next) => current.Include(next));

            if (specs.OrderByAsc is not null)
            {
                query = query.OrderBy(specs.OrderByAsc);
            }
            else if (specs.OrderByDesc is not null) 
            {
                query = query.OrderByDescending(specs.OrderByDesc);
            }

            if (specs.IsPaginated)
            {
                query = query.Skip(specs.Skip).Take(specs.Take);
            }

            return query;
        }
    }
}
