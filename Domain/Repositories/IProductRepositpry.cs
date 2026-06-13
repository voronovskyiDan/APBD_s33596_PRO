using Domain.Models.Contract;
using Domain.Models.Customer;
using Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<SoftwareProduct?> GetByIdWithDiscountsAsync(int id);
    }
}
