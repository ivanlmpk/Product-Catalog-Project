using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AuthenticationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(AuthDbContext context, IConfiguration config) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(Register userRegister)
    {
        if (userRegister == null) 
            return BadRequest();

        var getUser = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == userRegister.Email);

        if (getUser != null) 
            return BadRequest(userRegister.Email);

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
    public async Task<IActionResult> Login(Login userLogin)
    {
        if (string.IsNullOrWhiteSpace(userLogin.Email) || string.IsNullOrWhiteSpace(userLogin.Senha))
            return BadRequest("O campo de e-mail ou senha está vazio.");

        var getUserLogin = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

        if (getUserLogin == null) 
            return BadRequest("Não foi encontrado um usuário para esse e-mail e senha.");

        var verifyPassword = BCrypt.Net.BCrypt.Verify(userLogin.Senha, getUserLogin.Senha);

        if (!verifyPassword)
            return NotFound("Senha inválida.");

        var getRole = await context.UserRoles.FirstOrDefaultAsync(r => r.ApplicationUserId == getUserLogin.Id);

        string key = $"{config["Authentication:Key"]}.{getUserLogin.Nome}.{getUserLogin.Id}";

        string accessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));

        return Ok($"Token de acesso: {accessToken}");
    }
}
