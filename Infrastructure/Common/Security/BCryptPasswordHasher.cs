using Application.Interfaces;

namespace Infrastructure.Common.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private readonly string _pepper;

        public BCryptPasswordHasher(string pepper)
        {
            _pepper = pepper;
        }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + _pepper, 12);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password + _pepper, hash);
        }
    }
}
