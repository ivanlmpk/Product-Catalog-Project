using _1_BaseDTOs.Category;

namespace ExternalServices.Services.Categories;

public interface IESCategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAllAsync();
    Task<CategoryDTO> GetByIdAsync(int id);
    Task<CategoryDTO> CreateAsync(CategoryDTO category);
    Task<CategoryDTO> UpdateAsync(CategoryDTO category);
    Task<bool> DeleteAsync(int id);
}
