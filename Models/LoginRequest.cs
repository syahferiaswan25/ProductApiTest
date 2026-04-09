namespace ProductApi.Models;
using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required(ErrorMessage = "Username wajib diisi")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password wajib diisi")]
    public string Password { get; set; }
}