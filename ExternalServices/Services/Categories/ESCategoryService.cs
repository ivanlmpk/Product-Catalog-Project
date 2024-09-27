using _1_BaseDTOs.Category;
using System.Net.Http.Json;

namespace ExternalServices.Services.Categories;

public class ESCategoryService : IESCategoryService
{
    private readonly HttpClient _httpClient;
    private const string _authUrl = "api/v1/category";

    public ESCategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CategoryServiceBaseUrl");
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
    {
        CheckBaseAdress();
        var result = await _httpClient.GetAsync($"{_authUrl}/get-all");

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao obter a lista de categorias.");

        var categories = await result.Content.ReadFromJsonAsync<IEnumerable<CategoryDTO>>();

        return categories!;
    }
    public async Task<CategoryDTO> GetByIdAsync(int id)
    {
        CheckBaseAdress();
        var result = await _httpClient.GetAsync($"{_authUrl}/{id}");

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao obter a categoria.");

        var category = await result.Content.ReadFromJsonAsync<CategoryDTO>();
        if (category == null)
            throw new Exception("Não foi encontrado uma categoria para esse Id.");

        return category;
    }

    public async Task<CategoryDTO> CreateAsync(CategoryDTO category)
    {
        CheckBaseAdress();
        var result = await _httpClient.PostAsJsonAsync($"{_authUrl}/create", category);
        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao registrar categoria.");

        var createdCategory = await result.Content.ReadFromJsonAsync<CategoryDTO>();
        if (createdCategory == null || createdCategory.Id == 0)
            throw new Exception("Não foi possível criar a categoria");

        return createdCategory;
    }
    public async Task<CategoryDTO> UpdateAsync(CategoryDTO category)
    {
        CheckBaseAdress();
        var result = await _httpClient.PutAsJsonAsync($"{_authUrl}/{category.Id}", category);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao atualizar a categoria.");

        var updatedCategory = await result.Content.ReadFromJsonAsync<CategoryDTO>();
        if (updatedCategory == null)
            throw new Exception("Não foi possível atualizar a categoria.");

        return updatedCategory;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        CheckBaseAdress();
        var result = await _httpClient.DeleteAsync($"{_authUrl}/{id}");

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao deletar categoria.");

        return true;
    }

    private void CheckBaseAdress()
    {
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
        }
    }
}
