using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Username wajib diisi")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username 3-50 karakter")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password wajib diisi")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password minimal 6 karakter")]
    public string Password { get; set; }
}