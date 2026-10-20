using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork, ICartRepository cartRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDto, string email, CancellationToken ct = default)
        {
            var cart = await _cartRepository.GetCartAsync(orderDto.CartId, ct);
            if(cart is null) 
                return Result<OrderToReturnDTO>.Fail(Error.NotFound("Cart.NoFound", $"Cart With Id {orderDto.CartId} Is Not Found"));

            if(cart.CartItems.Count == 0)
                return Result<OrderToReturnDTO>.Fail(Error.Validation("Cart.Empty", $"Can't Create Order For Empty Cart :{orderDto.CartId} "));

            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var productIds = cart.CartItems.Select(c => c.Id).ToHashSet();
            var specs = new ProductWithIdsSpecification(productIds);
            var products = (await productRepo.GetAllAsync(specs, ct)).ToDictionary(p => p.Id);

            var orderItems = new List<OrderItem>(cart.CartItems.Count);
            
             foreach(var item in cart.CartItems)
            {
                if(!products.TryGetValue(item.Id, out var product))
                    return Result<OrderToReturnDTO>.Fail(Error.NotFound("Product.NotFound", $"Product: {item.Id} - Is Not Found"));

                orderItems.Add(new OrderItem()
                {
                    Price = product.Price * (1 - product.DiscountPercentage / 100),
                    Quantity = item.Quantity,
                    Product = new ProductItemOrdered()
                    {
                        ProductName = product.Name,
                        ProductId = product.Id,
                        PictureUrl = product.ThumbnailUrl
                    }
                });

            }

             var orderAddress = _mapper.Map<OrderAddress>(orderDto.ShipToAddress);

            var deliveryMethodRepo = _unitOfWork.GetRepository<DeliveryMethod, int>();
            var deliveryMethod =  await deliveryMethodRepo.GetByIdAsync(orderDto.DeliveryMethodId, ct);

            if(deliveryMethod is null)
                return Result<OrderToReturnDTO>.Fail(Error.NotFound("DeliveryMethod.NotFound", $"DeliveryMethod: {orderDto.DeliveryMethodId} - Is Not Found"));

            var subTotal = orderItems.Sum(oi => (oi.Price * oi.Quantity));


            var order = new Order(email, orderItems, orderAddress, deliveryMethod, subTotal, cart.PaymentIntentId!);

            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

            orderRepo.Add(order);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result < 1)
                return Result<OrderToReturnDTO>.Fail(Error.Failure("Order.Failure", "Failed To Create New Order"));
            else
                await _cartRepository.DeleteCartAsync(orderDto.CartId);
                return Result<OrderToReturnDTO>.Ok(_mapper.Map<OrderToReturnDTO>(order));

        }


        async Task<Result<IReadOnlyList<DeliveryMethodDTO>>> IOrderService.GetAllDeliveryMethodsAsync(CancellationToken ct)
        {
            var deliveryMethods= await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);
            return Result<IReadOnlyList<DeliveryMethodDTO>>.Ok(_mapper.Map<IReadOnlyList<DeliveryMethodDTO>>(deliveryMethods));
        }

        async Task<Result<IReadOnlyList<OrderToReturnDTO>>> IOrderService.GetAllOrdersAsync(string email, CancellationToken ct)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecifications(email), ct);
            return Result<IReadOnlyList<OrderToReturnDTO>>.Ok(_mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        async Task<Result<OrderToReturnDTO>> IOrderService.GetOrderByIdAndEMailAsync(Guid orderId, string email, CancellationToken ct)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecifications(orderId,email), ct);
            if(order is null)
                return Result<OrderToReturnDTO>.Fail(Error.NotFound("Order.NotFound", $"Order: {orderId} - Is Not Found"));
            else
                return Result<OrderToReturnDTO>.Ok(_mapper.Map<OrderToReturnDTO>(order));

        }

    }
}
