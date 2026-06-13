using Domain.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICustomerRespository
    {
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetByIdWithContractsAndSubscriptionsAsync(int id);
        Task<int> AddAsync(Customer customer);
        Task SaveChangesAsync();
    }
}
