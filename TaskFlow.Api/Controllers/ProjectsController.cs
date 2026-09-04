using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs.Projects;
using TaskFlow.Api.Entities;
using TaskFlow.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TaskFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ProjectsController(
        IProjectService projectService,
        IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllAsync();

        var response =
            _mapper.Map<List<ProjectResponseDto>>(projects);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetByIdAsync(id);

        if (project == null)
        {
            return NotFound("Proje bulunamadı.");
        }

        var response =
            _mapper.Map<ProjectResponseDto>(project);

        return Ok(response);
    }




    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);

        var createdProject =
            await _projectService.CreateAsync(project);

        var response =
            _mapper.Map<ProjectResponseDto>(createdProject);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response
        );
    }





    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);

        bool updated =
            await _projectService.UpdateAsync(id, project);

        if (!updated)
        {
            return NotFound("Proje bulunamadı.");
        }

        return Ok("Proje güncellendi.");
    }




    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted =
            await _projectService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound("Proje bulunamadı.");
        }

        return Ok("Proje silindi.");
    }
}
