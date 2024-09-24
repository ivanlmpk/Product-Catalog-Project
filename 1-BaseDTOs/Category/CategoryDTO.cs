namespace _1_BaseDTOs.Category;

public class CategoryDTO
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public int UserId { get; set; }
    public DateTime Data { get; set; }
}
