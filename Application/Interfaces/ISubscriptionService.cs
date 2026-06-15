using Application.DTOs.Common;
using Application.DTOs.Contract;
using Application.DTOs.Contract.Add;
using Application.DTOs.Subscription;
using Application.DTOs.Subscription.Add;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubscriptionService
    {
        public Task<SubscriptionDto> CreateSubscription(AddSubscriptionDto addSubscriptionDto);
        public Task AcceptPayment(int subscriptiontId, AcceptPaymentDto acceptPayment);
        public Task<SubscriptionDto?> GetById(int id);
    }
}
