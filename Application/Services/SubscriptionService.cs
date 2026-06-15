using Application.DTOs.Common;
using Application.DTOs.Subscription;
using Application.DTOs.Subscription.Add;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Models.Subscription;
using Domain.Repositories;

namespace Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICustomerRespository _customerRespository;
        private readonly IProductRepository _productRepository;
        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            ICustomerRespository customerRespository,
            IProductRepository productRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _customerRespository = customerRespository;
            _productRepository = productRepository;
        }

        public async Task AcceptPayment(int subscriptionId, AcceptPaymentDto acceptPayment)
        {
            var subscription = await _subscriptionRepository.GetByIdWithPaymentAsync(subscriptionId);
            if (subscription == null)
                throw new NotFoundException("No subscription with such id");

            var customer = await _customerRespository.GetByIdAsync(acceptPayment.CustomerId);
            if (customer == null)
                throw new NotFoundException("No customer with such id");

            SubscriptionPayment subscriptionPayment = new SubscriptionPayment(customer, subscription, acceptPayment.Amount);
            try
            {
                subscription.PayRenewal(subscriptionPayment);
            }
            finally
            {
                await _subscriptionRepository.SaveChangesAsync();
            }
        }

        public async Task<SubscriptionDto> CreateSubscription(AddSubscriptionDto addSubscriptionDto)
        {
            var customer = await _customerRespository.GetByIdAsync(addSubscriptionDto.CustomerId);
            if (customer == null)
                throw new NotFoundException("No customer with such id");

            var product = await _productRepository.GetByIdWithDiscountsAsync(addSubscriptionDto.ProductId);
            if (product == null)
                throw new NotFoundException("No product with such id");

            var subscription = ProductSubscription.Register(
                            customer,
                            product,
                            addSubscriptionDto.Name,
                            addSubscriptionDto.RenevalPeriodMonth,
                            addSubscriptionDto.PricePerPeriod
                            );

            int id = await _subscriptionRepository.AddAsync(subscription);
                                        
            return new SubscriptionDto
            {
                Id = id,
                CustomerId = subscription.CustomerId,
                ProductId = subscription.SoftwareProductId,
                Name = subscription.Name,
                RenewalPeriodMonths = subscription.RenewalPeriodMonths,
                PricePerPeriod = subscription.PricePerPeriod,
                Status = subscription.Status.ToString()
            };
        }

        public async Task<SubscriptionDto?> GetById(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null)
                return null;
            return new SubscriptionDto
            {
                Id = subscription.Id,
                CustomerId = subscription.CustomerId,
                ProductId = subscription.SoftwareProductId,
                Name = subscription.Name,
                RenewalPeriodMonths = subscription.RenewalPeriodMonths,
                PricePerPeriod = subscription.PricePerPeriod,
                Status = subscription.Status.ToString()
            };
        }
    }
}
