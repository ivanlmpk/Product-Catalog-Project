using Application.ProductService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.ProductService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        //[HttpPost("create")]
        //public async Task<IActionResult> Create(Product userProduct)
        //{
        //    if (userProduct == null) return BadRequest("Preencha os dados para cadastrar o produto.");
        //    //var result = await _productInterface.Create(userProduct);

        //    return Ok(result);
        //}
    }
}
