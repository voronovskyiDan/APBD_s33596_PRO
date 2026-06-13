using Domain.Models.Product;
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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<SoftwareProduct?> GetByIdWithDiscountsAsync(int id)
        {
            return await _context.SoftwareProducts.Where(p => p.Id == id).Include(p => p.Discounts).FirstOrDefaultAsync();
        }
    }
}
