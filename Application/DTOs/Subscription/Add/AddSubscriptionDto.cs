using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Subscription.Add
{
    public class AddSubscriptionDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RenevalPeriodMonth { get; set; }
        public decimal PricePerPeriod { get; set; }
    }
}
