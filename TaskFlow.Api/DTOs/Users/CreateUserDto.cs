using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.DTOs.Users;

public class CreateUserDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rol zorunludur.")]
    public string Role { get; set; } = "User";
}
