using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common
{
    public class AcceptPaymentDto
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
    }
}
