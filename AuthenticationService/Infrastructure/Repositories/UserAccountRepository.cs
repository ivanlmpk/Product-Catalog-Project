using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AuthenticationService.Helpers;
using AuthenticationService.Domain.Responses;

namespace AuthenticationService.Infrastructure.Repositories;

public class UserAccountRepository : IUserAccount
{
    private readonly AuthDbContext _context;
    private readonly IOptions<JwtSection> _config;

    public UserAccountRepository(AuthDbContext context, IOptions<JwtSection> config)
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
            Role = RoleType.StandardUser
        });

        await _context.SaveChangesAsync();

        return new OkObjectResult("Usuário cadastrado com sucesso!");
    }

    public async Task<LoginResponse> Login(Login userLogin)
    {
        if (string.IsNullOrWhiteSpace(userLogin.Email) || string.IsNullOrWhiteSpace(userLogin.Senha))
            return new LoginResponse(false, "O campo de e-mail ou senha está vazio.");

        var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

        if (applicationUser == null)
            return new LoginResponse(false, "Não foi encontrado um usuário para esse e-mail e senha.");

        var verifyPassword = BCrypt.Net.BCrypt.Verify(userLogin.Senha, applicationUser.Senha);

        if (!verifyPassword)
            return new LoginResponse(false, "Senha inválida.");

        var getRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.ApplicationUserId == applicationUser.Id);

        string jwtToken = GenerateToken(applicationUser, getRole!.Role!);
        string refreshToken = GenerateRefreshToken();

        return new LoginResponse(true, "Login feito com sucesso", jwtToken, refreshToken);
    }
}
