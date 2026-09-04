using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Web.Models;

public class UpdateUserViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rol seçmelisiniz.")]
    public string Role { get; set; } = "User";
}
