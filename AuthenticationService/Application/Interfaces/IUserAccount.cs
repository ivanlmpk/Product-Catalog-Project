using AuthenticationService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Application.Interfaces;

public interface IUserAccount
{
    Task<IActionResult> Register(Register userRegister);

    Task<IActionResult> Login(Login userLogin);
}
