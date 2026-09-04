using TaskFlow.Api.Entities;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User> CreateAsync(User user)
    {
        bool usernameExists =
            await _userRepository.ExistsByUsernameAsync(user.Username);

        if (usernameExists)
        {
            throw new InvalidOperationException(
                "Bu kullanıcı adı zaten kullanılıyor.");
        }

        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<bool> UpdateAsync(int id, User user)
    {
        var existingUser =
            await _userRepository.GetByIdAsync(id);

        if (existingUser == null)
            return false;

        existingUser.FullName = user.FullName;
        existingUser.Role = user.Role;

        _userRepository.Update(existingUser);

        return await _userRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingUser =
            await _userRepository.GetByIdAsync(id);

        if (existingUser == null)
            return false;

        _userRepository.Delete(existingUser);

        return await _userRepository.SaveChangesAsync();
    }
}
