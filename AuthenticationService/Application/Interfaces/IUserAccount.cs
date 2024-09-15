using _1_BaseDTOs.Login;
using _1_BaseDTOs.Token;
using _1_BaseDTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Application.Interfaces;

public interface IUserAccount
{
    Task<GeneralResponse> Register(Register userRegister);

    Task<LoginResponse> Login(Login userLogin);

    Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
}
