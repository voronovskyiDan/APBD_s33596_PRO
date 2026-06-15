using Domain.Models.Contract;
using Domain.Models.Customer;
using Domain.Models.Discount;
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
    public class RevenueCalculatorTests
    {
        [Fact]
        public void CalculateCurrentRevenue()
        {
            var customer = new IndividualCustomer(
                 "John", "Snow", "654356534745",
                 "Warsaw", "john@example.com", "+56354634564");
            var product = new SoftwareProduct(
                1, "FinancePro", "Accounting software", "1.0.0",
                SoftwareCategory.Finance, 10_000m);
            var saasProduct = new SoftwareProduct(
                2, "EduCloud", "Learning platform", "1.0.0",
                SoftwareCategory.Education, 6_000m);

            var contract = ProductContract.Create(
                customer,
                product,
                new PaymentWindow(DateTime.UtcNow, DateTime.UtcNow.AddDays(9)),
                additionalSupportYears: 1);
            contract.RegisterPayment(new ContractPayment(customer, contract, contract.TotalPrice));

            var subscription = ProductSubscription.Register(
                customer, saasProduct, "Basic Plan", renewalPeriodMonths: 12, pricePerPerid: 500m);

            var calculator = new RevenueCalculator();

            var result = calculator.CalculateCurrentRevenuePln(
                new List<ProductContract> { contract },
                new List<ProductSubscription> { subscription });

            Assert.Equal(11_500m, result);
        }

        [Fact]
        public void CalculateCurrentRevenue_WithDiscount()
        {
            var customer = new IndividualCustomer(
                 "John", "Snow", "654356534745",
                 "Warsaw", "john@example.com", "+56354634564");
            var product = new SoftwareProduct(
                1, "FinancePro", "Accounting software", "1.0.0",
                SoftwareCategory.Finance, 10_000m);
            product.Discounts.Add(new Discount("Black Friday", 10, new DateRange(DateTime.UtcNow, DateTime.UtcNow.AddDays(5)), DiscountType.General));

            var contract = ProductContract.Create(
                customer,
                product,
                new PaymentWindow(DateTime.UtcNow, DateTime.UtcNow.AddDays(9)),
                additionalSupportYears: 0);
            contract.RegisterPayment(new ContractPayment(customer, contract, contract.TotalPrice));

            var calculator = new RevenueCalculator();

            var result = calculator.CalculateCurrentRevenuePln(
                new List<ProductContract> { contract },
                []
            );
            Assert.Equal(9_000m, result);
        }

        [Fact]
        public void CalculatePredictedRevenue()
        {
            var customer = new IndividualCustomer(
                "John", "Snow", "654356534745",
                "Warsaw", "john@example.com", "+56354634564");
            var product = new SoftwareProduct(
                1, "FinancePro", "Accounting software", "1.0.0",
                SoftwareCategory.Finance, 10_000m);
            var saasProduct = new SoftwareProduct(
                2, "EduCloud", "Learning platform", "1.0.0",
                SoftwareCategory.Education, 6_000m);

            var contract = ProductContract.Create(
                customer,
                product,
                new PaymentWindow(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(9)),
                additionalSupportYears: 0);
            contract.RegisterPayment(new ContractPayment(customer, contract, contract.TotalPrice));

            var subscription = ProductSubscription.Register(
                customer, saasProduct, "Basic Plan", renewalPeriodMonths: 12, pricePerPerid: 500m);

            var calculator = new RevenueCalculator();

            var result = calculator.CalculatePredictedRevenuePln(
                new List<ProductContract> { contract },
                new List<ProductSubscription> { subscription });

            Assert.Equal(11_000m, result);
        }
    }
}
