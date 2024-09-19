namespace _1_BaseDTOs.Product;

public class ProductDTO
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public decimal? Preco { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public DateTime Data { get; set; }
}
