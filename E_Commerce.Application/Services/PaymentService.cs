using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly PaymentGatewaySettings _settings;
        private readonly IMapper _mapper;

        public PaymentService(ICartRepository cartRepository,
                              IUnitOfWork unitOfWork,
                              IPaymentGateway paymentGateway,
                              IOptions<PaymentGatewaySettings> options,
                              IMapper mapper
                               )
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _settings = options.Value;
            _mapper = mapper;
        }
        public async Task<Result<CartDTO>> CreateOrUpdatePaymentIntentAsync(string cartId, CancellationToken ct = default)
        {
            var cart = await _cartRepository.GetCartAsync(cartId, ct);
            if (cart == null)
                return Result<CartDTO>.Fail(new Error("Cart not found", $"Cart :{cartId} Is Not Found"));

            if(cart.CartItems.Count == 0)
                return Result<CartDTO>.Fail(new Error("Cart is empty", $"Cart :{cartId} Is Empty"));

            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var productIds = cart.CartItems.Select(ci => ci.Id).ToHashSet();
            var sepcs = new ProductWithIdsSpecification(productIds);
            var products = (await productRepo.GetAllAsync(sepcs, ct)).ToDictionary(p => p.Id);

            var deliveryRepo = _unitOfWork.GetRepository<DeliveryMethod, int>();
            var deliveryMethod = await deliveryRepo.GetByIdAsync(cart.DeliveryMethodId, ct);
            if(deliveryMethod is null)
                return Result<CartDTO>.Fail(new Error("DeliveryMethod.NotFound", $"DeliveryMethod :{cart.DeliveryMethodId} Is Not Found"));

            foreach (var item in cart.CartItems)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<CartDTO>.Fail(new Error("Product.NotFound", $"Product :{item.Id} Is Not Found"));

                item.Price = product.price;
            }

            cart.ShippingPrice = deliveryMethod.Cost;
            var subTotal = cart.CartItems.Sum(i => (i.Price * i.Quantity));
            var amount = subTotal + deliveryMethod.Cost;

            if (!string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                await _paymentGateway.UpdatePaymentIntent(cart.PaymentIntentId, amount, ct);
            }
            else
            {
                var paymentIntent = await _paymentGateway.CreatePaymentIntent(amount,_settings.DefaultCurrency , ct);
                cart.PaymentIntentId = paymentIntent.PaymentIntentId;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }

            await _cartRepository.CreateOrUpdateCartAsynce(cart,ct:ct);
            return Result<CartDTO>.Ok(_mapper.Map<CartDTO>(cart));

        }

        public async Task PaymentFailed(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var spec = new OrderWithPaymentIntentIdSpecification(paymentIntentId);
            var order = await orderRepo.GetByIdAsync(spec);
            if (order is null) return;

            order.MarkPaymentFailed();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task PaymentSucceeded(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var spec = new OrderWithPaymentIntentIdSpecification(paymentIntentId);
            var order = await orderRepo.GetByIdAsync(spec);
            if (order is null) return;

            order.MarkPaymentRecieved();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
