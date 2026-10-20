using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class BrandsTypesService : IBrandsTypesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandsTypesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> AddBrandAsync(string brandName, CancellationToken ct = default)
        {
            var brandsRepo = _unitOfWork.GetRepository<ProductBrand, int>();
            var brands = await brandsRepo.GetAllAsync(ct);
            var brand = brands.FirstOrDefault(b => b.Name == brandName);
            if (brand != null)
            {
                return Result<bool>.Fail(Error.Conflict("Brand.Conflict", $"Brand:{brandName} Already Exists"));
            }

            brandsRepo.Add(new ProductBrand { Name = brandName });
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Brand.Failure", $"Failed To Add Brand:{brandName}"));
        }


        public async Task<Result<bool>> AddTypeAsync(string typeName, CancellationToken ct = default)
        {
            var typesRepo = _unitOfWork.GetRepository<ProductType, int>();
            var types = await typesRepo.GetAllAsync(ct);
            var type = types.FirstOrDefault(t => t.Name == typeName);
            if (type != null)
            {
                return Result<bool>.Fail(Error.Conflict("Type.Conflict", $"Type:{typeName} Already Exists"));
            }
            typesRepo.Add(new ProductType { Name = typeName });
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Type.Failure", $"Failed To Add Type:{typeName}"));
        }


        public async Task<Result<bool>> RemoveBrandAsync(int brandId, CancellationToken ct = default)
        {
            var brandsRepo = _unitOfWork.GetRepository<ProductBrand, int>();
            var brand = await brandsRepo.GetByIdAsync(brandId, ct);
            if (brand == null)
            {
                return Result<bool>.Fail(Error.NotFound("Brand.NotFound", $"Brand With Id:{brandId} Not Found"));
            }
            brandsRepo.Remove(brand);
            var result = _unitOfWork.SaveChangesAsync(ct).Result;
            return result > 0 ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Brand.Failure", $"Failed To Remove Brand With Id:{brandId}"));
        }

        public async Task<Result<bool>> RemoveTypeAsync(int typeId, CancellationToken ct = default)
        {
            var typesRepo = _unitOfWork.GetRepository<ProductType, int>();
            var type = await typesRepo.GetByIdAsync(typeId, ct);
            if (type == null)
            {
                return Result<bool>.Fail(Error.NotFound("Type.NotFound", $"Type With Id:{typeId} Not Found"));
            }
            typesRepo.Remove(type);
            var result = _unitOfWork.SaveChangesAsync(ct).Result;
            return result > 0 ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Type.Failure", $"Failed To Remove Type With Id:{typeId}"));
        }


    }
}
