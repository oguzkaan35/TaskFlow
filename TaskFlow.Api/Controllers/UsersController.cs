using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs.Users;
using TaskFlow.Api.Entities;
using TaskFlow.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TaskFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();

        var response =
            _mapper.Map<List<UserResponseDto>>(users);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        var response =
            _mapper.Map<UserResponseDto>(user);

        return Ok(response);
    }



    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        var createdUser =
            await _userService.CreateAsync(user);

        var response =
            _mapper.Map<UserResponseDto>(createdUser);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response
        );
    }




    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        bool updated =
            await _userService.UpdateAsync(id, user);

        if (!updated)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        return Ok("Kullanıcı güncellendi.");
    }




    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted =
            await _userService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        return Ok("Kullanıcı silindi.");
    }
}
