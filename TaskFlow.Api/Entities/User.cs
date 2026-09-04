namespace TaskFlow.Api.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<TaskItem> AssignedTasks { get; set; }
    = new List<TaskItem>();

    public ICollection<Comment> Comments { get; set; }
        = new List<Comment>();

}
