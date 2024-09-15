using _1_BaseDTOs.Login;
using _1_BaseDTOs.Responses;
using _1_BaseDTOs.Token;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace ExternalServices.Authentication;

public class ESAuthenticationService : IESAuthenticationService
{
    public readonly HttpClient _httpClient;
    public readonly string _baseUrl;
    public readonly string _ApiVersion = "v1";
    public readonly string _controller = "Authentication";

    public ESAuthenticationService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ExternalServices:AuthenticationServiceBaseUrl"];
    }

    public async Task<LoginResponse> Login(Login userLogin)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/{_ApiVersion}/{_controller}/login", userLogin);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResponse> Register(Register userRegister)
    {
        throw new NotImplementedException();
    }
}
