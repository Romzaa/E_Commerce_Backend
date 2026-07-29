using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<T?> GetByIdAsync(TKey Id, CancellationToken ct = default);
        Task<T?> GetByIdAsync(ISpecification<T, TKey> specs, CancellationToken ct = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T,TKey> specs,CancellationToken ct = default);
        Task<int> CountAsync(ISpecification<T, TKey> specs, CancellationToken ct = default);
    }
}
