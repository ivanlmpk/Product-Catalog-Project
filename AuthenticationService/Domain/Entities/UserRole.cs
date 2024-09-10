using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationService.Domain.Entities;

public class UserRole
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("ApplicationUser")]
    public int ApplicationUserId { get; set; }

    public RoleType Role { get; set; }

    public required ApplicationUser ApplicationUser { get; set; }
}

public enum RoleType
{
    Admin,
    StandardUser,
    PremiumUser
}