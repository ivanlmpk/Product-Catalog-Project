using _1_BaseDTOs.Login;
using _1_BaseDTOs.Responses;
using _1_BaseDTOs.Token;
using ExternalServices.Helpers;
using System.Net.Http.Json;

namespace ExternalServices.Services.Authentication;

public class ESAuthenticationService(GetHttpClient getHttpClient) : IESAuthenticationService
{
    //public readonly HttpClient _httpClient;
    public string AuthUrl = "api/v1/authentication";

    public readonly string _baseUrL;
    public readonly string _ApiVersion = "v1";
    public readonly string _controller = "Authentication";

    //public ESAuthenticationService(HttpClient httpClient, IConfiguration configuration)
    //{
    //    _httpClient = httpClient;
    //    _baseUrl = configuration["ExternalServices:AuthenticationServiceBaseUrl"];
    //}

    public async Task<LoginResponse> Login(Login userLogin)
    {
        //var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/{_ApiVersion}/{_controller}/login", userLogin);
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", userLogin);
        if (!result.IsSuccessStatusCode)
            return new LoginResponse(false, "Erro ao logar.");

        return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
    }
    public async Task<GeneralResponse> Register(Register userRegister)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/register", userRegister);
        if (!result.IsSuccessStatusCode)
            return new GeneralResponse(false, "Erro ao registar.");

        return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
    }

    public Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
    {
        throw new NotImplementedException();
    }

}
