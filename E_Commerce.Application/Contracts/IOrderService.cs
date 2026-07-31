using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDto, string email, CancellationToken ct = default);
    }
}
