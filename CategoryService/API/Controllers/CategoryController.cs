using Application.CategoryService.Application.Interfaces;
using Application.CategoryService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.CategoryService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpGet("{id:int}", Name = "get-by-id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Categoria não encontrada. Error message: {ex.Message}");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCategory = await _categoryService.CreateAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id:int}", Name = "update")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != category.Id)
                return BadRequest("ID da categoria não corresponde ao ID da URL.");

            try
            {
                var updatedCategory = await _categoryService.UpdateAsync(category);
                return Ok(updatedCategory);
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Categoria não encontrada. Error message: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}", Name = "delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _categoryService.DeleteAsync(id);
                if (!deleted)
                    return NotFound("Categoria não encontrada.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

