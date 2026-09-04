using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Entities;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }




    public async Task<List<Project>> GetAllAsync()
    {
        return await _projectRepository.GetAllAsync();
    }




    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }




    public async Task<Project> CreateAsync(Project project)
    {
        bool projectExists =
            await _projectRepository.ExistsByNameAsync(project.ProjectName);

        if (projectExists)
        {
            throw new InvalidOperationException(
                "Bu isimde bir proje zaten mevcut.");
        }
        //burda iş kuralları olucak
        await _projectRepository.AddAsync(project);

        await _projectRepository.SaveChangesAsync();

        return project;
    }




    public async Task<bool> UpdateAsync(int id, Project project)
    {
        var existingProject = await _projectRepository.GetByIdAsync(id);

        if (existingProject == null)
            return false;

        existingProject.ProjectName = project.ProjectName;
        existingProject.Description = project.Description;

        _projectRepository.Update(existingProject);

        return await _projectRepository.SaveChangesAsync();
    }



    public async Task<bool> DeleteAsync(int id)
    {
        var existingProject = await _projectRepository.GetByIdAsync(id);

        if (existingProject == null)
            return false;

        _projectRepository.Delete(existingProject);

        return await _projectRepository.SaveChangesAsync();
    }

    

}
