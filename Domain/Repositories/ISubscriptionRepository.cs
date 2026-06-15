using Domain.Models.Contract;
using Domain.Models.Customer;
using Domain.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<ProductSubscription?> GetByIdAsync(int id);
        Task<ProductSubscription?> GetByIdWithPaymentAsync(int id);
        Task<int> AddAsync(ProductSubscription subscription);
        Task SaveChangesAsync();
        Task<List<ProductSubscription>> GetAllWithPaymentsAndCustomer();
        Task<List<ProductSubscription>> GetAllWithPaymentsAndCustomerByProdcutId(int productId);
    }
}
