using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.DTOs;

public class Register : AccountBase
{
    [Required(ErrorMessage = "O campo nome é obrigatório.")]
    [MinLength(5, ErrorMessage = "O nome precisa ter no mínimo 5 caracteres."), MaxLength(50)]
    public required string NomeCompleto { get; set; }

    [Required(ErrorMessage = "O campo confirmar senha é obrigatório.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Senha))]
    public required string ComfirmarSenha { get; set; }
}
