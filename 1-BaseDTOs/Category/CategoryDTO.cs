using System.ComponentModel.DataAnnotations;

namespace _1_BaseDTOs.Category;

public class CategoryDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public int UserId { get; set; }
    public DateTime Data { get; set; }
}
