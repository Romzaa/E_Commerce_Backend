using E_Commerce.Application.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IBrandsTypesService
    {
        public Task<Result<bool>> AddBrandAsync(string brandName, CancellationToken ct = default);

        public Task<Result<bool>> AddTypeAsync(string typeName, CancellationToken ct = default);

        public Task<Result<bool>> RemoveBrandAsync(int brandId, CancellationToken ct = default);

        public Task<Result<bool>> RemoveTypeAsync(int typeId, CancellationToken ct = default);
        
    }
}
