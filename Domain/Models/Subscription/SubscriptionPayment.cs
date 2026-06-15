using Domain.Exceptions;
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
            ProductSubscription subscription,
            decimal amountPln)
        {
            if (amountPln <= 0)
                throw new BadRequestException("Payment amount must be positive.");

            var now = DateTime.UtcNow;
            if (now < subscription.SubscriptionWindow.Start || now > subscription.SubscriptionWindow.End)
                throw new BadRequestException("Currnet time must be within start and end dates");
            
            Customer = customer;
            Subscription = subscription;
            AmountPln = amountPln;
            PeriodStart = subscription.SubscriptionWindow.Start;
            PeriodEnd = subscription.SubscriptionWindow.End;
            PaymentDate = DateTime.UtcNow;

            subscription.Payments.Add(this);
        }

        public SubscriptionPayment(
           Customer.Customer customer,
           ProductSubscription subscription,
           decimal amountPln,
           DateTime dt) : this(customer, subscription, amountPln)
        {
            PaymentDate = dt;
        }
    }
}
