using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Domain.Entities;

public class UserRole
{
    [Key]
    public int Id { get; set; }
    public int ApplicationUserId { get; set; }
    public RoleType Role { get; set; }
}

public enum RoleType
{
    Admin,
    StandardUser,
    PremiumUser
}