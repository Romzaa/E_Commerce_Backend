using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal abstract class BaseSpecification<T, TKey> : ISpecification<T, TKey> where T : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<T, object>>> IncludeExpreissions { get; } = [];

        public Expression<Func<T, bool>> Criteria { get; } = default!;
        public Expression<Func<T, object>>? OrderByAsc { get; private set;  }
        public Expression<Func<T, object>>? OrderByDesc { get; private set;  }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddPagination(int pageIndex, int pageSize)
        {
            IsPaginated = true;
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
        }
        protected void AddOrderByAsc(Expression<Func<T,object>> orderByAsc)
        {
            OrderByAsc = orderByAsc;

        }
        protected void AddOrderByDesc(Expression<Func<T,object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;

        }
        protected void AddInclude(Expression<Func<T, object>> expression)
        {
             IncludeExpreissions.Add(expression);
        }

       

    }
}
