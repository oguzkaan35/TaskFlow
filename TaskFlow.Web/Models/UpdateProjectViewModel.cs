using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Web.Models;

public class UpdateProjectViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Proje adı zorunludur.")]
    public string ProjectName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
