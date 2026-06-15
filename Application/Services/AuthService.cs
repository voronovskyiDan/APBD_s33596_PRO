using Application.DTOs.Auth;
using Application.Interfaces;
using BCrypt.Net;
using Domain.Models;
using Domain.Models.User;
using Domain.Repositories;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }
        public async Task<TokenDto?> LoginAsync(LoginDto model)
        {
            var email = model.Email.ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
                return null;

            if (!_passwordHasher.Verify(model.Password, user.PasswordHash))
                return null;

            var token = _jwtService.GenerateToken(user);

            return new TokenDto { Token = token };

        }

        public async Task<UserDto?> RegisterAsync(RegisterDto model)
        {
            var email = model.Email.ToLowerInvariant();

            if (await _userRepository.ExistsByEmaiAsync(email))
                return null;

            var user = new User(
                    email,
                    _passwordHasher.Hash(model.Password),
                    "User"
                );

            await _userRepository.AddAsync(user);

            return new UserDto {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
