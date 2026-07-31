using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Commerce.Application.DTOs.Order
{
    public class OrderDTO
    {
        [Required]
        public string CartId { get; set; } = default!;
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDTO ShipToAddress { get; set; } = default!;
    }
}
