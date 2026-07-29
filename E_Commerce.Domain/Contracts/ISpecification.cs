using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface ISpecification<T, TKey> where T : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<T, object>>> IncludeExpreissions { get; }
        public Expression<Func<T,bool>> Criteria { get; }
        public Expression<Func<T, object>>? OrderByAsc { get; }
        public Expression<Func<T, object>>? OrderByDesc { get; }
        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get;}
    }
}
