using System.ComponentModel.DataAnnotations;

namespace _1_BaseDTOs;

public class Register : AccountBase
{
    [Required(ErrorMessage = "O campo nome é obrigatório.")]
    [MinLength(5, ErrorMessage = "O nome precisa ter no mínimo 5 caracteres."), MaxLength(50)]
    public string NomeCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo confirmar senha é obrigatório.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Senha))]
    public string? ComfirmarSenha { get; set; }

    public string? Telefone { get; set; }

    public string? Cidade { get; set; }
}
