using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using TaskFlow.Web.Models;



namespace TaskFlow.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var model = new DashboardViewModel
            {
                Username = username ?? "",
                IsAdmin = role == "Admin"
            };

            // ADMIN GÝRÝÞ YAPTIYSA
            if (model.IsAdmin)
            {
                var usersResponse = await client.GetAsync(
                    "https://localhost:7000/api/Users"
                );

                var projectsResponse = await client.GetAsync(
                    "https://localhost:7000/api/Projects"
                );

                var tasksResponse = await client.GetAsync(
                    "https://localhost:7000/api/TaskItems"
                );

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

                    model.TotalUsers = users?.Count ?? 0;
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

                    model.TotalProjects = projects?.Count ?? 0;
                }

                if (tasksResponse.IsSuccessStatusCode)
                {
                    var json = await tasksResponse.Content.ReadAsStringAsync();

                    var tasks = JsonSerializer.Deserialize<List<TaskItemViewModel>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    ) ?? new List<TaskItemViewModel>();

                    model.TotalSystemTasks = tasks.Count;

                    model.TotalCompletedSystemTasks =
                        tasks.Count(x => x.StatusId == 3);
                }

                return View(model);
            }

            // NORMAL KULLANICI GÝRÝÞ YAPTIYSA
            var response = await client.GetAsync(
                "https://localhost:7000/api/TaskItems/my-tasks"
            );

            if (!response.IsSuccessStatusCode)
            {
                return View(model);
            }

            var taskJson = await response.Content.ReadAsStringAsync();

            var myTasks = JsonSerializer.Deserialize<List<TaskItemViewModel>>(
                taskJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<TaskItemViewModel>();

            model.TotalTasks = myTasks.Count;

            model.PendingTasks =
                myTasks.Count(x => x.StatusId == 1);

            model.InProgressTasks =
                myTasks.Count(x => x.StatusId == 2);

            model.CompletedTasks =
                myTasks.Count(x => x.StatusId == 3);

            model.UpcomingTasks = myTasks
                    .Where(x =>
                     x.DueDate.HasValue &&
                     x.StatusId != 3)
                .OrderBy(x => x.DueDate)
                .Take(5)
                .ToList();

            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}
