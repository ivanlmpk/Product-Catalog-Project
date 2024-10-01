using System.ComponentModel.DataAnnotations;

namespace _1_BaseDTOs.Product;

public class ProductDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal? Preco { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public DateTime Data { get; set; }
}
