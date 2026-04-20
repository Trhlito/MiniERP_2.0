using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Stock;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// -- Controller pro sklad --
[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    // -- Service vrstva pro sklad --
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    // -- Endpoint pro načtení seznamu skladových záznamů --
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stock = await _stockService.GetAllAsync();
        return Ok(stock);
    }

    // -- Endpoint pro načtení detailu skladového záznamu podle ID --
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var stockItem = await _stockService.GetByIdAsync(id);

        if (stockItem == null)
        {
            return NotFound();
        }

        return Ok(stockItem);
    }

    // -- Endpoint pro vytvoření skladového záznamu --
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequest request)
    {
        // -- Pokud validace neprošla, vrátíme chyby --
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var newStockId = await _stockService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = newStockId },
            new { id = newStockId });
    }

    // -- Endpoint pro úpravu skladového záznamu --
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStockRequest request)
    {
        // -- Pokud validace neprošla, vrátíme chyby --
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated = await _stockService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    // -- Endpoint pro smazání skladového záznamu --
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _stockService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}