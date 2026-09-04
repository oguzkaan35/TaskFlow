namespace TaskFlow.Api.Entities;

public class TaskState
{
    public int Id { get; set; }

    public string StatusName { get; set; } = string.Empty;

    public ICollection<TaskItem> Tasks { get; set; }
        = new List<TaskItem>();
}
