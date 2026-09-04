using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs.TaskItems;
using TaskFlow.Api.Entities;
using TaskFlow.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace TaskFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;
    private readonly IMapper _mapper;

    public TaskItemsController(
        ITaskItemService taskItemService,
        IMapper mapper)
    {
        _taskItemService = taskItemService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskItemService.GetAllAsync();

        var response =
            _mapper.Map<List<TaskItemResponseDto>>(tasks);

        return Ok(response);
    }



    [HttpGet("my-tasks")]
    public async Task<IActionResult> GetMyTasks()
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized("Kullanıcı bilgisi token içerisinde bulunamadı.");
        }

        var tasks =
            await _taskItemService.GetMyTasksAsync(userId);

        var response =
            _mapper.Map<List<TaskItemResponseDto>>(tasks);

        return Ok(response);
    }



    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateMyTaskStatus(
    int id,
    int statusId)
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized("Kullanıcı bilgisi token içerisinde bulunamadı.");
        }

        try
        {
            bool updated =
                await _taskItemService.UpdateMyTaskStatusAsync(
                    id,
                    userId,
                    statusId);

            if (!updated)
            {
                return NotFound("Görev bulunamadı.");
            }

            return Ok("Görev durumu güncellendi.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }




    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskItemService.GetByIdAsync(id);

        if (task == null)
            return NotFound("Görev bulunamadı.");

        var response =
            _mapper.Map<TaskItemResponseDto>(task);

        return Ok(response);
    }



    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskItemDto dto)
    {
        var task =
            _mapper.Map<TaskItem>(dto);

        var createdTask =
            await _taskItemService.CreateAsync(task);

        var response =
            _mapper.Map<TaskItemResponseDto>(createdTask);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }








    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateTaskItemDto dto)
    {
        var task =
            _mapper.Map<TaskItem>(dto);

        bool updated =
            await _taskItemService.UpdateAsync(id, task);

        if (!updated)
            return NotFound("Görev bulunamadı.");

        return Ok("Görev güncellendi.");
    }



    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted =
            await _taskItemService.DeleteAsync(id);

        if (!deleted)
            return NotFound("Görev bulunamadı.");

        return Ok("Görev silindi.");
    }




    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateMyTaskStatus(
    int id,
    UpdateTaskStatusDto dto)
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized(
                "Kullanıcı bilgisi token içerisinde bulunamadı.");
        }

        bool updated =
            await _taskItemService.UpdateMyTaskStatusAsync(
                id,
                userId,
                dto.StatusId);

        if (!updated)
        {
            return NotFound("Görev bulunamadı.");
        }

        return Ok("Görev durumu güncellendi.");
    }


}
