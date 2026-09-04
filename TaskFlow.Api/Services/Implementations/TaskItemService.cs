using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Entities;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services.Implementations;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;

    public TaskItemService(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }



    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _taskItemRepository.GetAllAsync();
    }



    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _taskItemRepository.GetByIdAsync(id);
    }



    public async Task<TaskItem> CreateAsync(TaskItem taskItem)
    {
        bool userExists =
            await _taskItemRepository.UserExistsAsync(taskItem.AssignedUserId);

        if (!userExists)
        {
            throw new InvalidOperationException(
                "Görevin atanacağı kullanıcı bulunamadı.");
        }

        bool projectExists =
            await _taskItemRepository.ProjectExistsAsync(taskItem.ProjectId);

        if (!projectExists)
        {
            throw new InvalidOperationException(
                "Görevin ait olduğu proje bulunamadı.");
        }

        bool statusExists =
            await _taskItemRepository.StatusExistsAsync(taskItem.StatusId);

        if (!statusExists)
        {
            throw new InvalidOperationException(
                "Görev durumu bulunamadı.");
        }

        await _taskItemRepository.AddAsync(taskItem);

        await _taskItemRepository.SaveChangesAsync();

        return taskItem;
    }



    public async Task<bool> UpdateAsync(int id, TaskItem taskItem)
    {
        var existingTask =
            await _taskItemRepository.GetByIdAsync(id);

        if (existingTask == null)
            return false;

        bool userExists =
            await _taskItemRepository.UserExistsAsync(taskItem.AssignedUserId);

        if (!userExists)
        {
            throw new InvalidOperationException(
                "Görevin atanacağı kullanıcı bulunamadı.");
        }

        bool projectExists =
            await _taskItemRepository.ProjectExistsAsync(taskItem.ProjectId);

        if (!projectExists)
        {
            throw new InvalidOperationException(
                "Görevin ait olduğu proje bulunamadı.");
        }

        bool statusExists =
            await _taskItemRepository.StatusExistsAsync(taskItem.StatusId);

        if (!statusExists)
        {
            throw new InvalidOperationException(
                "Görev durumu bulunamadı.");
        }

        existingTask.Title = taskItem.Title;
        existingTask.Description = taskItem.Description;
        existingTask.AssignedUserId = taskItem.AssignedUserId;
        existingTask.ProjectId = taskItem.ProjectId;
        existingTask.StatusId = taskItem.StatusId;
        existingTask.Priority = taskItem.Priority;
        existingTask.DueDate = taskItem.DueDate;
        existingTask.CompletedDate = taskItem.CompletedDate;

        _taskItemRepository.Update(existingTask);

        return await _taskItemRepository.SaveChangesAsync();

    }



    public async Task<bool> DeleteAsync(int id)
    {
        var existingTask =
            await _taskItemRepository.GetByIdAsync(id);

        if (existingTask == null)
            return false;

        _taskItemRepository.Delete(existingTask);

        return await _taskItemRepository.SaveChangesAsync();
    }


    public async Task<List<TaskItem>> GetMyTasksAsync(int userId)
    {
        return await _taskItemRepository
            .GetByAssignedUserIdAsync(userId);
    }

    public async Task<bool> UpdateMyTaskStatusAsync(
    int taskId,
    int userId,
    int statusId)
    {
        var task = await _taskItemRepository.GetByIdAsync(taskId);

        if (task == null)
        {
            return false;
        }

        if (task.AssignedUserId != userId)
        {
            throw new UnauthorizedAccessException(
                "Bu görev size atanmış değil.");
        }

        var statusExists =
            await _taskItemRepository.StatusExistsAsync(statusId);


        if (!statusExists)
        {
            throw new InvalidOperationException(
                "Geçersiz görev durumu.");
        }

        task.StatusId = statusId;

        if (statusId == 3)
        {
            task.CompletedDate = DateTime.UtcNow;
        }
        else
        {
            task.CompletedDate = null;
        }

        _taskItemRepository.Update(task);

        return await _taskItemRepository.SaveChangesAsync();
    }
}
