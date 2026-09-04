using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.DTOs.Projects;

public class CreateProjectDto
{
    [Required(ErrorMessage = "Proje adı zorunludur.")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Proje adı 3 ile 100 karakter arasında olmalıdır.")]
    public string ProjectName { get; set; } = string.Empty;

    [StringLength(500,
        ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? Description { get; set; }
}
