using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Transactions;

namespace E_Commerce.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, ICollection<OrderItem> items, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            Items = items;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            DeliverMethodId = deliveryMethod.Id;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; private set; } = default!;
        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.UtcNow;
        public ICollection<OrderItem> Items { get; private set; } = default!;
        public OrderAddress ShipToAddress { get; private set; } = default!;
        public DeliveryMethod DeliveryMethod { get; private set; } = default!;
        public int DeliverMethodId { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.pending;
        public decimal SubTotal { get; private set; }
        public string PaymentIntentId { get; private set; } = default!;

        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

        public void MarkPaymentRecieved() => Status = OrderStatus.paymentRecieved;
        public void MarkPaymentFailed() => Status = OrderStatus.paymentFailed;
    }
}
