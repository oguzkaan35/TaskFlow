using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskFlow.Web.Models;

public class UpdateTaskViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int AssignedUserId { get; set; }

    public int ProjectId { get; set; }

    public int StatusId { get; set; }

    public string Priority { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    public List<SelectListItem> Users { get; set; } = new();

    public List<SelectListItem> Projects { get; set; } = new();

    public List<SelectListItem> Statuses { get; set; } = new();
}
