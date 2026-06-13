using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Subscription
{
    public class SubscriptionPayment
    {
        public int Id { get; private set; }

        public int CustomerId { get; private set; }
        public Customer.Customer Customer { get; private set; } = null!;

        public int SubscriptionId { get; private set; }
        public ProductSubscription Subscription { get; private set; } = null!;

        public decimal AmountPln { get; private set; }

        public DateTime PaymentDate { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }

        private SubscriptionPayment() { }

        public SubscriptionPayment(
            Customer.Customer customer,
            decimal amountPln,
            DateTime periodStart,
            DateTime periodEnd)
        {
            if (amountPln <= 0)
                throw new ArgumentException("Payment amount must be positive.", nameof(amountPln));

            if (periodStart > periodEnd)
                throw new ArgumentException("Starting date must be before ending date");

            var now = DateTime.UtcNow;
            if (now < periodStart || now > periodEnd)
                throw new ArgumentException("Currnet time must be within start and end dates");


            Customer = customer;
            AmountPln = amountPln;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            PaymentDate = DateTime.UtcNow;
        }

        public SubscriptionPayment(
           Customer.Customer customer,
           decimal amountPln,
           DateTime periodStart,
           DateTime periodEnd,
           DateTime dt) : this(customer, amountPln, periodStart, periodEnd)
        {
            if (dt < periodStart || dt > periodEnd)
                throw new ArgumentException("Currnet time must be within start and end dates");

            PaymentDate = dt;
        }
    }
}
