using _1_BaseDTOs.Login;
using _1_BaseDTOs.Token;
using AuthenticationService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserAccount _accountInterface;

    public AuthenticationController(IUserAccount userAccountInterface)
    {
        _accountInterface = userAccountInterface;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(Register userRegister)
    {
        if (userRegister == null) return BadRequest("Preencha os dados para efetuar o registro.");
        var result = await _accountInterface.Register(userRegister);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Login userLogin)
    {
        if (userLogin == null) return BadRequest("Preencha os campos para efetuar o login.");
        var result = await _accountInterface.Login(userLogin);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshToken token)
    {
        if (token == null) return BadRequest("O token está vazio");
        var result = await _accountInterface.RefreshTokenAsync(token);

        return Ok(result);
    }
}
