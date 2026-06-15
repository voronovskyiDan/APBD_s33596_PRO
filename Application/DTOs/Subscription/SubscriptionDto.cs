using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Subscription
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal RenewalPeriodMonths { get; set; }
        public decimal PricePerPeriod { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
