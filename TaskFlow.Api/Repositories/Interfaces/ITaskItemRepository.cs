using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Repositories.Interfaces;

public interface ITaskItemRepository
{
    Task<List<TaskItem>> GetAllAsync();

    Task<TaskItem?> GetByIdAsync(int id);

    Task AddAsync(TaskItem taskItem);

    void Update(TaskItem taskItem);

    void Delete(TaskItem taskItem);

    Task<bool> UserExistsAsync(int userId);

    Task<bool> ProjectExistsAsync(int projectId);

    Task<bool> StatusExistsAsync(int statusId);

    Task<List<TaskItem>> GetByAssignedUserIdAsync(int userId);

    

    Task<bool> SaveChangesAsync();
}
