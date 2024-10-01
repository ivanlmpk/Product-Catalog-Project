using _1_BaseDTOs.Product;
using Application.ProductService.Application.Interfaces;
using Application.ProductService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.ProductService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IDistributedCache _cache;

        public ProductController(IProductService productService, IDistributedCache cache)
        {
            _productService = productService;
            _cache = cache; 
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            string cacheKey = "all_products";
            string serializedProducts;
            var cachedProducts = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedProducts))
            {
                // Se os dados estão no cache, desserializa e retorna
                var products = JsonConvert.DeserializeObject<List<ProductDTO>>(cachedProducts);
                return Ok(products);
            }
            else
            {
                // Se não está no cache, obtém do serviço
                var products = await _productService.GetAllAsync();

                // Serializa os dados e armazena no cache
                serializedProducts = JsonConvert.SerializeObject(products);

                // Define as opções de cache, como tempo de expiração
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                await _cache.SetStringAsync(cacheKey, serializedProducts, options);

                return Ok(products);
            }
        }

        [HttpGet("get-all-nocache")]
        public async Task<IActionResult> GetAllNoCache()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "get-by-id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Produto não encontrado. Erro: {ex.Message}");
            }
        }

        [HttpGet("{name}", Name = "check-if-exists")]
        public async Task<bool> CheckIfExists(string name)
        {
            try
            {
                var result = await _productService.CheckIfExists(name);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro: {ex.Message}");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id:int}", Name = "update")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != product.Id)
                return BadRequest("ID do produto não corresponde ao ID da URL.");

            try
            {
                var updatedProduct = await _productService.UpdateAsync(product);
                return Ok(updatedProduct);
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Produto não encontrado. Error message: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}", Name = "delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _productService.DeleteAsync(id);
                if (!deleted)
                    return NotFound("Produto não encontrado.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
