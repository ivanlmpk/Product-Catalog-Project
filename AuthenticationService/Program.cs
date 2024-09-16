using AuthenticationService.Application.Interfaces;
using AuthenticationService.Helpers;
using AuthenticationService.Infrastructure.Data;
using AuthenticationService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database") ??
        throw new InvalidOperationException("Erro: Sua conexão não foi encontrada!"));
});

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("Authentication"));
builder.Services.AddScoped<IUserAccount, UserAccountRepository>();

// Cors para aceitar todos dominios
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirBlazorWasm", builder =>
    {
        builder.WithOrigins("http://localhost:5039", "https://localhost:7057")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirBlazorWasm");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
