using TaskFlow.Api.DTOs.Auth;

namespace TaskFlow.Api.Services.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto dto);
}
