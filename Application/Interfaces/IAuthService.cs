using Application.DTOs.Auth;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(RegisterDto model);
        Task<TokenDto?> LoginAsync(LoginDto model);
    }
}
