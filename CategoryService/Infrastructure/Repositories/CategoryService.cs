using Application.CategoryService.Application.Interfaces;
using Application.CategoryService.Domain.Entities;
using Application.CategoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.CategoryService.Infrastructure.Repositories
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryDbContext _context;

        public CategoryService(CategoryDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null) throw new ArgumentException("Categoria inválida.");

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            if (id == 0) throw new ArgumentException("ID inválido.");

            var category = await _context.Categories.FindAsync(id);

            if (category == null) throw new KeyNotFoundException("Categoria não encontrada.");

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            if (category == null) throw new ArgumentException("Produto inválido.");

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id == 0) throw new ArgumentException("ID inválido.");

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
