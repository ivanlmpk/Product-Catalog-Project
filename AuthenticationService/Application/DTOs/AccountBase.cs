using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.DTOs;

public class AccountBase
{
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    [Required]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public required string Senha { get; set; }
}
