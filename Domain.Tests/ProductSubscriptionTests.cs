using Domain.Exceptions;
using Domain.Models.Contract;
using Domain.Models.Customer;
using Domain.Models.Product;
using Domain.Models.Revenue;
using Domain.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class ProductSubscriptionTests
    {
        [Fact]
        public void Register_CreatesActiveSubscriptionWithFirstPayment()
        {
            var customer = new IndividualCustomer(
                "John", "Snow", "654356534745",
                "Warsaw", "john@example.com", "+56354634564");
            var product = new SoftwareProduct(
                1, "FinancePro", "Accounting software", "1.0.0",
                SoftwareCategory.Finance, 10_000m);

            var subscription = ProductSubscription.Register(
                customer, product, "Basic Plan", renewalPeriodMonths: 12, pricePerPerid: 500m);

            Assert.Equal(SubscriptionStatus.Active, subscription.Status);
            Assert.Single(subscription.Payments);
            Assert.Equal(500m, subscription.Payments[0].AmountPln);
        }

        [Fact]
        public void PayRenewal_WhenPaidNotExpectedAmount_ThrowsConflict()
        {
            var customer = new IndividualCustomer(
                "John", "Snow", "654356534745",
                "Warsaw", "john@example.com", "+56354634564");
            var product = new SoftwareProduct(
                 1, "FinancePro", "Accounting software", "1.0.0",
                 SoftwareCategory.Finance, 10_000m);
            var contract = ProductSubscription.Register(
                customer,
                product,
                "name",
                1,
                500);

            Assert.Throws<ConflictException>(() =>
            {
                contract.PayRenewal(new SubscriptionPayment(customer, contract, 499));
            });

        }

        [Theory]
        [InlineData(500, false, 500)]
        [InlineData(500, true, 475)]
        public void CalculateRenewalPrice_VariousInputs_ReturnsCorrectValue(
        decimal basePrice,
        bool isReturningCustomer,
        decimal expected)
        {
            var result = ProductSubscription.CalculateRenewalPrice(basePrice, isReturningCustomer);

            Assert.Equal(expected, result);
        }
    }
}
