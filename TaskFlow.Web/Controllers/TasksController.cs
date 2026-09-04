using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using TaskFlow.Web.Models;

namespace TaskFlow.Web.Controllers;

public class TasksController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TasksController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> MyTasks()
    {
        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {

            return RedirectToAction("Login", "Auth");

        }


        var client = _httpClientFactory.CreateClient();



        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(
            "https://localhost:7000/api/TaskItems/my-tasks"
        );



        if (!response.IsSuccessStatusCode)
        {
            return View(new List<TaskItemViewModel>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var tasks = JsonSerializer.Deserialize<List<TaskItemViewModel>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        return View(tasks ?? new List<TaskItemViewModel>());
    }




    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int taskId, int statusId)
    {
        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PutAsync(
            $"https://localhost:7000/api/TaskItems/{taskId}/status?statusId={statusId}",
            null
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Görev durumu güncellenemedi.";
        }
        else
        {
            TempData["Success"] = "Görev durumu güncellendi.";
        }

        return RedirectToAction("MyTasks");
    }




}
