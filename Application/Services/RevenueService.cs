using Application.DTOs.Revenue;
using Application.Interfaces;
using Domain.Models.Contract;
using Domain.Models.Revenue;
using Domain.Models.Subscription;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RevenueService : IRevenueService
    {
        private readonly IContractRepository _contractRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly RevenueCalculator _revenueCalculator;
        public RevenueService(IContractRepository contractRepository, ISubscriptionRepository subscriptionRepository)
        {
            _contractRepository = contractRepository;
            _subscriptionRepository = subscriptionRepository;
            _revenueCalculator = new RevenueCalculator();   
        }

        public async Task<RevenueDto> GetCurrentRevenueAsync(int? softwareProductId = null)
        {
           var contracts = softwareProductId.HasValue
               ? await _contractRepository.GetAllWithPaymentsByProdcutId(softwareProductId.Value)
               : await _contractRepository.GetAllWithPayments();

            var subscriptions = softwareProductId.HasValue
                ? await _subscriptionRepository.GetAllWithPaymentsAndCustomerByProdcutId(softwareProductId.Value)
                : await _subscriptionRepository.GetAllWithPaymentsAndCustomer();

            decimal revenue = _revenueCalculator.CalculateCurrentRevenuePln(contracts, subscriptions);

            return new RevenueDto { Revenue = revenue };
        }

        public async Task<RevenueDto> GetPredictedRevenueAsync(int? softwareProductId = null)
        {
            var contracts = softwareProductId.HasValue
              ? await _contractRepository.GetAllWithPaymentsByProdcutId(softwareProductId.Value)
              : await _contractRepository.GetAllWithPayments();

            var subscriptions = softwareProductId.HasValue
                ? await _subscriptionRepository.GetAllWithPaymentsAndCustomerByProdcutId(softwareProductId.Value)
                : await _subscriptionRepository.GetAllWithPaymentsAndCustomer();

            decimal revenue = _revenueCalculator.CalculatePredictedRevenuePln(contracts, subscriptions);

            return new RevenueDto { Revenue = revenue };
        }
    }
}
