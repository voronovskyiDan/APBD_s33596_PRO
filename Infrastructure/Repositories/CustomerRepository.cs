using Domain.Models.Customer;
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
    public class CustomerRepository : ICustomerRespository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer.Id;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.Where(c  => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetByIdWithContractsAndSubscriptionsAsync(int id)
        {
            return await _context.Customers
                .Where(c => c.Id == id)
                .Include(c=>c.Contracts)
                .Include(c=>c.Subscriptions)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
