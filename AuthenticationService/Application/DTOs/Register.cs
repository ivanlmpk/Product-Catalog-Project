using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.DTOs;

public class Register : AccountBase
{
    [Required]
    [MinLength(5), MaxLength(50)]
    public required string NomeCompleto { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Senha))]
    public required string ComfirmarSenha { get; set; }
}
