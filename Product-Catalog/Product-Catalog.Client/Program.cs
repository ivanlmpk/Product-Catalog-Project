using ExternalServices.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IESAuthenticationService, ESAuthenticationService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//var configuration = builder.Configuration;
//var authServiceBaseUrl = configuration["ExternalServices:AuthenticationServiceBaseUrl"];

//// Registrar o serviço de autenticação com HttpClient e passando a configuração
//builder.Services.AddScoped<IESAuthenticationService>(sp =>
//    new ESAuthenticationService(sp.GetRequiredService<HttpClient>(), configuration));

builder.Services.AddMudServices();

await builder.Build().RunAsync();
