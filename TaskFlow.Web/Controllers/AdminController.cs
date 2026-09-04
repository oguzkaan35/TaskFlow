using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using TaskFlow.Web.Models;

namespace TaskFlow.Web.Controllers;

public class AdminController : Controller
{



    private readonly IHttpClientFactory _httpClientFactory;

    public AdminController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }




    public IActionResult Index()
    {
        var token = HttpContext.Session.GetString("JwtToken");
        var role = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (role != "Admin")
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }



    public async Task<IActionResult> AssignTask()
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // Kullanıcıları API'den getir
        var usersResponse = await client.GetAsync(
            "https://localhost:7000/api/Users"
        );

        // Projeleri API'den getir
        var projectsResponse = await client.GetAsync(
            "https://localhost:7000/api/Projects"
        );

        var model = new AssignTaskViewModel();

        if (usersResponse.IsSuccessStatusCode)
        {
            var json = await usersResponse.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<UserListViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            model.Users = users?
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.FullName} ({x.Username})"
                })
                .ToList() ?? new();
        }

        if (projectsResponse.IsSuccessStatusCode)
        {
            var json = await projectsResponse.Content.ReadAsStringAsync();

            var projects = JsonSerializer.Deserialize<List<ProjectListViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            model.Projects = projects?
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.ProjectName
                })
                .ToList() ?? new();
        }

        return View(model);
    }






    [HttpPost]
    public async Task<IActionResult> AssignTask(AssignTaskViewModel model)
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(new
        {
            title = model.Title,
            description = model.Description,
            assignedUserId = model.AssignedUserId,
            projectId = model.ProjectId,
            statusId = 1,
            priority = model.Priority,
            dueDate = model.DueDate
        });

        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(
            "https://localhost:7000/api/TaskItems",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Görev atanırken bir hata oluştu.";
            return RedirectToAction("AssignTask");
        }

        TempData["Success"] = "Görev başarıyla atandı.";

        return RedirectToAction("AssignTask");
    }





    public async Task<IActionResult> Users()
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(
            "https://localhost:7000/api/Users"
        );

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<UserListViewModel>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var users = JsonSerializer.Deserialize<List<UserListViewModel>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        return View(users ?? new List<UserListViewModel>());
    }




    public async Task<IActionResult> Projects()
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }


        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(
            "https://localhost:7000/api/Projects"
        );

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<ProjectListViewModel>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var projects = JsonSerializer.Deserialize<List<ProjectListViewModel>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        return View(projects ?? new List<ProjectListViewModel>());
    }





    [HttpGet]
    public IActionResult CreateProject()
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        return View();
    }




    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectViewModel model)
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }


        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(new
        {
            projectName = model.ProjectName,
            description = model.Description
        });

        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(
            "https://localhost:7000/api/Projects",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Proje oluşturulurken bir hata oluştu.";
            return View(model);
        }

        TempData["Success"] = "Proje başarıyla oluşturuldu.";

        return RedirectToAction("Projects");
    }






    [HttpGet]
    public async Task<IActionResult> EditProject(int id)
    {


        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(
            $"https://localhost:7000/api/Projects/{id}"
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Proje bulunamadı.";
            return RedirectToAction("Projects");
        }

        var json = await response.Content.ReadAsStringAsync();

        var project = JsonSerializer.Deserialize<ProjectListViewModel>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (project == null)
        {
            return RedirectToAction("Projects");
        }

        var model = new UpdateProjectViewModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description
        };

        return View(model);
    }




    [HttpPost]
    public async Task<IActionResult> EditProject(UpdateProjectViewModel model)
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(new
        {
            projectName = model.ProjectName,
            description = model.Description
        });

        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await client.PutAsync(
            $"https://localhost:7000/api/Projects/{model.Id}",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Proje güncellenirken bir hata oluştu.";
            return View(model);
        }

        TempData["Success"] = "Proje başarıyla güncellendi.";

        return RedirectToAction("Projects");
    }





    [HttpPost]
    public async Task<IActionResult> DeleteProject(int id)
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync(
            $"https://localhost:7000/api/Projects/{id}"
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Proje silinemedi.";
            return RedirectToAction("Projects");
        }

        TempData["Success"] = "Proje başarıyla silindi.";

        return RedirectToAction("Projects");
    }


    /*kullanıcı işlemleri kısmı ekle sil güncelle */


    [HttpGet]
    public IActionResult CreateUser()
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        return View();
    }



    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(new
        {
            fullName = model.FullName,
            username = model.Username,
            password = model.Password,
            role = model.Role
        });

        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(
            "https://localhost:7000/api/Users",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kullanıcı oluşturulurken bir hata oluştu.";
            return View(model);
        }

        TempData["Success"] = "Kullanıcı başarıyla oluşturuldu.";

        return RedirectToAction("Users");
    }





    [HttpGet]
    public async Task<IActionResult> EditUser(int id)
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(
            $"https://localhost:7000/api/Users/{id}"
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("Users");
        }

        var json = await response.Content.ReadAsStringAsync();

        var user = JsonSerializer.Deserialize<UserListViewModel>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (user == null)
        {
            return RedirectToAction("Users");
        }

        var model = new UpdateUserViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Username = user.Username,
            Role = user.Role
        };

        return View(model);
    }




    [HttpPost]
    public async Task<IActionResult> EditUser(UpdateUserViewModel model)
    {

        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(new
        {
            fullName = model.FullName,
            role = model.Role
        });

        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await client.PutAsync(
            $"https://localhost:7000/api/Users/{model.Id}",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kullanıcı güncellenirken bir hata oluştu.";
            return View(model);
        }

        TempData["Success"] = "Kullanıcı başarıyla güncellendi.";

        return RedirectToAction("Users");
    }





    [HttpPost]
    public async Task<IActionResult> DeleteUser(int id)
    {


        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync(
            $"https://localhost:7000/api/Users/{id}"
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Kullanıcı silinemedi.";
            return RedirectToAction("Users");
        }

        TempData["Success"] = "Kullanıcı başarıyla silindi.";

        return RedirectToAction("Users");
    }

    /*task kodları*/




    [HttpGet]
    public async Task<IActionResult> EditTask(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // Düzenlenecek görevi getir
        var taskResponse = await client.GetAsync(
            $"https://localhost:7000/api/TaskItems/{id}"
        );

        if (!taskResponse.IsSuccessStatusCode)
        {
            TempData["Error"] = "Görev bulunamadı.";
            return RedirectToAction("AllTasks");
        }

        var taskJson = await taskResponse.Content.ReadAsStringAsync();

        var task = JsonSerializer.Deserialize<TaskItemViewModel>(
            taskJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (task == null)
        {
            return RedirectToAction("AllTasks");
        }

        // Kullanıcıları getir
        var usersResponse = await client.GetAsync(
            "https://localhost:7000/api/Users"
        );

        // Projeleri getir
        var projectsResponse = await client.GetAsync(
            "https://localhost:7000/api/Projects"
        );

        var model = new UpdateTaskViewModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            AssignedUserId = task.AssignedUserId,
            ProjectId = task.ProjectId,
            StatusId = task.StatusId,
            Priority = task.Priority,
            DueDate = task.DueDate
        };

        if (usersResponse.IsSuccessStatusCode)
        {
            var json = await usersResponse.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<UserListViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            model.Users = users?
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.FullName} ({x.Username})"
                })
                .ToList() ?? new();
        }

        if (projectsResponse.IsSuccessStatusCode)
        {
            var json = await projectsResponse.Content.ReadAsStringAsync();

            var projects = JsonSerializer.Deserialize<List<ProjectListViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            model.Projects = projects?
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.ProjectName
                })
                .ToList() ?? new();
        }

        // Durumlar sabit olduğu için burada oluşturuyoruz
        model.Statuses = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Bekliyor" },
        new SelectListItem { Value = "2", Text = "Devam Ediyor" },
        new SelectListItem { Value = "3", Text = "Tamamlandı" }
    };

        return View(model);
    }





    [HttpPost]
    public async Task<IActionResult> EditTask(UpdateTaskViewModel model)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(new
        {
            title = model.Title,
            description = model.Description,
            assignedUserId = model.AssignedUserId,
            projectId = model.ProjectId,
            statusId = model.StatusId,
            priority = model.Priority,
            dueDate = model.DueDate
        });

        var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await client.PutAsync(
            $"https://localhost:7000/api/TaskItems/{model.Id}",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Görev güncellenirken bir hata oluştu.";
            return RedirectToAction("AllTasks");
        }

        TempData["Success"] = "Görev başarıyla güncellendi.";

        return RedirectToAction("AllTasks");
    }









    [HttpPost]
    public async Task<IActionResult> DeleteTask(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync(
            $"https://localhost:7000/api/TaskItems/{id}"
        );

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Görev silinemedi.";
            return RedirectToAction("AllTasks");
        }

        TempData["Success"] = "Görev başarıyla silindi.";

        return RedirectToAction("AllTasks");
    }




    public async Task<IActionResult> AllTasks()
    {


        if (!IsAdmin())
        {
            return RedirectToAction("Index", "Home");
        }

        var token = HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(
            "https://localhost:7000/api/TaskItems"
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






    /*admin kontrolü*/

    private bool IsAdmin()
    {
        var token = HttpContext.Session.GetString("JwtToken");
        var role = HttpContext.Session.GetString("Role");

        return !string.IsNullOrEmpty(token) && role == "Admin";
    }

}
