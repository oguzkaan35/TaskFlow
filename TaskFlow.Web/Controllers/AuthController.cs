using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TaskFlow.Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TaskFlow.Web.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        var json = JsonSerializer.Serialize(new
        {
            username = model.Username,
            password = model.Password
        });

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(
            "https://localhost:7000/api/Auth/login",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kullanıcı adı veya şifre yanlış.";
            return View(model);
        }

        var responseContent =
            await response.Content.ReadAsStringAsync();

        using var document =
            JsonDocument.Parse(responseContent);

        var token =
            document.RootElement
                .GetProperty("token")
                .GetString();



        HttpContext.Session.SetString("JwtToken", token!);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var role = jwtToken.Claims
            .FirstOrDefault(x =>
                x.Type == ClaimTypes.Role ||
                x.Type == "role" ||
                x.Type.EndsWith("/role"))
            ?.Value;

        if (!string.IsNullOrEmpty(role))
        {
            HttpContext.Session.SetString("Role", role);
        }

        HttpContext.Session.SetString("Username", model.Username);

        return RedirectToAction(
            "Index",
            "Home"
        );

    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login", "Auth");
    }


}


