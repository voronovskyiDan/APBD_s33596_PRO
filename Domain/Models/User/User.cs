using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public Role Role { get; private set; }

        private User() { }
        public User(string email, string passwordHash, string role) 
        {
            Email = email;
            PasswordHash = passwordHash;
            Role = Role.Employee;
        }
    }
}
