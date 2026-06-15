using Domain.Models.Contract;
using Domain.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IContractRepository
    {
        Task<ProductContract?> GetByIdAsync(int id);
        Task<ProductContract?> GetByIdWithPaymentsAsync(int id);

        Task<int> AddAsync(ProductContract contract);
        Task SaveChangesAsync();
        Task<List<ProductContract>> GetAllWithPayments();
        Task<List<ProductContract>> GetAllWithPaymentsByProdcutId(int productId);

    }
}
