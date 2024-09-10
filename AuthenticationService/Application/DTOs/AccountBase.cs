using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.DTOs;

public class AccountBase
{
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "O e-mail inserido não é válido.")]
    [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "O campo senha é obrigatório.")]
    public required string Senha { get; set; }
}
