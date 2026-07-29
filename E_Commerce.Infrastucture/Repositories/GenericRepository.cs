using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastucture.Data;
using E_Commerce.Infrastucture.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Repositories
{
    internal class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }
        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        {
           return await _dbContext.Set<T>().ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(TKey Id, CancellationToken ct = default)
        {
            return await _dbContext.Set<T>().FindAsync(Id, ct);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T, TKey> specs, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery<T, TKey>(_dbContext.Set<T>(), specs);
            return await query.ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(ISpecification<T, TKey> specs, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery<T, TKey>(_dbContext.Set<T>(), specs);
            return await query.FirstOrDefaultAsync(ct);
        }

        public async Task<int> CountAsync(ISpecification<T, TKey> specs, CancellationToken ct = default)
        {
            var query =  SpecificationEvaluator.CreateQuery<T, TKey>(_dbContext.Set<T>(), specs);
            return await query.CountAsync(ct);
        }
    }
}
