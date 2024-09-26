using System.ComponentModel.DataAnnotations;

namespace _1_BaseDTOs.Login;

public class Register : AccountBase
{
    [Required(ErrorMessage = "O campo nome é obrigatório.")]
    [MinLength(5, ErrorMessage = "O nome precisa ter no mínimo 5 caracteres."), MaxLength(50)]
    public string NomeCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo confirmar senha é obrigatório.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Senha))]
    public string? ComfirmarSenha { get; set; }

    [Required(ErrorMessage = "O campo de telefone é obrigatório.")]
    [MaxLength(16, ErrorMessage = "Número de telefone excedeu o limite.")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "O campo cidade é obrigatório.")]
    [MaxLength(50)]
    public string? Cidade { get; set; }
}
