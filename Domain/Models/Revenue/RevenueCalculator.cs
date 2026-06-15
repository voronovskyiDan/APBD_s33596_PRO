using Domain.Models.Contract;
using Domain.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Revenue
{
    public class RevenueCalculator
    {
        public decimal CalculateCurrentRevenuePln(
            List<ProductContract> contracts,
            List<ProductSubscription> subscriptions)
        {
            var contractRevenue = contracts
                .Where(c => c.Status == ContractStatus.Signed)
                .SelectMany(c => c.Payments)
                .Where(p => !p.IsRefunded)
                .Sum(p => p.AmountPln);

            var subscriptionRevenue = subscriptions
                .SelectMany(s => s.Payments)
                .Sum(p => p.AmountPln); 

            return Round(contractRevenue + subscriptionRevenue);
        }

        public decimal CalculatePredictedRevenuePln(
            List<ProductContract> contracts,
            List<ProductSubscription> subscriptions)
        {
            var current = CalculateCurrentRevenuePln(contracts, subscriptions);

            var unsignedContracts = contracts
                .Where(c => c.Status == ContractStatus.PendingPayment)
                .Where(c => !c.PaymentWindow.IsExpired(DateTime.UtcNow))
                .Sum(c => c.TotalPrice);

            var subscriptionRenewals = subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .Sum(s => ProductSubscription.CalculateRenewalPrice(
                    s.PricePerPeriod,
                    s.Customer.IsReturningCustomer()));

            return Round(current + unsignedContracts + subscriptionRenewals);
        }

        private static decimal Round(decimal value) =>
            Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}