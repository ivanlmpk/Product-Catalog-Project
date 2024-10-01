using _1_BaseDTOs.Product;
using _1_BaseDTOs.Responses;
using ExternalServices.Helpers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;

namespace ExternalServices.Services.Products;

public class ESProductService : IESProductService
{
    private readonly HttpClient _httpClient;
    private const string _authUrl = "api/v1/product";

    public ESProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ProductServiceClient");
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync()
    {
        CheckBaseAdress();
        var result = await _httpClient.GetAsync($"{_authUrl}/get-all");

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao obter a lista de produtos.");

        var products = await result.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();

        return products!;
    }
    public async Task<ProductDTO> GetByIdAsync(int id)
    {
        CheckBaseAdress();
        var result = await _httpClient.GetAsync($"{_authUrl}/{id}");

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao obter o produto.");

        var product = await result.Content.ReadFromJsonAsync<ProductDTO>();
        if (product == null)
            throw new Exception("Não foi encontrado um produto para esse Id.");

        return product;
    }

    public async Task<bool> CheckIfExists(string name)
    {
        CheckBaseAdress();
        var result = await _httpClient.GetAsync($"{_authUrl}/{name}");

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao checar produto.");

        var productChecked = await result.Content.ReadFromJsonAsync<bool>();

        return productChecked;  
    }

    public async Task<ProductDTO> CreateAsync(ProductDTO product)
    {
        CheckBaseAdress();
        var result = await _httpClient.PostAsJsonAsync($"{_authUrl}/create", product);
        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao registrar produto.");

        var createdProduct = await result.Content.ReadFromJsonAsync<ProductDTO>();
        if (createdProduct == null || createdProduct.Id == 0)
            throw new Exception("Não foi possível criar o produto");

        return createdProduct;
    }
    public async Task<ProductDTO> UpdateAsync(ProductDTO product)
    {
        CheckBaseAdress();
        var result = await _httpClient.PutAsJsonAsync($"{_authUrl}/{product.Id}", product);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao atualizar o produto.");

        var updatedProduct = await result.Content.ReadFromJsonAsync<ProductDTO>();
        if (updatedProduct == null)
            throw new Exception("Não foi possível atualizar o produto.");

        return updatedProduct;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        CheckBaseAdress();
        var result = await _httpClient.DeleteAsync($"{_authUrl}/{id}");

        if (!result.IsSuccessStatusCode)
            throw new Exception("Erro ao deletar produto.");

        return true;
    }

    private void CheckBaseAdress()
    {
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7135/");
        }
    }
}
