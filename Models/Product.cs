using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name wajib diisi")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name 3-100 karakter")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description wajib diisi")]
    [StringLength(500, ErrorMessage = "Max 500 karakter")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price harus lebih dari 0")]
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}