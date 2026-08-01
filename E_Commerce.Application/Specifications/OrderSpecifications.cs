using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class OrderSpecifications : BaseSpecification<Order,Guid>
    {
        public OrderSpecifications(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByAsc(o => o.OrderDate);

        }

        public OrderSpecifications(Guid id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
