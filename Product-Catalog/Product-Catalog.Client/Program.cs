using _1_BaseDTOs.Session;
using Blazored.LocalStorage;
using ExternalServices.Helpers;
using ExternalServices.Services.Authentication;
using ExternalServices.Services.Categories;
using ExternalServices.Services.Products;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7079/");
});

// Adicionando um novo HttpClient para o novo microsserviço
builder.Services.AddHttpClient("ProductServiceClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7135/"); // Substitua pela URL base do seu novo microsserviço
});

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7079/") });
var configRender = builder.Configuration.GetSection("ClientRendering:IsRendered");
if (configRender.Value == "False")
{
    configRender.Value = "True";
}

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IESAuthenticationService, ESAuthenticationService>();
builder.Services.AddScoped<IESProductService, ESProductService>();
builder.Services.AddScoped<IESCategoryService, ESCategoryService>();
builder.Services.AddSingleton<UserSession>();
builder.Services.AddMudServices();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; 
    options.InstanceName = "SampleInstance";
});

await builder.Build().RunAsync();

