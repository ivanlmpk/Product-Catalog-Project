using _1_BaseDTOs.Product;

namespace ExternalServices.Services.Products;

public interface IESProductService
{
    Task<IEnumerable<ProductDTO>> GetAllAsync();
    Task<ProductDTO> GetByIdAsync(int id);
    Task<ProductDTO> CreateAsync(ProductDTO product);
    Task<ProductDTO> UpdateAsync(ProductDTO product);
    Task<bool> DeleteAsync(int id);
}
