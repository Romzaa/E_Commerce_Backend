using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        pending = 0,
        paymentRecieved = 1,
        paymentFailed = 2
    }
}
