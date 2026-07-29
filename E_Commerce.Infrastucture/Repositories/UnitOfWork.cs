using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastucture.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<string, object> Repositories = [];
        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>
        {
            var typeName = typeof(T).Name;
            if (Repositories.TryGetValue(typeName, out var repo))
                return (IGenericRepository<T,TKey>)repo;

            var newRepo = new GenericRepository<T, TKey>(_dbContext);
            Repositories[typeName] = newRepo;
            return newRepo;
            
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
