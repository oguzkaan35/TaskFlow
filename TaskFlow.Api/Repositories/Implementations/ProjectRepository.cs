using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Entities;
using TaskFlow.Api.Repositories.Interfaces;

namespace TaskFlow.Api.Repositories.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly TaskFlowDbContext _context;

    public ProjectRepository(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string projectName)
    {
        return await _context.Projects
            .AnyAsync(x => x.ProjectName == projectName);
    }


    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
