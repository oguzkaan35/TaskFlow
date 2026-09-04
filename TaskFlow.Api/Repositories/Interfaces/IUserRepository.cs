using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByUsernameAsync(string username);

    Task<bool> ExistsByUsernameAsync(string username);

    Task AddAsync(User user);

    void Update(User user);

    void Delete(User user);

    Task<bool> SaveChangesAsync();
}
