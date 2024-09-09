using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserAccount _userAccountRepository;

    public AuthenticationController(IUserAccount userAccountRepository)
    {
        _userAccountRepository = userAccountRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(Register userRegister)
    {
        return await _userAccountRepository.Register(userRegister);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Login userLogin)
    {
        return await _userAccountRepository.Login(userLogin);
    }
}
