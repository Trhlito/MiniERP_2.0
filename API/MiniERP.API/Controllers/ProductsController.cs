using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Products;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// Controller pro produkty
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // Service vrstva pro produkty
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // Načtení seznamu produktů
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    // Vytvoření nového produktu
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        // Vrácení validačních chyb
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var newProductId = await _productService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = newProductId },
            new { id = newProductId });
    }

    // Načtení detailu produktu
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    // Úprava existujícího produktu
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
    {
        // Vrácení validačních chyb
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated = await _productService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    // Smazání produktu podle ID
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}