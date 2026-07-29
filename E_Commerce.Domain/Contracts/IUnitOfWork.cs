using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
