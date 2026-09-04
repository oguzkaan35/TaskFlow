
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Web.Models;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rol seçmelisiniz.")]
    public string Role { get; set; } = "User";
}
