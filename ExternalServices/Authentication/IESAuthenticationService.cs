using _1_BaseDTOs.Login;
using _1_BaseDTOs.Responses;
using _1_BaseDTOs.Token;

namespace ExternalServices.Authentication;

public interface IESAuthenticationService
{
    Task<GeneralResponse> Register(Register userRegister);

    Task<LoginResponse> Login(Login userLogin);

    Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
}
