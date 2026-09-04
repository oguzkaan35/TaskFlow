namespace TaskFlow.Web.Models;

public class DashboardViewModel
{
    public string Username { get; set; } = string.Empty;

    public int TotalTasks { get; set; }

    public int PendingTasks { get; set; }

    public int InProgressTasks { get; set; }

    public int CompletedTasks { get; set; }

    public bool IsAdmin { get; set; }

    public int TotalUsers { get; set; }

    public int TotalProjects { get; set; }

    public int TotalSystemTasks { get; set; }

    public int TotalCompletedSystemTasks { get; set; }

    public List<TaskItemViewModel> UpcomingTasks { get; set; } = new();
}
