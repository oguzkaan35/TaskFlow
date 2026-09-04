namespace TaskFlow.Api.Entities;

public class Comment
{
    public int Id { get; set; }

    public int TaskItemId { get; set; }

    public int UserId { get; set; }

    public TaskItem TaskItem { get; set; } = null!;

    public User User { get; set; } = null!;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
