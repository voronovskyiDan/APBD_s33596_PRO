using Domain.Models;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<bool> ExistsByEmaiAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
