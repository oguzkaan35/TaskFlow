namespace TaskFlow.Api.Entities;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int AssignedUserId { get; set; }

    public int ProjectId { get; set; }

    public int StatusId { get; set; }

    public string Priority { get; set; } = "Orta";

    public User AssignedUser { get; set; } = null!;

    public Project Project { get; set; } = null!;

    public TaskState Status { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; }
        = new List<Comment>();

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }



}
