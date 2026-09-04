using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.DTOs.TaskItems;

public class UpdateTaskStatusDto
{
    [Range(1, int.MaxValue,
        ErrorMessage = "Geçerli bir görev durumu seçilmelidir.")]
    public int StatusId { get; set; }
}