using _1_BaseDTOs;
using AuthenticationService.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Application.Interfaces;

public interface IUserAccount
{
    Task<IActionResult> Register(Register userRegister);

    Task<LoginResponse> Login(Login userLogin);

    Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
}
