using Domain.Models.Contract;
using Domain.Models.Customer;
using Domain.Models.Discount;
using Domain.Models.Product;
using Domain.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class ProductContractTests
    {
        [Fact]
        public void Create_WithSupportAndDiscount_CalculatesTotalPrice()
        {
            var customer = new IndividualCustomer(
              "John", "Snow", "654356534745",
              "Warsaw", "john@example.com", "+56354634564");
            var product = new SoftwareProduct(
                1, "FinancePro", "Accounting software", "1.0.0",
                SoftwareCategory.Finance, 10_000m);
            product.Discounts.Add(new Discount(
                "Spring Sale",
                15m,
                new DateRange(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(30)),
                DiscountType.General));
            var paymentWindow = new PaymentWindow(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddDays(9));


            var contract = ProductContract.Create(customer, product, paymentWindow, additionalSupportYears: 2);

            Assert.Equal(10_200m, contract.TotalPrice);
            Assert.Equal(ContractStatus.PendingPayment, contract.Status);
        }

        [Fact]
        public void RegisterPayment_WhenFullyPaid_SignsContract()
        {
            var customer = new IndividualCustomer(
                "John", "Snow", "654356534745",
                "Warsaw", "john@example.com", "+56354634564");
            var product = new SoftwareProduct(
                 1, "FinancePro", "Accounting software", "1.0.0",
                 SoftwareCategory.Finance, 10_000m);
            var contract = ProductContract.Create(
                customer,
                product,
                new PaymentWindow(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(9)),
                additionalSupportYears: 0);

            contract.RegisterPayment(new ContractPayment(customer, contract, contract.TotalPrice));

            Assert.True(contract.IsSigned);
            Assert.True(contract.IsFullyPaid);
        }  
    }
}
