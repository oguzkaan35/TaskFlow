using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.DTOs.Users;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rol zorunludur.")]
    public string Role { get; set; } = "User";
}
