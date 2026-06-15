using Domain.Common;
using Domain.Exceptions;
using Domain.Models.Discount;
using Domain.Models.Product;

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
            SubscriptionWindow subscriptionWindow)
        {
            Name = name;
            RenewalPeriodMonths = renewalPeriodMonths;
            PricePerPeriod = pricePerPeriod;
            Customer = customer;
            SoftwareProduct = softwareProduct;
            Status = status;
            SubscriptionWindow = subscriptionWindow;
        }

        public static ProductSubscription Register(
                Customer.Customer customer,
                SoftwareProduct product,
                string name,
                int renewalPeriodMonths,
                decimal pricePerPerid
            )
        {
            if (renewalPeriodMonths < MinRenewalPeriodMonths || renewalPeriodMonths > MaxRenewalPeriodMonths)
                throw new BadRequestException("Renewal Period must be within 3 month and 2 years");
            if (customer.IsDeleted)
                throw new ConflictException("Cannot register a subscription for a deleted customer.");

            customer.EnsureCanAcquireProduct(product);
            var promotionalDiscount = product.GetHighestActiveDiscount(DiscountType.SubscriptionFirstPeriod);

            var expectedFirstPayment = CalculateFirstPeriodPrice(
               pricePerPerid,
               promotionalDiscount,
               customer.IsReturningCustomer());

            var now = DateTime.UtcNow;

            var subscriptionWindow = SubscriptionWindow.Create(now, renewalPeriodMonths);


            var subscription = new ProductSubscription(
                name,
                renewalPeriodMonths,
                pricePerPerid,
                customer,
                product,
                SubscriptionStatus.Active,
                subscriptionWindow);

            SubscriptionPayment firstPayment = new SubscriptionPayment(
                customer,
                subscription,
                expectedFirstPayment,
                now);

            return subscription;
        }

        public void PayRenewal(SubscriptionPayment payment)
        {
             if (Status != SubscriptionStatus.Active)
                throw new ConflictException("Cannot pay for a cancelled subscription.");

            if (payment.Customer.Id != CustomerId)
                throw new ConflictException("Payment customer does not match contract customer.");

            if (SubscriptionWindow.IsExpired())
            {
                Status = SubscriptionStatus.Cancelled;
                throw new ConflictException("Subscription cancelled due to unpaid previous periods.");
            }

            if (IsPaidForCurrentPeriod())
                throw new ConflictException("Current renewal period is already paid.");

            var expectedAmount = CalculateRenewalPrice(PricePerPeriod, Customer.IsReturningCustomer());
            if (payment.AmountPln != expectedAmount)
                throw new ConflictException($"Renewal payment must equal {expectedAmount} PLN.");

            Payments.Add(payment);

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
