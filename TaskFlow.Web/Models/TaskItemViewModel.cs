namespace TaskFlow.Web.Models;

public class TaskItemViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int AssignedUserId { get; set; }

    public string AssignedUserName { get; set; } = string.Empty;

    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public int StatusId { get; set; }

    public string StatusName { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}
