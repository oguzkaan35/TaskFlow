using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.DTOs.TaskItems;

public class UpdateTaskItemDto
{
    [Required(ErrorMessage = "Görev başlığı zorunludur.")]
    [StringLength(150, MinimumLength = 3,
        ErrorMessage = "Görev başlığı 3 ile 150 karakter arasında olmalıdır.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000,
        ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
    public string? Description { get; set; }

    [Range(1, int.MaxValue,
        ErrorMessage = "Geçerli bir kullanıcı seçilmelidir.")]
    public int AssignedUserId { get; set; }

    [Range(1, int.MaxValue,
        ErrorMessage = "Geçerli bir proje seçilmelidir.")]
    public int ProjectId { get; set; }

    [Range(1, int.MaxValue,
        ErrorMessage = "Geçerli bir görev durumu seçilmelidir.")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Öncelik bilgisi zorunludur.")]
    public string Priority { get; set; } = "Orta";

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}
