using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AuthenticationService.Helpers;
using AuthenticationService.Domain.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

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

        var getUser = await FindUserByEmail(userRegister.Email);

        if (getUser != null)
            return new BadRequestObjectResult(userRegister.Email);

        var entity = _context.ApplicationUsers.Add(new ApplicationUser()
        {
            Nome = userRegister.NomeCompleto,
            Email = userRegister.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(userRegister.Senha),
        }).Entity;

        await _context.SaveChangesAsync();

        await AddToDatabase(new UserRole()
        {
            ApplicationUser = entity,
            ApplicationUserId = entity.Id,
            Role = entity.Nome == "Ivan Lempek" ? RoleType.Admin : RoleType.StandardUser
        });

        await _context.SaveChangesAsync();

        return new OkObjectResult("Usuário cadastrado com sucesso!");
    }

    public async Task<LoginResponse> Login(Login userLogin)
    {
        if (string.IsNullOrWhiteSpace(userLogin.Email) || string.IsNullOrWhiteSpace(userLogin.Senha))
            return new LoginResponse(false, "O campo de e-mail ou senha está vazio.");

        var applicationUser = await FindUserByEmail(userLogin.Email);

        if (applicationUser == null)
            return new LoginResponse(false, "Não foi encontrado um usuário para esse e-mail e senha.");

        var verifyPassword = BCrypt.Net.BCrypt.Verify(userLogin.Senha, applicationUser.Senha);

        if (!verifyPassword)
            return new LoginResponse(false, "Senha inválida.");

        var getRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.ApplicationUserId == applicationUser.Id);

        string jwtToken = GenerateToken(applicationUser, getRole!.Role);
        string refreshToken = GenerateRefreshToken();

        return new LoginResponse(true, "Login feito com sucesso", jwtToken, refreshToken);
    }

    private string GenerateToken(ApplicationUser user, RoleType role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Nome),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config.Value.Issuer,
            audience: _config.Value.Audience,
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private async Task<ApplicationUser> FindUserByEmail(string email) =>
        await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower()! == email.ToLower()!);

    private async Task<T> AddToDatabase<T>(T model)
    {
        var result = _context.Add(model!);
        await _context.SaveChangesAsync();

        return (T)result.Entity;
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenInfo token)
    {
        if (token == null) 
            return new LoginResponse(false, "O token está vazio");

        var findToken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(t => t.Token == token.Token);
        if (findToken == null)
            return new LoginResponse(false, "O refresh token é obrigatório");

        var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == findToken.UserId);
        if (user == null)
            return new LoginResponse(false, "O refresh token não pode ser gerado pois o usuário não existe");

        var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.ApplicationUserId == user.Id);

        string jwtToken = GenerateToken(user, userRole!.Role);
        string refreshToken = GenerateRefreshToken();

        var updateRefreshToken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(t => t.UserId == user.Id);
        if (updateRefreshToken == null)
            return new LoginResponse(false, "O refresh token não pode ser gerado pois o usuário não está logado");

        updateRefreshToken.Token = refreshToken;
        await _context.SaveChangesAsync();

        return new LoginResponse(true, "O Token foi atualizado com sucesso", jwtToken, refreshToken);
    } 
}
