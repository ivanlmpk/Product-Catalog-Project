using Application.ProductService.Application.Interfaces;
using Application.ProductService.Infrastructure.Data;
using Application.ProductService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "APIProduto",
        Description = "API para armazenar os produtos do catálogo.",
        Version = "v1"
    })
);
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database") ??
        throw new InvalidOperationException("Erro: Sua conexão não foi encontrada!"));
});

builder.Services.AddScoped<IProductService, ProductService>();

// Cors para aceitar meus dominios
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirBlazorWasm2", builder =>
    {
        builder.WithOrigins("http://localhost:5039", "https://localhost:7057")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; 
    options.InstanceName = "RedisInstance";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirBlazorWasm2");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
