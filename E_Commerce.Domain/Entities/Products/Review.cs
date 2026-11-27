using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Products
{
    public class Review : BaseEntity<int>   
    {
        public int Rating { get; set; }

        public string ReviewerName { get; set; } = default!;

        public string ReviewerEmail { get; set; } = default!;

        public string Comment { get; set; } = default!;

        public DateTime Date { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = default!;
    }
}
