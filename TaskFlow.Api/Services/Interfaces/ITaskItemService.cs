using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Services.Interfaces;

public interface ITaskItemService
{
    Task<List<TaskItem>> GetAllAsync();

    Task<TaskItem?> GetByIdAsync(int id);

    Task<TaskItem> CreateAsync(TaskItem taskItem);

    Task<bool> UpdateAsync(int id, TaskItem taskItem);

    Task<List<TaskItem>> GetMyTasksAsync(int userId);

    Task<bool> UpdateMyTaskStatusAsync(
    int taskId,
    int userId,
    int statusId);

    Task<bool> DeleteAsync(int id);
}
