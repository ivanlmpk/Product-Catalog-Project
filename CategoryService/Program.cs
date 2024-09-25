using Application.CategoryService.Application.Interfaces;
using Application.CategoryService.Infrastructure.Data;
using Application.CategoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CategoryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database") ??
        throw new InvalidOperationException("Erro: Sua conexão não foi encontrada!"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();

// Cors para aceitar meus dominios
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirBlazorWasm3", builder =>
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

app.UseCors("PermitirBlazorWasm3");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
