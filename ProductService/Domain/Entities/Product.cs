using System.ComponentModel.DataAnnotations;

namespace Application.ProductService.Domain.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Titulo { get; set; }
    [Required]
    public string? Descricao { get; set; }
    [Required]
    public decimal? Preco { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public DateTime Data { get; set; }
}
