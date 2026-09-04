using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();

    Task<Project?> GetByIdAsync(int id);

    Task AddAsync(Project project);

    void Update(Project project);

    void Delete(Project project);

    Task<bool> SaveChangesAsync();

    Task<bool> ExistsByNameAsync(string projectName);

}
