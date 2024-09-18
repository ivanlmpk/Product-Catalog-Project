using Application.ProductService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.ProductService.Infrastructure.Data
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
