using System.ComponentModel.DataAnnotations;

namespace Application.CategoryService.Domain.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public int UserId { get; set; }
        public DateTime Data { get; set; }
    }
}
