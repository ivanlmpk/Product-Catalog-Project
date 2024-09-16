using Blazored.LocalStorage;
using ExternalServices.Helpers;
using ExternalServices.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Product_Catalog.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7079/");
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
builder.Services.AddMudServices();

await builder.Build().RunAsync();

