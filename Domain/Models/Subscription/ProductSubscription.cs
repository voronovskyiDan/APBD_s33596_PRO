using Domain.Common;
using Domain.Models.Contract;
using Domain.Models.Customer;
using Domain.Models.Discount;
using Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Subscription
{
    public class ProductSubscription
    {
        private const int MinRenewalPeriodMonths = 1;
        private const int MaxRenewalPeriodMonths = 24;

        public int Id { get; private set; }

        public string Name { get; private set; } = null!;
        public int RenewalPeriodMonths { get; private set; }
        public decimal PricePerPeriod { get; private set; }

        public int CustomerId { get; private set; }
        public Customer.Customer Customer { get; private set; } = null!;

        public int SoftwareProductId { get; private set; }
        public SoftwareProduct SoftwareProduct { get; private set; } = null!;

        public SubscriptionStatus Status { get; private set; }

        public List<SubscriptionPayment> Payments { get; private set; } = [];

        public SubscriptionWindow SubscriptionWindow { get; private set; } = null!;

        private ProductSubscription() { }
        private ProductSubscription(
            string name,
            int renewalPeriodMonths,
            decimal pricePerPeriod, 
            Customer.Customer customer,
            SoftwareProduct softwareProduct,
            SubscriptionStatus status,
            List<SubscriptionPayment> payments,
            SubscriptionWindow subscriptionWindow)
        {
            Name = name;
            RenewalPeriodMonths = renewalPeriodMonths;
            PricePerPeriod = pricePerPeriod;
            Customer = customer;
            SoftwareProduct = softwareProduct;
            Status = status;
            Payments = payments;
            SubscriptionWindow = subscriptionWindow;
        }

        public static ProductSubscription Register(
                Customer.Customer customer,
                SoftwareProduct product,
                string name,
                int renewalPeriodMonths,
                int pricePerPerid
            )
        {
            if (renewalPeriodMonths < MinRenewalPeriodMonths || renewalPeriodMonths > MaxRenewalPeriodMonths)
                throw new ArgumentOutOfRangeException("Renewal Period must be within 3 month and 2 years");
            if (customer.IsDeleted)
                throw new InvalidOperationException("Cannot register a subscription for a deleted customer.");

            customer.EnsureCanAcquireProduct(product);
            var promotionalDiscount = product.GetHighestActiveDiscount(DiscountType.SubscriptionFirstPeriod);

            var expectedFirstPayment = CalculateFirstPeriodPrice(
               pricePerPerid,
               promotionalDiscount,
               customer.IsReturningCustomer());

            var now = DateTime.UtcNow;

            var subscriptionWindow = SubscriptionWindow.Create(now, renewalPeriodMonths);


            SubscriptionPayment firstPayment = new SubscriptionPayment(
                customer,
                expectedFirstPayment,
                now,
                subscriptionWindow.Start,
                subscriptionWindow.End);

            var subscription = new ProductSubscription(
                name,
                renewalPeriodMonths,
                pricePerPerid,
                customer,
                product,
                SubscriptionStatus.Active,
                new List<SubscriptionPayment> { firstPayment },
                subscriptionWindow);

            return subscription;
        }

        public void PayRenewal(decimal amountPln)
        {
            if (Status != SubscriptionStatus.Active)
                throw new InvalidOperationException("Cannot pay for a cancelled subscription.");

            if (SubscriptionWindow.IsExpired())
            {
                Status = SubscriptionStatus.Cancelled;
                throw new InvalidOperationException("Subscription cancelled due to unpaid previous periods.");
            }

            if (IsPaidForCurrentPeriod())
                throw new InvalidOperationException("Current renewal period is already paid.");

            var expectedAmount = CalculateRenewalPrice(PricePerPeriod, Customer.IsReturningCustomer());
            if (amountPln != expectedAmount)
                throw new InvalidOperationException($"Renewal payment must equal {expectedAmount} PLN.");

            Payments.Add(new SubscriptionPayment(
                Customer,
                amountPln,
                SubscriptionWindow.Start,
                SubscriptionWindow.End));

            SubscriptionWindow.Advance(RenewalPeriodMonths);

        }

        public void Cancel() => Status = SubscriptionStatus.Cancelled;

        private bool IsPaidForCurrentPeriod()
        {
            return Payments.Any(p =>
                p.PeriodStart == SubscriptionWindow.Start &&
                p.PeriodEnd == SubscriptionWindow.End);
        }

        private static decimal CalculateFirstPeriodPrice(
           decimal basePrice,
           Discount.Discount? promotionalDiscount,
           bool isReturningCustomer) =>
           PriceCalculator.ApplyDiscounts(basePrice, promotionalDiscount, isReturningCustomer);
        public static decimal CalculateRenewalPrice(decimal basePrice, bool isReturningCustomer)
        {
            if (isReturningCustomer)
                return PriceCalculator.ApplyReturningCustomerDiscountOnly(basePrice);
            return basePrice;

        }

    }
}
