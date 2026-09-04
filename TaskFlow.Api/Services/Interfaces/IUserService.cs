using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task<User> CreateAsync(User user);

    Task<bool> UpdateAsync(int id, User user);

    Task<bool> DeleteAsync(int id);
}