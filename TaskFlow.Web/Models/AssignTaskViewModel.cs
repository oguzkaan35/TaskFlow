using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Web.Models;

public class AssignTaskViewModel
{
    [Required(ErrorMessage = "Görev başlığı zorunludur.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Açıklama zorunludur.")]
    public string Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Kullanıcı seçiniz.")]
    public int AssignedUserId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Proje seçiniz.")]
    public int ProjectId { get; set; }

    public int StatusId { get; set; } = 1;

    [Required(ErrorMessage = "Öncelik seçiniz.")]
    public string Priority { get; set; } = "Orta";

    public DateTime? DueDate { get; set; }

    public List<SelectListItem> Users { get; set; } = new();

    public List<SelectListItem> Projects { get; set; } = new();
}