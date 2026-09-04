namespace TaskFlow.Api.DTOs.TaskItems;

public class TaskItemResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int AssignedUserId { get; set; }

    public int ProjectId { get; set; }

    public int StatusId { get; set; }

    public string Priority { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string AssignedUserName { get; set; } = string.Empty;

    public string ProjectName { get; set; } = string.Empty;

    public string StatusName { get; set; } = string.Empty;

}
