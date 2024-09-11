using AuthenticationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options) 
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }
}

    

