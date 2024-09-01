using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Domain.Entities;

public class UserRole
{
    [Key]
    public int Id { get; set; }
    public int ApplicationUserId { get; set; }
    public string? Role { get; set; }
}
