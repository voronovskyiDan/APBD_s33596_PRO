using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Subscription.Add
{
    public class AddSubscriptionDto
    {
        [Required]
        public int? CustomerId { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [MinLength(3)]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 24)]
        public int RenevalPeriodMonth { get; set; }

        [Range(1, double.MaxValue)]
        public decimal PricePerPeriod { get; set; }
    }
}
