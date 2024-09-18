using Application.ProductService.Application.Interfaces;
using Application.ProductService.Domain.Entities;
using Application.ProductService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.ProductService.Infrastructure.Repositories;

public class ProductService : IProductService
{
    private readonly ProductDbContext _context;

    public ProductService(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        if (product == null) throw new ArgumentException("Produto inválido.");

        _context.Products.Add(product);

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        if (id == 0) throw new ArgumentException("ID inválido.");

        var product = await _context.Products.FindAsync(id);

        if (product == null) throw new KeyNotFoundException("Produto não encontrado.");

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        if (product == null) throw new ArgumentException("Produto inválido.");

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id == 0) throw new ArgumentException("ID inválido.");

        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
