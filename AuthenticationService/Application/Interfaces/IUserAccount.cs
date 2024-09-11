using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Application.Interfaces;

public interface IUserAccount
{
    Task<IActionResult> Register(Register userRegister);

    Task<LoginResponse> Login(Login userLogin);

    Task<LoginResponse> RefreshTokenAsync(RefreshTokenInfo token);
}
