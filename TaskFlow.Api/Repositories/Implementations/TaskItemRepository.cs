using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Entities;
using TaskFlow.Api.Repositories.Interfaces;

namespace TaskFlow.Api.Repositories.Implementations;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly TaskFlowDbContext _context;

    public TaskItemRepository(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _context.TaskItems
            .Include(x => x.AssignedUser)
            .Include(x => x.Project)
            .Include(x => x.Status)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.TaskItems
            .Include(x => x.AssignedUser)
            .Include(x => x.Project)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users
            .AnyAsync(x => x.Id == userId);
    }

    public async Task<bool> ProjectExistsAsync(int projectId)
    {
        return await _context.Projects
            .AnyAsync(x => x.Id == projectId);
    }

    public async Task<bool> StatusExistsAsync(int statusId)
    {
        return await _context.TaskStatuses
            .AnyAsync(x => x.Id == statusId);
    }

    public async Task AddAsync(TaskItem taskItem)
    {
        await _context.TaskItems.AddAsync(taskItem);
    }

    public void Update(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
    }

    public void Delete(TaskItem taskItem)
    {
        _context.TaskItems.Remove(taskItem);
    }


    public async Task<List<TaskItem>> GetByAssignedUserIdAsync(int userId)
    {
        return await _context.TaskItems
            .Include(x => x.AssignedUser)
            .Include(x => x.Project)
            .Include(x => x.Status)
            .AsNoTracking()
            .Where(x => x.AssignedUserId == userId)
            .ToListAsync();
    }




    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
