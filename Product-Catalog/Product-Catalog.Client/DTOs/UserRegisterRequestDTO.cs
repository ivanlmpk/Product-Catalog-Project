namespace Product_Catalog.Client.DTOs
{
    public class UserRegisterRequestDTO
    {
        public int Id { get; set; }
        public string? NomeCompleto { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Cidade { get; set; }
    }
}
