using Domain.Models.Contract;
using Domain.Models.Subscription;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(ProductSubscription subscription)
        {
            _context.ProductSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription.Id;
        }

        public async Task<List<ProductSubscription>> GetAllWithPaymentsAndCustomer()
        {
            return await _context.ProductSubscriptions
                .Include(x => x.Payments)
                .Include(x => x.Customer)
                .ToListAsync();
        }

        public async Task<List<ProductSubscription>> GetAllWithPaymentsAndCustomerByProdcutId(int productId)
        {
            return await _context.ProductSubscriptions
                 .Include(x => x.Payments)
                 .Include(x => x.Customer)
                 .Where(p => p.SoftwareProductId == productId)
                 .ToListAsync();
        }

        public async Task<ProductSubscription?> GetByIdAsync(int id)
        {
            return await _context.ProductSubscriptions.Where(pc => pc.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProductSubscription?> GetByIdWithPaymentAsync(int id)
        {
            return await _context.ProductSubscriptions
                .Where(pc => pc.Id == id)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
