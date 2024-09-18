using Application.CategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.CategoryService.Infrastructure.Data
{
    public class CategoryDbContext(DbContextOptions<CategoryDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
    }
}
