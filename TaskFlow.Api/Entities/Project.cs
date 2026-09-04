namespace TaskFlow.Api.Entities;

public class Project
{
    public int Id { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<TaskItem> Tasks { get; set; }
      = new List<TaskItem>();

}
