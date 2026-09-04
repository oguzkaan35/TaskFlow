using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Services.Interfaces;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync();

    Task<Project?> GetByIdAsync(int id);

    Task<Project> CreateAsync(Project project);

    Task<bool> UpdateAsync(int id, Project project);

    Task<bool> DeleteAsync(int id);

   

}