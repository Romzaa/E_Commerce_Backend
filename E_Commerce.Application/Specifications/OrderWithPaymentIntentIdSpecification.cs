using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class OrderWithPaymentIntentIdSpecification : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentIdSpecification(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
