using System.ComponentModel.DataAnnotations;

namespace _1_BaseDTOs;

public class AccountBase
{
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "O e-mail inserido não é válido.")]
    [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "O campo senha é obrigatório.")]
    public string? Senha { get; set; }
}
