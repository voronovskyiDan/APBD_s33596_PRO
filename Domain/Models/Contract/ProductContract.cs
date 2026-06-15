using Domain.Common;
using Domain.Exceptions;
using Domain.Models.Discount;
using Domain.Models.Product;
using Domain.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Contract
{
    public class ProductContract
    {
        private const int IncludedUpdateYears = 1;
        private const decimal AdditionalSupportYearCostPln = 1000m;

        public int Id { get; private set; }

        public int CustomerId { get; private set; }
        public Customer.Customer Customer { get; private set; } = null!;

        public int SoftwareProductId { get; private set; }
        public SoftwareProduct SoftwareProduct { get; private set; } = null!;

        public string SoftwareVersion { get; private set; } = string.Empty;

        public PaymentWindow PaymentWindow { get; private set; } = null!;

        public int AdditionalSupportYears { get; private set; }
        public int TotalUpdateYears => IncludedUpdateYears + AdditionalSupportYears;

        public decimal TotalPrice { get; private set; }
        public ContractStatus Status { get; private set; }

        public DateTime? SigningDate { get; set; }

        public List<ContractPayment> Payments { get; set; } = [];

        public bool IsSigned => Status == ContractStatus.Signed;
        public bool IsActive => (Status == ContractStatus.Signed && SigningDate!.Value.AddYears(AdditionalSupportYears) < DateTime.UtcNow ) || (Status == ContractStatus.PendingPayment && !PaymentWindow.IsExpired());
        public decimal TotalPaid => Payments.Where(p => !p.IsRefunded).Sum(p => p.AmountPln);
        public bool IsFullyPaid => TotalPaid == TotalPrice;

        private ProductContract() { }

        public void Delete()
        {
            if (Status == ContractStatus.Signed)
                throw new ConflictException("Signed contracts cannot be deleted.");

            Status = ContractStatus.Deleted;
        }

        public void RegisterPayment(ContractPayment payment)
        {
            if (Status != ContractStatus.PendingPayment)
                throw new ConflictException("Payments can only be registered for pending contracts.");

            if (payment.Customer.Id != CustomerId)
                throw new ConflictException("Payment customer does not match contract customer.");

            if(Status == ContractStatus.Expired)
                throw new ConflictException("Contract payment window has expired.");

            if (PaymentWindow.IsExpired())
            {
                Status = ContractStatus.Expired;
                throw new ConflictException("Contract payment window has expired.");
            }

            if (TotalPrice < TotalPaid + payment.AmountPln)
                throw new ConflictException("Contract payment amount is too big");
            
            Payments.Add(payment);

            if (IsFullyPaid)
                Status = ContractStatus.Signed;
        }

        public static ProductContract Create(
            Customer.Customer customer,
            SoftwareProduct product,
            PaymentWindow paymentWindow,
            int additionalSupportYears)
        {
            if (customer.IsDeleted)
                throw new ConflictException("Cannot create a contract for a deleted customer.");
            ValidateAdditionalSupportYears(additionalSupportYears);
            customer.EnsureCanAcquireProduct(product);
            
            var promotionalDiscount = product.GetHighestActiveDiscount();
            var totalPrice = CalculatePrice(
                product.AnnualLicensePrice,
                additionalSupportYears,
                promotionalDiscount,
                customer.IsReturningCustomer());

            var contract = new ProductContract
            {
                CustomerId = customer.Id,
                Customer = customer,
                SoftwareProductId = product.Id,
                SoftwareProduct = product,
                SoftwareVersion = product.Version,
                PaymentWindow = paymentWindow,
                AdditionalSupportYears = additionalSupportYears,
                TotalPrice = totalPrice,
                Status = ContractStatus.PendingPayment
            };

            return contract;
        }

        private static decimal CalculatePrice(
            decimal annualLicensePricePln,
            int additionalSupportYears,
            Discount.Discount? promotionalDiscount,
            bool isReturningCustomer)
        {
            ValidateAdditionalSupportYears(additionalSupportYears);

            var basePrice = annualLicensePricePln + (additionalSupportYears * AdditionalSupportYearCostPln);
            return PriceCalculator.ApplyDiscounts(basePrice, promotionalDiscount, isReturningCustomer);
        }

        private static void ValidateAdditionalSupportYears(int additionalSupportYears)
        {
            if (additionalSupportYears < 0 || additionalSupportYears > 3)
                throw new BadRequestException("Additional support years must be between 0 and 3.");
        }

        public void ExpireIfUnpaid(DateTime moment)
        {
            if (Status != ContractStatus.PendingPayment)
                return;

            if (!PaymentWindow.IsExpired())
                return;

            foreach (var payment in Payments.Where(p => !p.IsRefunded))
                payment.MarkRefunded();

            Status = ContractStatus.Expired;
        }
    }
}
