using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs.Auth;
using TaskFlow.Api.Services.Interfaces;

namespace TaskFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);

        if (token == null)
        {

            return Unauthorized(
                "Kullanıcı adı veya şifre yanlış."
            );

        }

        return Ok(new
        {
            token
        });
    }
}


