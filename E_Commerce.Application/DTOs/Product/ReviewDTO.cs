using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace E_Commerce.Application.DTOs.Product
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }

        public string ReviewerName { get; set; } = default!;

        public string ReviewerEmail { get; set; } = default!;

        public string Comment { get; set; } = default!;
        public DateTime Date { get; set; }

        public int ProductId { get; set; }
    }
}
