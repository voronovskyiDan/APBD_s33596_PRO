using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Contract
{
    public class PaymentWindow
    {
        public const int MinPaymentWindowDays = 3;
        public const int MaxPaymentWindowDays = 30;

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
         
        public PaymentWindow(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("End date must be greater than or equal to start date.");

            var days = (endDate.Date - startDate.Date).Days + 1;
            if (days < MinPaymentWindowDays || days > MaxPaymentWindowDays)
                throw new ArgumentException(
                    $"Contract payment window must be between {MinPaymentWindowDays} and {MaxPaymentWindowDays} days.");

            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsExpired() =>
            DateTime.UtcNow > EndDate;
        public bool IsExpired(DateTime dt) =>
            dt > EndDate;
    }
}
