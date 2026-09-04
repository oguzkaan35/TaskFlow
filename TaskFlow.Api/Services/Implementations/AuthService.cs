using TaskFlow.Api.DTOs.Auth;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user =
            await _userRepository.GetByUsernameAsync(dto.Username);

        if (user == null)
        {
            return null;
        }

        bool passwordCorrect =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash
            );

        if (!passwordCorrect)
        {
            return null;
        }

        return _tokenService.CreateToken(user);
    }
}