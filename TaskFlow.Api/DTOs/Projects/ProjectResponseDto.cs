namespace TaskFlow.Api.DTOs.Projects;

public class ProjectResponseDto
{
    public int Id { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }
}
