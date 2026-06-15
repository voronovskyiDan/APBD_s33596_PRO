using Domain.Models.Contract;
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
    public class ContractRepository : IContractRepository
    {
        private readonly AppDbContext _context;
        public ContractRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(ProductContract contract)
        {
            _context.ProductContracts.Add(contract);
            await _context.SaveChangesAsync();
            return contract.Id;
        }

        public async Task<List<ProductContract>> GetAllWithPayments()
        {
            return await _context.ProductContracts
                .Include(x => x.Payments)
                .ToListAsync();
        }

        public async Task<List<ProductContract>> GetAllWithPaymentsByProdcutId(int productId)
        {
            return await _context.ProductContracts
                .Include(x => x.Payments)
                .Where(p => p.SoftwareProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductContract?> GetByIdAsync(int id)
        {
            return await _context.ProductContracts.Where(pc => pc.Id == id).FirstOrDefaultAsync();
        }
        public async Task<ProductContract?> GetByIdWithPaymentsAsync(int id)
        {
            return await _context.ProductContracts
                .Where(pc => pc.Id == id)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
