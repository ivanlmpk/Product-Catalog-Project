using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuthenticationService.Infrastructure.Repositories;

public class UserAccountRepository : IUserAccount
{
    private readonly AuthDbContext _context;
    private readonly IConfiguration _config;

    public UserAccountRepository(AuthDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<IActionResult> Register(Register userRegister)
    {
        if (userRegister == null)
            return new BadRequestResult();

        var getUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == userRegister.Email);

        if (getUser != null)
            return new BadRequestObjectResult(userRegister.Email);

        var entity = _context.ApplicationUsers.Add(new ApplicationUser()
        {
            Nome = userRegister.NomeCompleto,
            Email = userRegister.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(userRegister.Senha),
        }).Entity;

        await _context.SaveChangesAsync();

        _context.UserRoles.Add(new UserRole()
        {
            ApplicationUserId = entity.Id,
            Role = "Usuário padrão"
        });

        await _context.SaveChangesAsync();

        return new OkObjectResult("Usuário cadastrado com sucesso!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Login userLogin)
    {
        if (string.IsNullOrWhiteSpace(userLogin.Email) || string.IsNullOrWhiteSpace(userLogin.Senha))
            return new BadRequestObjectResult("O campo de e-mail ou senha está vazio.");

        var getUserLogin = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

        if (getUserLogin == null)
            return new NotFoundObjectResult("Não foi encontrado um usuário para esse e-mail e senha.");

        var verifyPassword = BCrypt.Net.BCrypt.Verify(userLogin.Senha, getUserLogin.Senha);

        if (!verifyPassword)
            return new NotFoundObjectResult("Senha inválida.");

        var getRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.ApplicationUserId == getUserLogin.Id);

        string key = $"{_config["Authentication:Key"]}.{getUserLogin.Nome}.{getUserLogin.Id}";

        string accessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));

        return new OkObjectResult($"Token de acesso: {accessToken}");
    }
}
