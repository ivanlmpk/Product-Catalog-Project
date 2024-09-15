﻿using _1_BaseDTOs.Login;
using _1_BaseDTOs.Responses;
using _1_BaseDTOs.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace _2_ExternalServices.Authentication;

public class ESAuthenticationService : IESAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ESAuthenticationService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ExternalServices:AuthenticationServiceBaseUrl"];
    }

    public async Task<LoginResponse> Login(Login userLogin)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/login", userLogin);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Register(Register userRegister)
    {
        throw new NotImplementedException();
    }
}
