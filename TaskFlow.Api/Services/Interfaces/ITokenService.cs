using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
