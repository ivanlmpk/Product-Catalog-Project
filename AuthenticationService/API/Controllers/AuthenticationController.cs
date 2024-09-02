using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(AuthDbContext context, IConfiguration config) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(Register userRegister)
    {
        if (userRegister == null) return BadRequest();

        var getUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == userRegister.Email);

        if (getUser != null) return BadRequest(userRegister.Email);

        var entity = context.ApplicationUsers.Add(new ApplicationUser()
        {
            Nome = userRegister.NomeCompleto,
            Email = userRegister.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(userRegister.Senha),
        }).Entity;

        await context.SaveChangesAsync();

        context.UserRoles.Add(new UserRole()
        {
            ApplicationUserId = entity.Id,
            Role = "Usuário padrão"
        });

        await context.SaveChangesAsync();

        return Ok("Usuário cadastrado com sucesso!");
    }

    [HttpPost("login")]
    public Login(Login userLogin)
    {

    }
}
