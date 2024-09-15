using _1_BaseDTOs.Login;
using _1_BaseDTOs.Responses;
using _1_BaseDTOs.Token;
using Microsoft.AspNetCore.Mvc;

namespace _2_ExternalServices.Authentication;

public interface IESAuthenticationService
{
    Task<IActionResult> Register(Register userRegister);

    Task<LoginResponse> Login(Login userLogin);

    Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
}
